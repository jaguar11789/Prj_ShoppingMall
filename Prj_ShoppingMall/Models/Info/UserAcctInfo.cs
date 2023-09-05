using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class UserAcctInfo
    {
        public int    intUserNo          { get; set; }
        public string strUserId          { get; set; }
        public int    intRemainRealCash  { get; set; }
        public int    intRemainRealBonus { get; set; }
        public int    intRemainCash      { get; set; }
                                         
        public int    intUseCashAmt      { get; set; }
        public int    intUseBonusAmt     { get; set; }
        public int    intCnlCash         { get; set; }
        public int    intCollectCash     { get; set; }
        public int    intTotalCashAmt    { get; set; }
                                         
        public int    intTotalBonusAmt   { get; set; }
        public int    intSumCash         { get; set; }
        public string dateRegDate        { get; set; }
        public string strUpdDate         { get; set; }
        public int    intRetVal          { get; set; }
                                         
        public string strRetMsg          { get; set; }
    }
}