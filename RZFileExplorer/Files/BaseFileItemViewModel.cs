using System;
using System.IO;
using System.Windows.Input;
using REghZy.MVVM.Commands;
using REghZy.MVVM.ViewModels;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public abstract class BaseFileItemViewModel : BaseViewModel {
        private FileExplorerViewModel fileExplorer;
        public FileExplorerViewModel FileExplorer {
            get => this.fileExplorer;
            set => this.fileExplorer = value ?? throw new ArgumentNullException(nameof(value), "File Explorer cannot be null");
        }

        public abstract bool IsFile { get; }
        public abstract bool IsDirectory { get; }
        public abstract bool IsDrive { get; }
        public abstract bool Exists { get; }

        private string filePath;

        public string FilePath {
            get => this.filePath;
            set => RaisePropertyChanged(ref this.filePath, value);
        }

        public ICommand NavigateCommand { get; }

        protected BaseFileItemViewModel(FileExplorerViewModel fileExplorer, string path = null) {
            this.FileExplorer = fileExplorer;
            this.FilePath = path;
            this.NavigateCommand = new RelayCommand(() => this.FileExplorer.Navigate(this));
            this.Update();
        }

        public bool ExistsAsFile() {
            return File.Exists(this.filePath);
        }

        public bool ExistsAsDirectory() {
            return Directory.Exists(this.filePath);
        }

        public abstract void Update();
    }
}