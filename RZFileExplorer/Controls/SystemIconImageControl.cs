using System.Windows;
using System.Windows.Controls;
using RZFileExplorer.Icons;

namespace RZFileExplorer.Controls {
    public class SystemIconImageControl : Control {
        public static readonly DependencyProperty SystemIconTypeProperty =
            DependencyProperty.Register(
                "SystemIconType",
                typeof(CSIDL),
                typeof(SystemIconImageControl),
                new FrameworkPropertyMetadata(CSIDL.CSIDL_DESKTOP, FrameworkPropertyMetadataOptions.AffectsRender));

        public CSIDL SystemIconType {
            get => (CSIDL) GetValue(SystemIconTypeProperty);
            set => SetValue(SystemIconTypeProperty, value);
        }
    }
}