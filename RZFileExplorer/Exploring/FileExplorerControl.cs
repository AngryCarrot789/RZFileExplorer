using System.Windows;
using System.Windows.Controls;

namespace RZFileExplorer.Exploring {
    public class FileExplorerControl : ItemsControl {
        public static readonly DependencyProperty ExplorerModeProperty =
            DependencyProperty.Register(
                "ExplorerMode",
                typeof(ExplorerMode),
                typeof(FileExplorerControl),
                new FrameworkPropertyMetadata(ExplorerMode.Wrap, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ExplorerMode ExplorerMode {
            get => (ExplorerMode) GetValue(ExplorerModeProperty);
            set => SetValue(ExplorerModeProperty, value);
        }
    }
}