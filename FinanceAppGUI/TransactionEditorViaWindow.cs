using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceAppGUI
{
    public class TransactionEditorViaWindow : ITransactionEditorService
    {
        
        public bool EditTransaction(Transaction t)
        {
            var editor = new TransactionEditorWindow(t);
            return editor.ShowDialog() == true;
        }


    }
}
