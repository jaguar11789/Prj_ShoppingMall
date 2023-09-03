using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class PayResult
    {
        public string code { get; set; }
        public string message { get; set; }

        public string user_id { get; set; }
        public string user_name { get; set; }
        public string order_no { get; set; }
        public string service_name { get; set; }
        public string product_name { get; set; }

        public string custom_parameter { get; set; }
        public string tid { get; set; }
        public string cid { get; set; }
        public double amount { get; set; }
        public string pay_info { get; set; }

        public string pgcode { get; set; }
        public string billkey { get; set; }
        public string domestic_flag { get; set; }
        public string transaction_date { get; set; }
        public string card_info { get; set; }

        public string payhash { get; set; }
        public int install_month { get; set; }
        //public string nonsettleamt { get; set; }
        public CashReceipt cash_receipt { get; set; }
    }
}