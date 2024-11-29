using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceAppGUI
{
    public class TransactionEditorWindowViewModel : ObservableObject
    {
        private Transaction transactionToEdit;
        public List<string> Category { get { return new List<string>() { "Expense", "Income" }; } }

        public Transaction TransactionToEdit
        {
            get
            { return transactionToEdit; }
            set { SetProperty(ref transactionToEdit, value); }
        }  
    }
}
