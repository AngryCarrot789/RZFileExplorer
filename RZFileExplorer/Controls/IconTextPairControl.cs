using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RZFileExplorer.Icons;

namespace RZFileExplorer.Files.Controls {
    public class IconTextPairControl : Control, IImageable {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(
                "ImageSource",
                typeof(ImageSource),
                typeof(IconTextPairControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(IconTextPairControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.None, (d, e) => { }, (a, b) => b == null ? "" : b.ToString()));

        public static readonly DependencyProperty TargetFilePathProperty =
            DependencyProperty.Register(
                "TargetFilePath",
                typeof(string),
                typeof(IconTextPairControl),
                new PropertyMetadata(null, OnTargetFilePathPropertyChanged));

        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register(
                "IconType",
                typeof(IconType),
                typeof(IconTextPairControl),
                new PropertyMetadata(IconType.Normal));

        public IconType IconType {
            get => (IconType) GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        private static void OnTargetFilePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((IconTextPairControl) d).OnTargetFileChanged();
        }

        public string TargetFilePath {
            get => (string) GetValue(TargetFilePathProperty);
            set => SetValue(TargetFilePathProperty, value);
        }

        public ImageSource ImageSource {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string Text {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private bool queuedLoadForXamlLoad;

        public IconTextPairControl() {
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