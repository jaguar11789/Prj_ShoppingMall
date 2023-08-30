using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class CouponInfo
    {
        public int      intCpNo    { get; set; }
        public string   strCpName  { get; set; }
        public string   strCpType  { get; set; }
        public string   strCpUnit  { get; set; }
        public long     lngCpCode  { get; set; }

        public string   strRegDate { get; set; }
        public string   strUseDate { get; set; }
        public string   strUserId  { get; set; }
        public string   strAdminId { get; set; }
        public string   strCpState { get; set; }

        public int      intCpUnit  { get; set; }
        public int      intCpCount { get; set; }
        public int      intRetVal  { get; set; }
        public string   strRetMsg  { get; set; }
    }
}