using System;
using System.IO;
using RZFileExplorer.Icons;

namespace RZFileExplorer.Files.QuickAccessing {
    public class QuickAccessItemViewModel : StaticFileItemViewModel {
        private CSIDL systemIconType;
        public CSIDL SystemIconType {
            get => this.systemIconType;
            set => RaisePropertyChanged(ref this.systemIconType, value);
        }

        private string displayName;

        public string DisplayName {
            get => this.displayName;
            set => RaisePropertyChanged(ref this.displayName, value);
        }

        public static QuickAccessItemViewModel Favorites => new QuickAccessItemViewModel(Environment.SpecialFolder.Favorites, CSIDL.CSIDL_COMMON_FAVORITES);
        public static QuickAccessItemViewModel Desktop => new QuickAccessItemViewModel(Environment.SpecialFolder.Desktop, CSIDL.CSIDL_DESKTOP);
        public static QuickAccessItemViewModel MyDocuments => new QuickAccessItemViewModel(Environment.SpecialFolder.MyDocuments, CSIDL.CSIDL_PERSONAL);
        public static QuickAccessItemViewModel MyPictures => new QuickAccessItemViewModel(Environment.SpecialFolder.MyPictures, CSIDL.CSIDL_MYPICTURES);
        public static QuickAccessItemViewModel MyVideos => new QuickAccessItemViewModel(Environment.SpecialFolder.MyVideos, CSIDL.CSIDL_FAVORITES);
        public static QuickAccessItemViewModel MyMusic => new QuickAccessItemViewModel(Environment.SpecialFolder.MyMusic, CSIDL.CSIDL_MYMUSIC);
        public static QuickAccessItemViewModel History => new QuickAccessItemViewModel(Environment.SpecialFolder.History, CSIDL.CSIDL_HISTORY);
        public static QuickAccessItemViewModel Fonts => new QuickAccessItemViewModel(Environment.SpecialFolder.Fonts, CSIDL.CSIDL_FONTS);
        public static QuickAccessItemViewModel Recent => new QuickAccessItemViewModel(Environment.SpecialFolder.Recent, CSIDL.CSIDL_RECENT);
        public static QuickAccessItemViewModel PrinterShortcuts => new QuickAccessItemViewModel(Environment.SpecialFolder.PrinterShortcuts, CSIDL.CSIDL_PRINTERS);
        public static QuickAccessItemViewModel NetworkShortcuts1 => new QuickAccessItemViewModel(Environment.SpecialFolder.NetworkShortcuts, CSIDL.CSIDL_CONNECTIONS);
        public static QuickAccessItemViewModel NetworkShortcuts2 => new QuickAccessItemViewModel(Environment.SpecialFolder.NetworkShortcuts, CSIDL.CSIDL_NETWORK);

        public QuickAccessItemViewModel() {

        }

        public QuickAccessItemViewModel(string filePath) : base(filePath) {

        }

        public QuickAccessItemViewModel(CSIDL systemIconType) {
            this.SystemIconType = systemIconType;
        }

        public QuickAccessItemViewModel(string filePath, CSIDL systemIconType) : base(filePath) {
            this.DisplayName = Path.GetFileName(filePath);
            this.SystemIconType = systemIconType;
        }

        public QuickAccessItemViewModel(Environment.SpecialFolder specialFolder, CSIDL systemIconType) : base(Environment.GetFolderPath(specialFolder)) {
            this.SystemIconType = systemIconType;
            this.DisplayName = specialFolder.ToString();
        }
    }
}