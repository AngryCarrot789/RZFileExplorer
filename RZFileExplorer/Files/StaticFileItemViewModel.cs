using System.IO;
using REghZy.MVVM.ViewModels;

namespace RZFileExplorer.Files {
    public class StaticFileItemViewModel : BaseViewModel {
        private string filePath;
        public string FilePath {
            get => this.filePath;
            set => RaisePropertyChanged(ref this.filePath, value);
        }

        public FileInfo FileInfo => new FileInfo(this.FilePath);

        public StaticFileItemViewModel() {

        }

        public StaticFileItemViewModel(string filePath) {
            this.FilePath = filePath;
        }

        public bool Exists() {
            return File.Exists(this.filePath);
        }
    }
}