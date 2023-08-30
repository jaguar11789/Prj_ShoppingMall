using Prj_ShoppingMall.Models.CashService;
using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers
{
    public class CashController : Controller
    {
        CashService _cashService = new CashService();
        // GET: Cash
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChargeCash(string payMethod, int itemPrice, string itemName, UserInfo userInfo)
        {
            int intOrderNo = 0;
            string strResult = null;
            try
            {
                userInfo.strUserId = Session["strUserId"].ToString();
                // 1. PG 로그 적재
                //intOrderNo = _cashService.InsertPGLog(userInfo.UserId, payMethod, itemPrice, itemName);

                // 2. 결제요청
                strResult = _cashService.requestPayment(payMethod, itemPrice, itemName, userInfo.strUserId, intOrderNo);
            }
            catch (Exception)
            {
                return Json(strResult);
            }
            if (strResult != null)
            {
                return Json(strResult);
            }
            return Json(new { success = true });
        }
    }
}