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
using System.Windows.Shapes;

namespace FinanceAppGUI
{
    /// <summary>
    /// Interaction logic for TransactionEditorWindow.xaml
    /// </summary>
    public partial class TransactionEditorWindow : Window
    {
        public TransactionEditorWindow(Transaction T)
        {
            InitializeComponent();
            var viewModel = new TransactionEditorWindowViewModel();
            viewModel.TransactionToEdit = T;
            this.DataContext = viewModel;
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (DataContext is TransactionEditorWindowViewModel vm && vm.IsValid)
            {
                foreach (var item in panel.Children)
                {
                    if (item is TextBox t)
                    {
                        t.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    }
                    if (item is ComboBox cb)
                    {
                        cb.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

                    }
                }
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("The transaction must have a valid category, name and amount!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = false;
        }


    }
}
