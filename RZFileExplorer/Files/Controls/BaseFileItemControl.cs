using System.Windows.Controls;
using System.Windows.Input;

namespace RZFileExplorer.Files.Controls {
    public abstract class BaseFileItemControl : Control {
        protected BaseFileItemControl() {

        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
            if (this.DataContext is BaseFileItemViewModel file && file.FileExplorer != null) {
                file.FileExplorer.Navigate(file);
            }
            else {
                base.OnMouseDoubleClick(e);
            }
        }
    }

    public class DriveWrapItemControl : BaseFileItemControl { }

    public class DirectoryWrapItemControl : BaseFileItemControl { }

    public class FileWrapItemControl : BaseFileItemControl { }
}