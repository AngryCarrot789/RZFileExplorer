using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using REghZy.MVVM.Commands;
using REghZy.MVVM.ViewModels;
using RZFileExplorer.Exploring;
using RZFileExplorer.Files;

namespace RZFileExplorer.ViewModels {
    public class FileExplorerViewModel : BaseViewModel {
        private string inputPath;
        public string InputPath {
            get => this.inputPath;
            set => RaisePropertyChanged(ref this.inputPath, value);
        }

        private string previousDirectory;
        private string currentDirectory;
        public string CurrentDirectory {
            get => this.currentDirectory;
            set {
                this.previousDirectory = this.currentDirectory;
                RaisePropertyChanged(ref this.currentDirectory, value);
            }
        }

        private ExplorerMode explorerMode;
        public ExplorerMode ExplorerMode {
            get => this.explorerMode;
            set => RaisePropertyChanged(ref this.explorerMode, value);
        }

        public ICommand NavigateToTopInputCommand { get; }

        public ICommand RefreshCommand { get; }

        public ObservableCollection<BaseFileItemViewModel> Files { get; }

        public RelayCommandParam<BaseFileItemViewModel> NavigateToItemCommand { get; }
        public RelayCommandParam<string> NavigateToPathCommand { get; }

        public HistoryViewModel History { get; }

        public FileExplorerViewModel() {
            this.History = new HistoryViewModel(this);
            this.Files = new ObservableCollection<BaseFileItemViewModel>();
            this.NavigateToItemCommand = new RelayCommandParam<BaseFileItemViewModel>(this.Navigate);
            this.NavigateToPathCommand = new RelayCommandParam<string>((a) => this.Navigate(a));
            this.NavigateToTopInputCommand = new RelayCommand(() => this.Navigate(this.InputPath));
            this.RefreshCommand = new RelayCommand(this.RefreshCurrentFolder);
            ClearListAndShowDrives(false);
        }

        public void Navigate(string input, bool appendHistory = true) {
            if (string.IsNullOrEmpty(input)) {
                ClearListAndShowDrives(appendHistory);
            }
            else if (Directory.Exists(input)) {
                NavigateToDirectory(input, appendHistory);
            }
            else if (File.Exists(input)) {
                OpenFile(input);
            }
            else {
                MessageBox.Show($"No such directory or file: {input}", "Path is not a directory or file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void Navigate(BaseFileItemViewModel baseFile) {
            if (baseFile is FileItemViewModel file) {
                if (file.ExistsAsFile()) {
                    OpenFile(file.FilePath);
                }
                else {
                    MessageBox.Show($"No such directory or file: {file.FilePath}", "Path is not a directory or file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else if (baseFile is DirectoryItemViewModel || baseFile is DriveItemViewModel) {
                NavigateToDirectory(baseFile.FilePath);
            }
            else if (baseFile == null) {
                ClearListAndShowDrives();
            }
            else if (Debugger.IsAttached) {
                MessageBox.Show("Unknown base file type: " + baseFile.GetType() + " -> " + baseFile);
            }
        }

        public void PreNavigation(string target, bool appendHistory = true) {
            if (appendHistory) {
                this.History.OnNavigate(this.currentDirectory);
            }

            this.CurrentDirectory = target;
        }

        public void ClearListAndShowDrives(bool appendHistory = true) {
            SetHeaderPath(null);
            PreNavigation(null, appendHistory);
            this.Files.Clear();
            this.ExplorerMode = ExplorerMode.Wrap;
            foreach (DriveInfo info in DriveInfo.GetDrives()) {
                this.Files.Add(new DriveItemViewModel(this, info));
            }
        }

        public void NavigateToDirectory(string path, bool appendHistory = true) {
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryInfo[] directories;
            FileInfo[] files;
            try {
                directories = directory.GetDirectories();
                files = directory.GetFiles();
            }
            catch (UnauthorizedAccessException) {
                MessageBox.Show($"Access denied to folder:\n\n{path}", "Access denied", MessageBoxButton.OK, MessageBoxImage.Error);
                SetHeaderPath(this.CurrentDirectory);
                return;
            }
            #if !DEBUG
            catch (SecurityException securityException) {
                MessageBox.Show($"Security error while accessing folder {path}:\n{securityException}", "Security error", MessageBoxButton.OK, MessageBoxImage.Warning);
                SetHeaderPath(this.CurrentDirectory);
                return;
            }
            catch (Exception e) {
                MessageBox.Show($"Unexpected error while accessing folder {path}: {e}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                SetHeaderPath(this.CurrentDirectory);
                return;
            }
            #endif

            PreNavigation(path, appendHistory);
            SetHeaderPath(path);
            this.Files.Clear();
            this.ExplorerMode = ExplorerMode.List;
            foreach (DirectoryInfo info in directories) {
                this.Files.Add(new DirectoryItemViewModel(this, info.FullName));
            }

            foreach (FileInfo file in files) {
                this.Files.Add(new FileItemViewModel(this, file.FullName));
            }
        }

        public void OpenFile(string path) {
            if (File.Exists(path)) {
                Process.Start(path);
            }
            else {
                MessageBox.Show($"File does not exist: {path}", "Cannot open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // MessageBox.Show($"You tried to open: {path}", "Open file", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SetHeaderPath(string path) {
            this.InputPath = path;
        }

        public void RefreshCurrentFolder() {
            if (Directory.Exists(this.CurrentDirectory)) {
                this.Navigate(this.CurrentDirectory, false);
            }
            else if (this.CurrentDirectory != null) {
                MessageBoxResult result = MessageBox.Show($"The current directory no longer exists. Would you like to find the nearest existing sub-directory?\n\n{this.CurrentDirectory}", "Directory no longer exists", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    string path = this.CurrentDirectory;
                    while (path != null && !Directory.Exists(path)) {
                        path = Path.GetDirectoryName(path); // gets the parent of the given file or directory
                    }

                    Navigate(path, true);
                }
            }
            else {
                ClearListAndShowDrives(false);
            }
        }
    }
}