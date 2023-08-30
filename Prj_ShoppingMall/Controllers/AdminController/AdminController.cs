using Prj_ShoppingMall.Models.AdminService;
using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers.AdminController
{
    public class AdminController : Controller
    {
        AdminService _adminService             = new AdminService();
        AdminItemService _adminItemService     = new AdminItemService();
        AdminCouponService _adminCouponService = new AdminCouponService();
        // GET: Admin
        public ActionResult AdminIndex()
        {
            return View();
        }

        // 관리자 로그인 페이지
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        // 관리자 로그인
        [HttpPost]
        public ActionResult Login(AdminInfo objAdminInfo)
        {
            if (objAdminInfo.strAdminId != null && objAdminInfo.strAdminPwd != null)
            {
                var result = _adminService.adminLogin(objAdminInfo);
                TempData["Message"] = result.Item2;
                if (result.Item1 == 0)
                {
                    Session["strAdminId"] = objAdminInfo.strAdminId.ToString();
                    ViewBag.IsLoggedIn = true;

                    return RedirectToAction("AdminIndex", "Admin");
                }
            }
            ModelState.AddModelError(string.Empty, "관리자 ID 혹은 비밀번호가 올바르지 않습니다.");

            return RedirectToAction("AdminLogin", "Admin");
        }

        // 관리자 로그아웃
        [HttpPost]
        public ActionResult AdminLogout()
        {
            Session.Remove("strAdminId");
            return RedirectToAction("Index", "Home");
        }

        // 쿠폰 발급 페이지
        [HttpGet]
        public ActionResult Coupon()
        {
            return View();
        }

        // 쿠폰 리스트 페이지
        [HttpGet]
        public ActionResult CouponCheck()
        {
            return View();
        }

        // 상품관리 페이지
        [HttpGet]
        public ActionResult ItemManagement()
        {
            return View();
        }

        // 상품등록 페이지
        [HttpGet]
        public ActionResult ItemRegist()
        {
            return View();
        }

        // 상품등록
        [HttpPost]
        public ActionResult ItemRegist(ItemInfo objItemInfo)
        {
            CheckInfo objCheckInfo = null;
            AdminInfo objAdminInfo = new AdminInfo();

            if (Session["strAdminId"] != null)
            {
                objAdminInfo.strAdminId = Session["strAdminId"].ToString();
                objCheckInfo = _adminItemService.itemRegist(objItemInfo, objAdminInfo.strAdminId);

                return RedirectToAction("ItemRgstSuccess", "Admin");
            }
            else
            {
                return View();
            }
        }

        // 상품등록 성공 페이지
        [HttpGet]
        public ActionResult ItemRgstSuccess()
        {
            return View();
        }

        // 상품등록 실패 페이지
        [HttpGet]
        public ActionResult ItemRgstFail()
        {
            return View();
        }
    }
}