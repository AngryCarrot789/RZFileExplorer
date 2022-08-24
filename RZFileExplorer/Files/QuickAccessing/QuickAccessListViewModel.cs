using System.Collections.ObjectModel;
using REghZy.MVVM.ViewModels;
using RZFileExplorer.Icons;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer.Files.QuickAccessing {
    public class QuickAccessListViewModel : BaseViewModel {
        public FileExplorerViewModel FileExplorer { get; }

        public ObservableCollection<QuickAccessItemViewModel> QuickAccessList { get; }

        private QuickAccessItemViewModel selectedItem;
        public QuickAccessItemViewModel SelectedItem {
            get => this.selectedItem;
            set {
                RaisePropertyChanged(ref this.selectedItem, value);
                NavigateToItem(value);
            }
        }

        public QuickAccessListViewModel(FileExplorerViewModel fileExplorer) {
            this.FileExplorer = fileExplorer;
            this.QuickAccessList = new ObservableCollection<QuickAccessItemViewModel> {
                QuickAccessItemViewModel.Favorites,
                QuickAccessItemViewModel.Desktop,
                QuickAccessItemViewModel.MyDocuments,
                QuickAccessItemViewModel.MyPictures,
                QuickAccessItemViewModel.MyVideos,
                QuickAccessItemViewModel.MyMusic,
                QuickAccessItemViewModel.History,
                QuickAccessItemViewModel.Fonts,
                QuickAccessItemViewModel.Recent,
                QuickAccessItemViewModel.PrinterShortcuts,
                // QuickAccessItemViewModel.NetworkShortcuts1,
                QuickAccessItemViewModel.NetworkShortcuts2
            };

            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_ADMINTOOLS", CSIDL.CSIDL_ADMINTOOLS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_ALTSTARTUP", CSIDL.CSIDL_ALTSTARTUP));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_APPDATA", CSIDL.CSIDL_APPDATA));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_BITBUCKET", CSIDL.CSIDL_BITBUCKET));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_CDBURN_AREA", CSIDL.CSIDL_CDBURN_AREA));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_ADMINTOOLS", CSIDL.CSIDL_COMMON_ADMINTOOLS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_ALTSTARTUP", CSIDL.CSIDL_COMMON_ALTSTARTUP));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_APPDATA", CSIDL.CSIDL_COMMON_APPDATA));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_DESKTOPDIRECTORY", CSIDL.CSIDL_COMMON_DESKTOPDIRECTORY));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_DOCUMENTS", CSIDL.CSIDL_COMMON_DOCUMENTS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_FAVORITES", CSIDL.CSIDL_COMMON_FAVORITES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_MUSIC", CSIDL.CSIDL_COMMON_MUSIC));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_OEM_LINKS", CSIDL.CSIDL_COMMON_OEM_LINKS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_PICTURES", CSIDL.CSIDL_COMMON_PICTURES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_PROGRAMS", CSIDL.CSIDL_COMMON_PROGRAMS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_STARTMENU", CSIDL.CSIDL_COMMON_STARTMENU));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_STARTUP", CSIDL.CSIDL_COMMON_STARTUP));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_TEMPLATES", CSIDL.CSIDL_COMMON_TEMPLATES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMMON_VIDEO", CSIDL.CSIDL_COMMON_VIDEO));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COMPUTERSNEARME", CSIDL.CSIDL_COMPUTERSNEARME));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_CONNECTIONS", CSIDL.CSIDL_CONNECTIONS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_CONTROLS", CSIDL.CSIDL_CONTROLS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_COOKIES", CSIDL.CSIDL_COOKIES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_DESKTOP", CSIDL.CSIDL_DESKTOP));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_DESKTOPDIRECTORY", CSIDL.CSIDL_DESKTOPDIRECTORY));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_DRIVES", CSIDL.CSIDL_DRIVES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FAVORITES", CSIDL.CSIDL_FAVORITES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FLAG_CREATE", CSIDL.CSIDL_FLAG_CREATE));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FLAG_DONT_VERIFY", CSIDL.CSIDL_FLAG_DONT_VERIFY));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FLAG_MASK", CSIDL.CSIDL_FLAG_MASK));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FLAG_NO_ALIAS", CSIDL.CSIDL_FLAG_NO_ALIAS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FLAG_PER_USER_INIT", CSIDL.CSIDL_FLAG_PER_USER_INIT));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_FONTS", CSIDL.CSIDL_FONTS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_HISTORY", CSIDL.CSIDL_HISTORY));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_INTERNET", CSIDL.CSIDL_INTERNET));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_INTERNET_CACHE", CSIDL.CSIDL_INTERNET_CACHE));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_LOCAL_APPDATA", CSIDL.CSIDL_LOCAL_APPDATA));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_MYDOCUMENTS", CSIDL.CSIDL_MYDOCUMENTS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_MYMUSIC", CSIDL.CSIDL_MYMUSIC));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_MYPICTURES", CSIDL.CSIDL_MYPICTURES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_MYVIDEO", CSIDL.CSIDL_MYVIDEO));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_NETHOOD", CSIDL.CSIDL_NETHOOD));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_NETWORK", CSIDL.CSIDL_NETWORK));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PERSONAL", CSIDL.CSIDL_PERSONAL));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PRINTERS", CSIDL.CSIDL_PRINTERS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PRINTHOOD", CSIDL.CSIDL_PRINTHOOD));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROFILE", CSIDL.CSIDL_PROFILE));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROGRAM_FILES", CSIDL.CSIDL_PROGRAM_FILES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROGRAM_FILES_COMMON", CSIDL.CSIDL_PROGRAM_FILES_COMMON));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROGRAM_FILES_COMMONX86", CSIDL.CSIDL_PROGRAM_FILES_COMMONX86));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROGRAM_FILESX86", CSIDL.CSIDL_PROGRAM_FILESX86));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_PROGRAMS", CSIDL.CSIDL_PROGRAMS));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_RECENT", CSIDL.CSIDL_RECENT));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_RESOURCES", CSIDL.CSIDL_RESOURCES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_RESOURCES_LOCALIZED", CSIDL.CSIDL_RESOURCES_LOCALIZED));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_SENDTO", CSIDL.CSIDL_SENDTO));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_STARTMENU", CSIDL.CSIDL_STARTMENU));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_STARTUP", CSIDL.CSIDL_STARTUP));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_SYSTEM", CSIDL.CSIDL_SYSTEM));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_SYSTEMX86", CSIDL.CSIDL_SYSTEMX86));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_TEMPLATES", CSIDL.CSIDL_TEMPLATES));
            // this.QuickAccessList.Add(new QuickAccessItemViewModel("CSIDL_WINDOWS", CSIDL.CSIDL_WINDOWS));
        }

        private void NavigateToItem(QuickAccessItemViewModel value) {
            this.FileExplorer.Navigate(value.FilePath);
        }
    }
}