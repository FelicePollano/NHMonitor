using System;
using System.Collections.Generic;
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

namespace NHMonitor.Views
{
    /// <summary>
    /// Logica di interazione per QueryEventView.xaml
    /// </summary>
    public partial class QueryEventView : UserControl
    {
        public QueryEventView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NHMonitor.Resources.sql.xml"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    Editor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
        }

    }
}
