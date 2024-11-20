using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinanceAppGUI
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public ObservableCollection<Transaction> Expense { get; set; }
        public ObservableCollection<Transaction> Income { get; set; }


        private Transaction selected;

        public Transaction Selected
        {
            get { return selected; }
            set
            {
                SetProperty(ref selected, value);
                (EditCommand as RelayCommand).NotifyCanExecuteChanged();
                (RemoveCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand NewCommand { get; set; }

        public MainWindowViewModel()
        {
            Income = new ObservableCollection<Transaction>();
            Expense = new ObservableCollection<Transaction>();

            Income.Add(new Transaction() { Name = "Salary", Amount = 10000 ,Category="Income"  });
            Expense.Add(new Transaction() { Name = "Shopping", Amount = 5000, Category="Expense" });

            NewCommand = new RelayCommand(
              () =>
              {
                  Transaction newT = new Transaction();
                  var editor = new TransactionEditorWindow(newT);
                  if (editor.ShowDialog() == true)
                  {
                      if (newT.Category == "Income")
                      {
                          Income.Add(newT);
                      }
                      
                      else
                      {
                          Expense.Add(newT);
                      }
                  }
              });
            RemoveCommand = new RelayCommand(
                () => {
                    if (Selected.Category.Equals("Expense"))
                    {
                        Expense.Remove(Selected);
                    }
                    else
                    {
                        Income.Remove(Selected);
                    }
                    
                },
                () => Selected != null
                );
            EditCommand = new RelayCommand(
                () => {
                    var editor = new TransactionEditorWindow(Selected);
                    if (editor.ShowDialog() == true)
                        MessageBox.Show("Save successful.");
                },
                () => Selected != null
                );
        }
    }
}
