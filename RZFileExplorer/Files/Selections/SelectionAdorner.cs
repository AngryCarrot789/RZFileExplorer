using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace RZFileExplorer.Files.Selections {
    // Credits to: https://www.codeproject.com/Articles/209560/ListBox-drag-selection
    public sealed class SelectionAdorner : Adorner {
        // Initializes a new instance of the SelectionAdorner class.
        public SelectionAdorner(UIElement parent)
            : base(parent) {
            // Make sure the mouse doesn't see us.
            this.IsHitTestVisible = false;

            // We only draw a rectangle when we're enabled.
            this.IsEnabledChanged += delegate { this.InvalidateVisual(); };
        }

        // Gets or sets the area of the selection rectangle.
        public Rect SelectionArea { get; set; }

        // Participates in rendering operations that are directed by the layout system.
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            if (this.IsEnabled) {
                // Make the lines snap to pixels (add half the pen width [0.5])
                double[] x = { this.SelectionArea.Left + 0.5, this.SelectionArea.Right + 0.5 };
                double[] y = { this.SelectionArea.Top + 0.5, this.SelectionArea.Bottom + 0.5 };
                drawingContext.PushGuidelineSet(new GuidelineSet(x, y));

                Brush fill = SystemColors.HighlightBrush.Clone();
                fill.Opacity = 0.4;
                drawingContext.DrawRectangle(
                    fill,
                    new Pen(SystemColors.HighlightBrush, 1.0),
                    this.SelectionArea);
            }
        }
    }
}
