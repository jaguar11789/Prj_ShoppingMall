using Prj_ShoppingMall.Models.AdminService;
using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers.UserController
{
    public class UserPurchaseController : Controller
    {
        UserItemService _userItemService = new UserItemService();
        AdminCouponService _adminCouponService = new AdminCouponService();
        // GET: UserPurchase
        public ActionResult Index()
        {
            return View();
        }

        // 배송지 수정
        [HttpPost]
        public JsonResult AddrUpdate(UserInfo objUserInfo)
        {
            CheckInfo objCheckInfo = null;

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objCheckInfo = _userItemService.userAddrUpdate(objUserInfo, objUserInfo.strUserId);

                return Json(objCheckInfo);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // 쿠폰 유효성 검사
        [HttpPost]
        public JsonResult CheckCoupon(CouponInfo objCouponInfo)
        {
            CheckInfo objCheckInfo = null;

            if (Session["strUserId"] != null)
            {
                objCheckInfo = _adminCouponService.couponValidation(objCouponInfo);

                return Json(objCheckInfo);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // 상품구매
        [HttpPost]
        public JsonResult PurchaseItem(UserInfo objUserInfo, ItemInfo objItemInfo, string strItemColor, string strItemSize, long? lngCpCode, string strReceiveUserId)
        {
            CheckInfo objResult = null;
            long cpCodeValue    = lngCpCode ?? 0;

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objResult = _userItemService.purchaseItem(objUserInfo, objItemInfo, strItemColor, strItemSize, cpCodeValue, strReceiveUserId);

                return Json(objResult);
            }
            else
            {
                return Json(new { success = false });
            }
        }
        
        // 사용자 아이디 확인(선물)
        [HttpPost]
        public JsonResult CheckReceiveUserId(string strReceiveUserId)
        {
            CheckInfo objResult = null;
            objResult = _userItemService.checkReceiveUserId(strReceiveUserId);

            return Json(objResult);
        }

        // 상품 구매(선물)
        [HttpPost]
        public JsonResult PurcharGift(UserInfo objUserInfo, ItemInfo objItemInfo, string strItemColor, string strItemSize, long? lngCpCode, string strReceiveUserId)
        {
            CheckInfo objResult = null;
            long cpCodeValue = lngCpCode ?? 0;

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objResult = _userItemService.purchaseGift(objUserInfo, objItemInfo, strItemColor, strItemSize, cpCodeValue, strReceiveUserId);

                return Json(objResult);
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}