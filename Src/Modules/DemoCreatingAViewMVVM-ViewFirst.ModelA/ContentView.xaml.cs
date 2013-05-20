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
using PrismDemo.Infrastructure;

namespace DemoCreatingAViewMVVM_ViewFirst.ModelA
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl, IView2
    {
        public ContentView(IContentAViewViewModel variable)
        {
            InitializeComponent();

            ViewModel = variable;
        }

        public IViewModel2 ViewModel
        {
            get
            {
                return (IViewModel2)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
