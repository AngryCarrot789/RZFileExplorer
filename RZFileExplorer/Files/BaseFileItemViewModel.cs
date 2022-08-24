using System;
using System.IO;
using System.Windows;
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

        public bool IgnoreRenameOnFileNameChanged { get; set; }

        private string filePath;
        public string FilePath {
            get => this.filePath;
            set {
                RaisePropertyChanged(ref this.filePath, value);
                this.IgnoreRenameOnFileNameChanged = true;
                this.FileName = Path.GetFileName(value);
                this.IgnoreRenameOnFileNameChanged = false;
            }
        }

        private string fileName;
        public string FileName {
            get => this.fileName;
            set {
                RaisePropertyChanged(ref this.fileName, value);
                if (!this.IgnoreRenameOnFileNameChanged) {
                    DoRename();
                }
            }
        }

        private bool isHidden;
        public bool IsHidden {
            get => this.isHidden;
            set => RaisePropertyChanged(ref this.isHidden, value);
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

        public virtual void DoRename() {
            MessageBox.Show("Rename not implemented yet", "Rename", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}