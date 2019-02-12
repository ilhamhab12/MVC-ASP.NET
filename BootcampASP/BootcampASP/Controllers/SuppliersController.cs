using BootcampASP.Context;
using BootcampASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootcampASP.Controllers
{
    public class SuppliersController : Controller
    {
        MyContext _context = new MyContext();
        Supplier supplier = new Supplier();
        // GET: Suppliers
        public ActionResult Index()
        {
            var obj = _context.Suppliers.Where(x => x.IsDelete == false).ToList();
            return View(obj);
        }

        //INSERT
        [HttpGet]
        public ActionResult Create()
        {                      
            return View();

        }
        //Insert ngak pake bind ngakpapa
        [HttpPost]
        public ActionResult Create([Bind(Include ="Name, JoinDate")] Supplier supplier)
        {
            supplier.JoinDate = DateTimeOffset.Now.LocalDateTime;
            supplier.CreateDate = DateTimeOffset.Now.LocalDateTime;
            supplier.IsDelete = false;
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //UPDATE
        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                throw new NullReferenceException();
            }
                var get = _context.Suppliers.SingleOrDefault(x => x.Id == Id);
                if (get == null)
                {
                    throw new NullReferenceException();
                }
            return View(get);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var get = _context.Suppliers.SingleOrDefault(x => x.Id == supplier.Id);
                get.Name = supplier.Name;
                get.UpdateDate = DateTimeOffset.Now.LocalDateTime;
                _context.Entry(get).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //DELETE
        [HttpGet]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                throw new NullReferenceException();
            }
            var get = _context.Suppliers.SingleOrDefault(x => x.Id == Id);
            if(get == null)
            {
                throw new NullReferenceException();
            }
            return View(get);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Supplier supplier)
        {
            var get = _context.Suppliers.SingleOrDefault(x => x.Id == supplier.Id);
            get.IsDelete = true;
            get.DeleteDate = DateTimeOffset.Now.LocalDateTime;
            _context.Entry(get).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}