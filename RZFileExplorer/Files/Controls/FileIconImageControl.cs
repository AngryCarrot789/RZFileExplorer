using System.Drawing;
using Image = System.Windows.Controls.Image;

namespace RZFileExplorer.Files.Controls {
    public class AsyncImageControl : Image {
        public static readonly DependencyProperty TargetFilePathProperty =
            DependencyProperty.Register(
                "TargetFilePath",
                typeof(string),
                typeof(IconTextPairControl),
                new PropertyMetadata(null, OnTargetFilePathPropertyChanged));
    }
}