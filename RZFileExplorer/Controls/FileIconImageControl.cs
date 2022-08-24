using System.Drawing;
using System.Windows;
using System.Windows.Media;
using RZFileExplorer.Icons;
using Image = System.Windows.Controls.Image;

namespace RZFileExplorer.Files.Controls {
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

        public ImageSource ImageSource {
            get => this.Source;
            set => this.Source = value;
        }

        private static void OnTargetFilePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((FileIconImageControl) d).OnTargetFileChanged();
        }

        private bool queuedLoadForXamlLoad;

        public FileIconImageControl() {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (this.queuedLoadForXamlLoad) {
                this.queuedLoadForXamlLoad = false;
                OnTargetFileChanged();
            }
        }

        public void OnTargetFileChanged() {
            if (this.IsLoaded) {
                FileIconService.Instance.EnqueueForIconResolution(this.TargetFilePath, this, false, false, this.IconType);
            }
            else {
                this.queuedLoadForXamlLoad = true;
            }
        }
    }
}