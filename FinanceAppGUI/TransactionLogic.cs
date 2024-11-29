using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FinanceAppGUI
{
    public class TransactionLogic : ITransactionLogic
    {
        IList<Transaction> expense;
        IList<Transaction> income;
        IMessenger messenger;
        ITransactionEditorService editorService;


        public TransactionLogic(IMessenger messenger, ITransactionEditorService editorService)
        {
            this.messenger = messenger;
            this.editorService = editorService;
        }

        public void SetupCollections(IList<Transaction> income, IList<Transaction> expense)
        {
            this.income = income;
            this.expense = expense;
        }

        public void NewTransaction()
        {
            Transaction t = new Transaction();
            bool save = editorService.EditTransaction(t);

            if (save && t.Category == "Expense")
            {
                expense.Add(t);
            }
            else
            {
                income.Add(t);
            }
            messenger.Send("Transaction added", "TransactionInfo");
        }

        public void EditTransaction(Transaction t)
        {
            string originalCategory = t.Category;
            bool save = editorService.EditTransaction(t);

            if (originalCategory != t.Category)
            {
                if (originalCategory == "Income")
                {
                    income.Remove(t);
                    expense.Add(t);
                }
                else if (originalCategory == "Expense")
                {
                    expense.Remove(t);
                    income.Add(t);
                }
                MessageBox.Show("Save successful");
            }
        }

        public void RemoveIncome(Transaction t)
        {
            income.Remove(t);
            messenger.Send("Income removed", "TransactionInfo");
        }

        public void RemoveExpense(Transaction t)
        {
            expense.Remove(t);
            messenger.Send("Expense removed", "TransactionInfo");
        }

        public int Balance()
        {
            int inc_temp = income.Count != 0 ? income.Sum(a => a.Amount) : 0;
            int exp_temp = expense.Count != 0 ? expense.Sum(a => a.Amount) : 0;

            return inc_temp - exp_temp;
        }
    }
}
