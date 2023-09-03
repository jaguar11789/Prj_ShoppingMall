using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class CashReceipt
    {
        public string cid { get; set; }
        public string code { get; set; }
        public string deal_no { get; set; }
        public string issue_type { get; set; }
        public string message { get; set; }

        public string payer_sid { get; set; }
        public string type { get; set; }
    }
}