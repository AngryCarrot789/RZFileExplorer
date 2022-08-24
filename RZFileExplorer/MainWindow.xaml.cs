using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RZFileExplorer.Icons;
using RZFileExplorer.ViewModels;

namespace RZFileExplorer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = new FileExplorerViewModel();
            FileIconService.Init();
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            FileIconService.Instance.CanDirectoryThreadRun = false;
            FileIconService.Instance.CanFileThreadRun = false;
            FileIconService.Instance.CanUpdateTaskRun = false;
        }
    }
}
