using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NHMonitor.Behaviors
{
    public class BindTextToAvalon:Behavior<TextEditor>
    {


        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindableText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText", typeof(string), typeof(BindTextToAvalon), new PropertyMetadata(null));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(e.Property == BindableTextProperty)
            {
                AssociatedObject.Document.Text = BindableText;
            }
            base.OnPropertyChanged(e);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
        }
    }
}
