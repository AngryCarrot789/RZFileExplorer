using System.Windows;
using RZFileExplorer.Files;
using RZFileExplorer.Icons;
using Image = System.Windows.Controls.Image;

namespace RZFileExplorer.Controls {
    public class FileIconImageControl : Image, IImageable {
        public static readonly DependencyProperty TargetFilePathProperty =
            DependencyProperty.Register(
                "TargetFilePath",
                typeof(string),
                typeof(FileIconImageControl),
                new PropertyMetadata(null, OnTargetFilePathPropertyChanged));

        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register(
                "IconType",
                typeof(IconType),
                typeof(FileIconImageControl),
                new FrameworkPropertyMetadata(IconType.Normal, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        }

        public string TargetFilePath {
            get => (string) GetValue(TargetFilePathProperty);
            set => SetValue(TargetFilePathProperty, value);
        }

        public IconType IconType {
            get => (IconType) GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        private static void OnTargetFilePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((FileIconImageControl) d).OnTargetFileChanged();
        }

        private bool triggerUpdateOnLoad;

        public FileIconImageControl() {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (this.triggerUpdateOnLoad) {
                this.triggerUpdateOnLoad = false;
                OnTargetFileChanged();
            }
        }

        public void OnTargetFileChanged() {
            if (this.IsLoaded) {
                FileIconService.Instance.EnqueueForIconResolution(this.TargetFilePath, this, false, false, this.IconType);
                this.triggerUpdateOnLoad = false;
            }
            else {
                this.triggerUpdateOnLoad = true;
            }
        }
    }
}