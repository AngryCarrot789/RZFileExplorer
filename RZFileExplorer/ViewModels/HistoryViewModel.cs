using System.Collections.Generic;
using System.Windows.Input;
using REghZy.MVVM.Commands;

namespace RZFileExplorer.ViewModels {
    public class HistoryViewModel {
        private readonly Stack<string> undo;
        private readonly Stack<string> redo;

        public IEnumerable<string> Undo => this.undo;
        public IEnumerable<string> Redo => this.redo;

        public int UndoCount => this.undo.Count;
        public int RedoCount => this.redo.Count;

        public FileExplorerViewModel FileExplorer { get; }

        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }

        public bool IgnoreNavigation { get; set; }

        public HistoryViewModel(FileExplorerViewModel fileExplorer) {
            this.FileExplorer = fileExplorer;
            this.undo = new Stack<string>();
            this.redo = new Stack<string>();
            this.GoBackCommand = new RelayCommand(this.GoBackAction);
            this.GoForwardCommand = new RelayCommand(this.GoForwardAction);
        }

        public bool TryGetNextUndo(out string value) {
            if (this.undo.Count == 0) {
                value = default;
                return false;
            }

            value = this.undo.Pop();
            this.redo.Push(this.FileExplorer.CurrentDirectory);
            return true;
        }

        public bool TryGetNextRedo(out string value) {
            if (this.redo.Count == 0) {
                value = default;
                return false;
            }

            value = this.redo.Pop();
            this.undo.Push(this.FileExplorer.CurrentDirectory);
            return true;
        }

        public void PushHistory(in string value) {
            this.redo.Clear();
            this.undo.Push(value);
        }

        public void Clear() {
            this.redo.Clear();
            this.undo.Clear();
        }

        public void OnNavigate(string lastDestination) {
            if (this.IgnoreNavigation) {
                return;
            }

            this.PushHistory(lastDestination);
        }

        private void GoBackAction() {
            if (this.TryGetNextUndo(out string file)) {
                DoNavigation(file);
            }
        }

        private void GoForwardAction() {
            if (this.TryGetNextRedo(out string file)) {
                DoNavigation(file);
            }
        }

        public void DoNavigation(string path) {
            this.IgnoreNavigation = true;
            try {
                this.FileExplorer.Navigate(path);
            }
            finally {
                this.IgnoreNavigation = false;
            }
        }

        public override string ToString() {
            return $"Undo: {this.UndoCount} | Redo: {this.RedoCount}";
        }
    }
}