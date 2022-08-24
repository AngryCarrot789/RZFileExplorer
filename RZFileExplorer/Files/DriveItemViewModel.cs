using System.IO;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public class DriveItemViewModel : BaseDirectoryItemViewModel {
        public sealed override bool IsFile      => false;
        public sealed override bool IsDirectory => false;
        public sealed override bool IsDrive     => true;

        public sealed override bool Exists     => ExistsAsDirectory();

        private string formatType;
        public string FormatType {
            get => this.formatType;
            set => RaisePropertyChanged(ref this.formatType, value);
        }

        private string volumeLabel;
        public string VolumeLabel {
            get => this.volumeLabel;
            set => RaisePropertyChanged(ref this.volumeLabel, value);
        }

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

        public DriveItemViewModel(FileExplorerViewModel fileExplorer, DriveInfo info) : base(fileExplorer, info.Name) {
            this.VolumeLabel = info.VolumeLabel;
            this.FormatType = info.DriveFormat; // NTFS most of the time; for drives at least
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