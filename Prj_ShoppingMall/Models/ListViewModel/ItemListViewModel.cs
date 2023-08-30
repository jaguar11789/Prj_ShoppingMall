using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.ListViewModel
{
    public class ItemListViewModel
    {
        public IEnumerable<ItemInfo> Item { get; set; }
        public int    intRetVal { get; set; }
        public string strRetMsg { get; set; }
    }
}