namespace FinanceAppGUI
{
    public interface ITransactionLogic
    {
        void NewTransaction();
        void EditTransaction(Transaction t);
        void RemoveIncome(Transaction t);
        void RemoveExpense(Transaction t);
        void SetupCollections(IList<Transaction> expense, IList<Transaction> income);
        double Balance();
    }
}