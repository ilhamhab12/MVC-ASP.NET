using BootcampASP.Context;
using BootcampASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootcampASP.Controllers
{
    public class TransactionsController : Controller
    {
        MyContext _context = new MyContext();
        Item item = new Item();
        Supplier supplier = new Supplier();
        Transaction transaction = new Transaction();
        TransactionItem transactionItem = new TransactionItem();
        // GET: Transactions
        public ActionResult Index()
        {
            //var list = _context.Transactions.Where(x => x.IsDelete == false).ToList();
            return View();
        }

        public JsonResult getItemSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            //pake yg ini juga bisa
            //var get = _context.Suppliers.Where(x => x.IsDelete == false);
            //suppliers = get.OrderBy(a => a.Name).ToList();
            
            //untuk menampilkan supplier menggunakan where dan orderby di gabung
            suppliers = _context.Suppliers.Where(a => a.IsDelete == false).OrderBy(a => a.Name).ToList();

            return new JsonResult { Data = suppliers, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getItems(int Id)
        {
            List<Item> items = new List<Item>();

            //sama sama bisa pake == dan equals(equals seharusnya object)
            //items = _context.Items.Where(a => a.Suppliers.Id.Equals(Id) && a.IsDelete == false).OrderBy(a => a.Name).ToList();
            items = _context.Items.Where(a => a.Suppliers.Id == Id && a.IsDelete == false).OrderBy(a => a.Name).ToList();
            

            return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult save(Transaction transaction, List<TransactionItem> TransactionItem)
        {
            
            bool status = false;
            DateTime  dateOrg;
            var isValidDate = DateTime.TryParseExact(transaction.TransactionDate.ToString(), "mm-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out dateOrg);
            if (isValidDate)
            {
                transaction.TransactionDate = dateOrg;
            }

            var isValidModel = TryUpdateModel(transaction);
            if (isValidModel)
            {
                transaction.CreateDate = DateTimeOffset.Now.LocalDateTime;
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                //foreach menampilkan isi list itemtransaction dan cari idnya
                foreach (var item in TransactionItem) {
                    
                    //mencari transaction id terahir
                    var getTransaction = _context.Transactions.Max(x => x.Id);
                    var getTransactionDetail = _context.Transactions.Find(getTransaction);

                    //cari id di item in transactionItem
                    var getitem = _context.Items.Find(item.Id);

                    //memasukan data ke transactionItem
                    transactionItem.CreateDate = DateTimeOffset.Now.LocalDateTime;
                    transactionItem.Transactions = getTransactionDetail;
                    transactionItem.Items = getitem;
                    transactionItem.Quantity = item.Quantity;
                    _context.TransactionItems.Add(transactionItem);
                    _context.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

    }
}