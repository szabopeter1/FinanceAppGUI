using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FinanceAppGUI
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public ITransactionLogic logic;
        public ObservableCollection<Transaction> Expense { get; set; }
        public ObservableCollection<Transaction> Income { get; set; }

        public ICollectionView IncomeView => CollectionViewSource.GetDefaultView(Income);
        public ICollectionView ExpenseView => CollectionViewSource.GetDefaultView(Expense);


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

        public int Balance
        {
            get { return logic.Balance(); }
        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop,
                    typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public MainWindowViewModel()
            : this(IsInDesignMode ? null : Ioc.Default.GetService<ITransactionLogic>())
        {

        }
        
        public MainWindowViewModel(ITransactionLogic logic)
        {
            Income = new ObservableCollection<Transaction>();
            Expense = new ObservableCollection<Transaction>();
            this.logic = logic;
            logic.SetupCollections(Income, Expense);
            Income.Add(new Transaction() { Name = "Salary", Amount = 10000 ,Category="Income"  });
            Expense.Add(new Transaction() { Name = "Shopping", Amount = 5000, Category="Expense" });

            NewCommand = new RelayCommand(
              () =>
              {
                  logic.NewTransaction();
              });
            RemoveCommand = new RelayCommand(
                () => {
                    if (Selected.Category.Equals("Expense"))
                    {
                        logic.RemoveExpense(selected);
                    }
                    else
                    {
                        logic.RemoveIncome(selected);
                    }                   
                },
                () => Selected != null
                );
            EditCommand = new RelayCommand(
                () => {
                    logic.EditTransaction(selected);                  
                },
                () => Selected != null
                );

            Messenger.Register<MainWindowViewModel, string, string>(this, "TransactionInfo",
                (recipient, msg) =>
                {
                    OnPropertyChanged("Balance");
                });
            
        }
    }
}
