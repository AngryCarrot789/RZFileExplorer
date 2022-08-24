using System.Windows.Controls;
using System.Windows.Input;

namespace RZFileExplorer.Files.Controls {
    public class BaseFileItemControl : Control {
        public BaseFileItemControl() {

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
}