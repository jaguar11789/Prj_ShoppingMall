using Newtonsoft.Json;
using Prj_ShoppingMall.Models.CashService;
using Prj_ShoppingMall.Models.Common;
using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers
{
    public class CashNotificationController : Controller
    {
        CashService _cashService = new CashService();
        // GET: CashNotification
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CashNotification()
        {
            Log.Info("[CashNotificationController][CashNotification]...");
            try
            {
                StreamReader objStreamReader = new StreamReader(Request.GetBufferlessInputStream(), Encoding.GetEncoding("utf-8"));
                string strResponse = objStreamReader.ReadToEnd();

                objStreamReader.Close();

                if (string.IsNullOrEmpty(strResponse))
                {
                    Response.Write("{\"code\":9999, \"message\":\"Response data is empty\"}");
                }
                PayResult objJsonData = JsonConvert.DeserializeObject<PayResult>(strResponse);

                int intResult = 0;
                //if (strCode.Equals("0"))
                intResult = _cashService.ChargeCash(objJsonData);

                if (intResult == 0)
                    _cashService.updatePGLog(objJsonData);

                Response.Write("{\"code\":0, \"message\":\"success\"}");
            }
            catch (Exception ex)
            {
                Log.Info("[CashNotificationController][CashNotification] WebException : " + ex);
            }

            return View();
        }
    }
}