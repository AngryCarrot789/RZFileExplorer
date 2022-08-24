using System.IO;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public class DriveItemViewModel : BaseDirectoryItemViewModel {
        public sealed override bool IsFile      => false;
        public sealed override bool IsDirectory => false;
        public sealed override bool IsDrive     => true;

        public sealed override bool Exists     => ExistsAsDirectory();

        private long totalSpace;
        public long TotalSpace {
            get => this.totalSpace;
            set => RaisePropertyChanged(ref this.totalSpace, value);
        }

        private long usedSpace;
        public long UsedSpace {
            get => this.usedSpace;
            set => RaisePropertyChanged(ref this.usedSpace, value);
        }

        public long RemainingSpace => this.totalSpace - this.usedSpace;

        public DriveItemViewModel(FileExplorerViewModel fileExplorer, string path) : base(fileExplorer, path) {

        }

        public override void Update() {
            DriveInfo info = new DriveInfo(this.FilePath);
            if (info.IsReady) {
                this.TotalSpace = info.TotalSize;
                this.UsedSpace = this.TotalSpace - info.TotalFreeSpace;
            }
        }
    }
}