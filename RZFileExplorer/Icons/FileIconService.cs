using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RZFileExplorer.Files;
using RZFileExplorer.Files.Controls;

namespace RZFileExplorer.Icons {
    public class FileIconService {
        private struct PathControlPair {
            public readonly string path;
            public readonly IconTextPairControl control;

            public PathControlPair(IconTextPairControl control) {
                this.control = control;
                this.path = control.TargetFilePath;
            }
        }

        private struct ControlImagePair {
            public readonly IconTextPairControl control;
            public readonly string path;
            public readonly ImageSource image;

            public ControlImagePair(IconTextPairControl control, string path, ImageSource image) {
                this.control = control;
                this.path = path;
                this.image = image;
            }

            public void Set() {
                this.control.ImageSource = this.image;
            }
        }

        public static FileIconService Instance { get; } = new FileIconService();

        private readonly Thread fileThread;
        private readonly Thread directoryThread;
        private readonly ConcurrentQueue<PathControlPair> fileQueue;
        private readonly ConcurrentQueue<PathControlPair> directoryQueue;
        private readonly ConcurrentQueue<ControlImagePair> updateQueue;

        private readonly IconCache cache;

        private volatile bool canFileThreadRun;
        public bool CanFileThreadRun {
            get => this.canFileThreadRun;
            set => this.canFileThreadRun = value;
        }

        private volatile bool canDirectoryThreadRun;
        public bool CanDirectoryThreadRun {
            get => this.canDirectoryThreadRun;
            set => this.canDirectoryThreadRun = value;
        }

        private volatile bool canUpdateTaskRun;
        public bool CanUpdateTaskRun {
            get => this.canUpdateTaskRun;
            set => this.canUpdateTaskRun = value;
        }

        public FileIconService() {
            this.fileQueue = new ConcurrentQueue<PathControlPair>();
            this.directoryQueue = new ConcurrentQueue<PathControlPair>();
            this.updateQueue = new ConcurrentQueue<ControlImagePair>();
            this.cache = new IconCache();

            this.canFileThreadRun = true;
            this.canDirectoryThreadRun = true;
            this.canUpdateTaskRun = true;

            this.fileThread = new Thread(this.FileQueueThreadMain);
            this.directoryThread = new Thread(this.DirectoryQueueThreadMain);

            this.fileThread.Start();
            this.directoryThread.Start();

            Task.Run(async () => {
                while (this.canUpdateTaskRun) {
                    int size = Math.Min(this.updateQueue.Count, 20);
                    if (size > 0) {
                        Application.Current.Dispatcher.Invoke(() => {
                            for (int i = 0; i < size; i++) {
                                if (this.updateQueue.TryDequeue(out ControlImagePair pair)) {
                                    pair.Set();
                                    this.cache.PutImage(pair.path, pair.image);
                                }
                                else {
                                    break;
                                }
                            }
                        });
                    }

                    await Task.Delay(50);
                }
            });
            Task.Run(async () => {
                while (this.canUpdateTaskRun) {
                    this.cache.Tick();
                    await Task.Delay(500);
                }
            });
        }

        public static void Init() {

        }

        private void FileQueueThreadMain() {
            while (this.canFileThreadRun) {
                int count = Math.Min(this.fileQueue.Count, 5);
                if (count > 0) {
                    Application.Current.Dispatcher.Invoke(() => {
                        for (int i = 0; i < count; i++) {
                            if (this.fileQueue.TryDequeue(out PathControlPair control)) {
                                string path = control.path;
                                if (File.Exists(path)) {
                                    this.updateQueue.Enqueue(new ControlImagePair(control.control, path, IconService.GetFileIconAsBitmapSource(path, true, false)));
                                }
                            }
                        }
                    });
                }

                Thread.Sleep(10);
            }
        }

        private void DirectoryQueueThreadMain() {
            while (this.canDirectoryThreadRun) {
                int count = Math.Min(this.directoryQueue.Count, 5);
                if (count > 0) {
                    Application.Current.Dispatcher.Invoke(() => {
                        for (int i = 0; i < count; i++) {
                            if (this.directoryQueue.TryDequeue(out PathControlPair control)) {
                                string path = control.path;
                                if (Directory.Exists(path)) {
                                    this.updateQueue.Enqueue(new ControlImagePair(control.control, path, IconService.GetFileIconAsBitmapSource(path, true, true)));
                                }
                            }
                        }
                    });
                }

                Thread.Sleep(10);
            }
        }

        public void EnqueueForIconFetch(IconTextPairControl control, bool forceFile = false, bool forceDirectory = false) {
            if (forceFile || forceDirectory) {
                if (forceFile) {
                    this.fileQueue.Enqueue(new PathControlPair(control));
                }
                else {
                    this.directoryQueue.Enqueue(new PathControlPair(control));
                }
            }
            else {
                string path = control.TargetFilePath;
                if (this.cache.TryGetImage(path, out ImageSource source)) {
                    this.updateQueue.Enqueue(new ControlImagePair(control, path, source));
                }
                else if (File.Exists(path)) {
                    this.fileQueue.Enqueue(new PathControlPair(control));
                }
                else if (Directory.Exists(path)) {
                    this.directoryQueue.Enqueue(new PathControlPair(control));
                }
            }
        }
    }
}