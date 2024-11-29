using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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

        public ICommand ExportCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        public double Balance
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

            ExportCommand = new RelayCommand(() =>
            {

                try
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = DateTime.Now.ToString("yyyyddMM_HHmmss"); ; // Default file name
                    dlg.DefaultExt = ".csv"; // Default file extension
                    dlg.Filter = "CSV Files (*.csv)|*.csv"; // Filter files by extension



                    // Process save file dialog box results
                    if (dlg.ShowDialog() == true)
                    {
                        using (var writer = new StreamWriter(dlg.FileName))
                        {
                            writer.WriteLine("Category,Name,Amount"); // Fejléc
                            foreach (var transaction in Income)
                            {
                                writer.WriteLine($"Income,{transaction.Name},{transaction.Amount}");
                            }
                            foreach (var transaction in Expense)
                            {
                                writer.WriteLine($"Expense,{transaction.Name},{transaction.Amount}");
                            }
                        }
                        MessageBox.Show("Export succesful!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred during export: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });

            ImportCommand = new RelayCommand(() =>
            {

                try
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                    // Set filter for file extension and default file extension 
                    dlg.DefaultExt = ".csv";
                    dlg.Filter = "CSV Files (*.csv)|*.csv";


                    // Process open file dialog box results
                    if (dlg.ShowDialog() == true)
                    {

                        // Kérdés az adatok felülírásáról
                        var result = MessageBox.Show(
                            "Overwrite?",
                            "Overwrite",
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Gyűjtemények ürítése
                            Income.Clear();
                            Expense.Clear();
                        }

                        else if (result == MessageBoxResult.Cancel)
                        {
                            // Felhasználó megszakította az importálást
                            return;
                        }

                        using (var reader = new StreamReader(dlg.FileName))
                        {
                            string line;
                            // Fejléc átugrása
                            reader.ReadLine();

                            while ((line = reader.ReadLine()) != null)
                            {
                                var columns = line.Split(',');

                                if (columns.Length != 3 || !double.TryParse(columns[2], out _))
                                {
                                    MessageBox.Show($"Unknown format", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    continue;
                                }

                                if (columns.Length == 3)
                                {
                                    string category = columns[0].Trim();
                                    string name = columns[1].Trim();
                                    double amount = double.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);

                                    if (category != "Income" && category != "Expense")
                                    {
                                        MessageBox.Show($"Unknown category: {category}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        continue;
                                    }

                                    var transaction = new Transaction
                                    {
                                        Category = category,
                                        Name = name,
                                        Amount = amount
                                    };

                                    if (category == "Income")
                                    {
                                        Income.Add(transaction);
                                    }
                                    else if (category == "Expense")
                                    {
                                        Expense.Add(transaction);
                                    }

                                }
                            }
                        }

                        MessageBox.Show("Import succesful!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred during import: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });

        }
    }
}
