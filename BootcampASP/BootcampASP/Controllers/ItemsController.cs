using BootcampASP.Context;
using BootcampASP.Models;
using BootcampASP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootcampASP.Controllers
{
    public class ItemsController : Controller
    {
        MyContext _context = new MyContext();
        Item item = new Item();
        ItemVM itemVM = new ItemVM();
        // GET: Items
        public ActionResult Index()
        {
            var show = _context.Items.Where(x => x.IsDelete == false).ToList();
            return View(show);
        }
        [HttpGet]
        public ActionResult Create()
        {
            //Make Selectlist dropdown from table supplier
            //get supplier is delete == false
            var get = _context.Suppliers.Where(x => x.IsDelete == false);
            var getSupplier = get.Select(item => new SelectListItem()
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = false
            }).ToList();

            ViewBag.Supplier = getSupplier;


            return View();
        }
        [HttpPost]
        public ActionResult Create(ItemVM itemVM)
        {
            if (ModelState.IsValid)
            {
                Item item = new Item();
                item.Name = itemVM.Name;
                item.Stok = itemVM.Stok;
                item.Price = itemVM.Price;
                item.Suppliers = _context.Suppliers.Find(itemVM.Suppliers_Id);
                item.CreateDate = DateTimeOffset.Now.LocalDateTime;
                item.IsDelete = false;
                _context.Items.Add(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
           
            if(Id == null)
            {
                throw new NullReferenceException();
            }
            var getId = _context.Items.Find(Id);
            if (getId == null)
            {
                throw new NullReferenceException();
            }

            //Get Supplier dropdown list
            var getCombo = _context.Suppliers.Where(x => x.IsDelete == false);
            var GetSupplier = getCombo.Select(item => new SelectListItem()
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }).ToList();
            ViewBag.Supplier = GetSupplier;

            //Set Selected true
            foreach (var get in GetSupplier)
            {
                if (get.Value.Equals(getId.Suppliers.Id.ToString()))
                {
                    get.Selected = true;
                    break;
                }
            }

            //parsing ItemVm
            itemVM.Name = getId.Name;
            itemVM.Stok = getId.Stok;
            itemVM.Price = getId.Price;
            itemVM.Suppliers_Id = getId.Suppliers.Id;
            return View(itemVM);
        }

        [HttpPost]
        public ActionResult Edit(ItemVM itemVM)
        {
            if(ModelState.IsValid)
            {
                var getId = _context.Items.Find(itemVM.Id);
                getId.Name = itemVM.Name;
                getId.Stok = itemVM.Stok;
                getId.Price = itemVM.Price;
                getId.Suppliers = _context.Suppliers.Find(itemVM.Suppliers_Id);
                getId.UpdateDate = DateTimeOffset.Now.LocalDateTime;
                _context.Entry(getId).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //[HttpGet]
        //public ActionResult Delete(int? Id)
        //{
        //    if (Id == null)
        //    {
        //        throw new NullReferenceException();
        //    }
        //    var get = _context.Items.SingleOrDefault(x => x.Id == Id);
        //    if (get == null)
        //    {
        //        throw new NullReferenceException();
        //    }
        //    return View(get);
       // }

        [HttpPost]
        public ActionResult Delete(int? Id)
        {
            if(ModelState.IsValid)
            {
                var getId = _context.Items.Find(Id);
                getId.IsDelete = true;
                getId.DeleteDate = DateTimeOffset.Now.LocalDateTime;
                _context.Entry(getId).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}