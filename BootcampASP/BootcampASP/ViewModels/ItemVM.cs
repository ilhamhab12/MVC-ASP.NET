﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootcampASP.ViewModels
{
    public class ItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stok { get; set; }
        public int Price { get; set; }
        public int Suppliers_Id { get; set; }


    }
}