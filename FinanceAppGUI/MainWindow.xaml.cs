using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceAppGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("money.jpg", UriKind.Relative)),
                Opacity = 0.2,
                Stretch = Stretch.UniformToFill
            };

            grid.Background = brush;
        }
        
    }
}