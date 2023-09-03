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

        // 쿠폰 유효성 검사 페이지
        [HttpGet]
        public ActionResult CouponValidation()
        {
            return View();
        }

        // 쿠폰 상세정보 페이지
        public ActionResult CouponDtl(int CpNo)
        {
            CouponInfo objResult = new CouponInfo();
            objResult = _adminCouponService.getCouponDetailInfo(CpNo);

            ViewBag.CouponInfo = objResult;

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

        // 상품상세 페이지
        [HttpGet]
        public ActionResult ItemDtl(int itemNo)
        {
            ItemInfo objItemInfo = null;

            if(Session["strAdminId"] != null)
            {
                // 상품 기본정보
                objItemInfo = _adminItemService.getItemDetail(itemNo);

                // 상품 색상정보
                List<int> lstItemColorInfo = _adminItemService.getItemColorInfo(itemNo);
                objItemInfo.intItemColorNo = lstItemColorInfo;

                // 상품 사이즈정보
                List<int> lstItemSizeInfo = _adminItemService.getItemSizeInfo(itemNo);
                objItemInfo.intItemSizeNo = lstItemSizeInfo;

                ViewBag.ItemInfo = objItemInfo;

                return View();
            }
            else
            {
                return View();
            }
        }

        // 상품수정 성공
        [HttpGet]
        public ActionResult ItemModifySuccess()
        {
            return View();
        }

        // 상품수정 실패
        [HttpGet]
        public ActionResult ItemModifyFail()
        {
            return View();
        }
    }
}