using System.Collections.Generic;
using System.IO;
using System.Linq;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files {
    public abstract class BaseDirectoryItemViewModel : BaseFileItemViewModel {
        public override bool IsFile => false;
        public override bool Exists     => ExistsAsDirectory();

        protected BaseDirectoryItemViewModel(FileExplorerViewModel fileExplorer, string path = null) : base(fileExplorer, path) {

        }

        public IEnumerable<DirectoryItemViewModel> EnumerateDirectories() {
            if (!Directory.Exists(this.FilePath)) {
                throw new DirectoryNotFoundException(this.FilePath);
            }

            return Directory.EnumerateDirectories(this.FilePath).Select(a => new DirectoryItemViewModel(this.FileExplorer, a));
        }

        public IEnumerable<FileItemViewModel> EnumerateFiles() {
            if (!Directory.Exists(this.FilePath)) {
                throw new DirectoryNotFoundException(this.FilePath);
            }

            return Directory.EnumerateFiles(this.FilePath).Select(a => new FileItemViewModel(this.FileExplorer, a));
        }
    }
}