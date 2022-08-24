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
        private readonly struct QueuedIconResolution {
            public readonly string path;
            public readonly IImageable imageable;
            public readonly IconType iconType;

            public QueuedIconResolution(string path, IImageable imageable, IconType iconType) {
                this.path = path;
                this.imageable = imageable;
                this.iconType = iconType;
            }
        }

        private readonly struct PendingIconDelivery {
            public readonly IImageable imageable;
            public readonly string path;
            public readonly ImageSource image;

            public PendingIconDelivery(string path, IImageable imageable, ImageSource image) {
                this.path = path;
                this.imageable = imageable;
                this.image = image;
            }

            public void SetImage() {
                this.imageable.ImageSource = this.image;
            }
        }

        public static FileIconService Instance { get; } = new FileIconService();

        private readonly Thread fileThread;
        private readonly Thread directoryThread;
        private readonly ConcurrentQueue<QueuedIconResolution> fileQueue;
        private readonly ConcurrentQueue<QueuedIconResolution> directoryQueue;
        private readonly ConcurrentQueue<PendingIconDelivery> updateQueue;

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
            this.fileQueue = new ConcurrentQueue<QueuedIconResolution>();
            this.directoryQueue = new ConcurrentQueue<QueuedIconResolution>();
            this.updateQueue = new ConcurrentQueue<PendingIconDelivery>();
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
                                if (this.updateQueue.TryDequeue(out PendingIconDelivery pair)) {
                                    pair.SetImage();
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
                            if (this.fileQueue.TryDequeue(out QueuedIconResolution control)) {
                                string path = control.path;
                                if (File.Exists(path)) {
                                    this.updateQueue.Enqueue(new PendingIconDelivery(path, control.imageable, ShellUtils.GetFileIconAsBitmapSource(path, control.iconType, false)));
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
                            if (this.directoryQueue.TryDequeue(out QueuedIconResolution control)) {
                                string path = control.path;
                                if (Directory.Exists(path)) {
                                    BitmapSource source;
                                    if (control.iconType == IconType.Large) {
                                        source = ShellEx.icon_of_path_large(path, false, true);
                                    }
                                    else {
                                        source = ShellUtils.GetFileIconAsBitmapSource(path, control.iconType, true);
                                    }

                                    this.updateQueue.Enqueue(new PendingIconDelivery(path, control.imageable, source));
                                }
                            }
                        }
                    });
                }

                Thread.Sleep(10);
            }
        }

        public void EnqueueForIconResolution(string path, IImageable control, bool forceFile = false, bool forceDirectory = false, IconType iconType = IconType.Normal) {
            if (forceFile || forceDirectory) {
                if (forceFile) {
                    this.fileQueue.Enqueue(new QueuedIconResolution(path, control, iconType));
                }
                else {
                    this.directoryQueue.Enqueue(new QueuedIconResolution(path, control, iconType));
                }
            }
            else if (!string.IsNullOrEmpty(path)) {
                if (this.cache.TryGetImage(path, out ImageSource source)) {
                    this.updateQueue.Enqueue(new PendingIconDelivery(path, control, source));
                }
                else if (File.Exists(path)) {
                    this.fileQueue.Enqueue(new QueuedIconResolution(path, control, iconType));
                }
                else if (Directory.Exists(path)) {
                    this.directoryQueue.Enqueue(new QueuedIconResolution(path, control, iconType));
                }
            }
        }
    }
}