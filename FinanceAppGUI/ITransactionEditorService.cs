using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceAppGUI
{
    public interface ITransactionEditorService
    {
        bool EditTransaction(Transaction t);
    }
}
