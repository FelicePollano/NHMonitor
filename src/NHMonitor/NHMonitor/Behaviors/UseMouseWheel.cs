using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NHMonitor.Behaviors
{
    public class UseMouseWheel : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.PreviewMouseWheel += (s, e) =>
            {
                AssociatedObject.ScrollToVerticalOffset(AssociatedObject.VerticalOffset-e.Delta);
                e.Handled = true;
            };
            base.OnAttached();
        }
    }
}
