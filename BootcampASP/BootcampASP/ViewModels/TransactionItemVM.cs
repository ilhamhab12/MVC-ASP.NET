using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampASP.ViewModels
{
    public class TransactionItemVM
    {
        public int Id { get; set; }
        public int Transactions_Id { get; set; }
        public int Items_Id { get; set; }
        public int Quantity { get; set; }        
        
    }
}