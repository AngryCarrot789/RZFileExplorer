using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RZFileExplorer.Icons;

namespace RZFileExplorer.Files.Controls {
    public class IconTextPairControl : Control {
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

        public void OnTargetFileChanged() {
            FileIconService.Instance.EnqueueForIconFetch(this);
        }
    }
}