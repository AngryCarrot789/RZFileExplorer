using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using REghZy.MVVM.Commands;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public class DirectoryItemViewModel : BaseDirectoryItemViewModel {
        public override bool IsDirectory => true;
        public override bool IsDrive     => false;

        public ICommand PromptCalculateSizeCommand { get; }

        private bool hasCalculatedSize;
        public bool HasCalculatedSize {
            get => this.hasCalculatedSize;
            set => RaisePropertyChangedCheckEqual(ref this.hasCalculatedSize, value);
        }

        private long calculatedSize;
        public long CalculatedSize {
            get => this.calculatedSize;
            set => RaisePropertyChanged(ref this.calculatedSize, value);
        }

        public DirectoryItemViewModel(FileExplorerViewModel fileExplorer, string path) : base(fileExplorer, path) {
            this.CalculatedSize = -1;
            this.PromptCalculateSizeCommand = new RelayCommand(PromptCalculateDirectorySizeAction);
        }

        public override void Update() {
            DirectoryInfo info = new DirectoryInfo(this.FilePath);
            if (info.Exists) {

            }
        }

        public void PromptCalculateDirectorySizeAction() {
            if (MessageBox.Show($"Calculate the ENTIRE directory size, including all sub-directories?\n{this.FilePath}", "Calculate ENTIRE directory size", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) {
                DirectoryInfo info = new DirectoryInfo(this.FilePath);
                if (info.Exists) {
                    this.CalculatedSize = CalculateDirectorySize(info);
                    this.HasCalculatedSize = true;
                }
                else {
                    this.CalculatedSize = -1;
                    MessageBox.Show($"The directory no longer exists\n{this.FilePath}", "Directory not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static long CalculateDirectorySize(DirectoryInfo info) {
            long size = 0L;
            try {
                foreach (DirectoryInfo directory in info.EnumerateDirectories()) {
                    size += CalculateDirectorySize(directory);
                }
            }
            catch (UnauthorizedAccessException) { }

            try {
                foreach (FileInfo file in info.EnumerateFiles()) {
                    size += file.Length;
                }
            }
            catch (UnauthorizedAccessException) { }

            return size;
        }
    }
}