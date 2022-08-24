using System.IO;
using System.Windows.Input;
using REghZy.MVVM.Commands;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public class FileItemViewModel : BaseFileItemViewModel {
        public sealed override bool IsFile      => true;
        public sealed override bool IsDirectory => false;
        public sealed override bool IsDrive     => false;

        public sealed override bool Exists     => ExistsAsFile();

        private long fileSize;
        public long FileSize {
            get => this.fileSize;
            set => RaisePropertyChanged(ref this.fileSize, value);
        }

        public FileInfo Info => new FileInfo(this.FilePath);

        public FileItemViewModel(FileExplorerViewModel fileExplorer) : base(fileExplorer) {

        }

        public FileItemViewModel(FileExplorerViewModel fileExplorer, string path) : base(fileExplorer, path) {

        }

        public override void Update() {
            FileInfo info = new FileInfo(this.FilePath);
            if (info.Exists) {
                this.FileSize = info.Length;
            }
        }
    }
}