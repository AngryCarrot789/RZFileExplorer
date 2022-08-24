using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public ImageSource ImageSource {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string Text {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}