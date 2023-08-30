using Prj_ShoppingMall.Models.AdminService;
using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.ListViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers.AdminController
{
    public class AdminCouponController : Controller
    {
        AdminCouponService _adminCouponService = new AdminCouponService();
        // GET: AdminCoupon
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CouponIssued(CouponInfo objCouponInfo)
        {
            CheckInfo objCheckInfo = null;
            AdminInfo objAdminInfo = new AdminInfo();

            if(Session["strAdminId"] != null)
            {
                objAdminInfo.strAdminId = Session["strAdminId"].ToString();
                objCheckInfo = _adminCouponService.couponIssued(objCouponInfo, objAdminInfo.strAdminId);

                return Json(objCheckInfo);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // 쿠폰 조회
        [HttpPost]
        public JsonResult CouponCheck(int intCpUnit, int intCpState)
        {
            CouponListViewModel objResult = null;

            if(Session["strAdminId"] != null)
            {
                objResult = _adminCouponService.getCouponList(intCpUnit, intCpState);

                return Json(objResult);
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}