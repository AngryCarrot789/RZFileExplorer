using REghZy.MVVM.ViewModels;
using RZFileExplorer.Files.QuickAccessing;

namespace RZFileExplorer.ViewModels {
    public class MainViewModel : BaseViewModel {
        public FileExplorerViewModel FileExplorer { get; }

        public QuickAccessListViewModel QuickAccess { get; }

        public MainViewModel() {
            this.FileExplorer = new FileExplorerViewModel();
            this.QuickAccess = new QuickAccessListViewModel(this.FileExplorer);
        }
    }
}