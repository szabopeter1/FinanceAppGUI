using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceAppGUI
{
    public class Transaction: ObservableObject
    {
        private string name;
        private string category;
        private double amount;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Category
        {
            get { return category; }
            set { SetProperty(ref category, value); }
        }

        public double Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value); }
        }

        public Transaction GetCopy()
        {
            return new Transaction()
            {
                name = Name,
                amount = Amount
            };
        }
    }
}
