using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers.UserController
{
    public class UserMyPageController : Controller
    {
        UserService _userService = new UserService();
        UserCouponService _userCouponService = new UserCouponService();
        // GET: UserMyPage
        public ActionResult Index()
        {
            return View();
        }

        // 사용자정보(수정)
        [HttpPost]
        public JsonResult UserModify(UserInfo objUserInfo)
        {
            if (Session["strUserId"] != null) {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objUserInfo = _userService.UserModify(objUserInfo, objUserInfo.strUserId);

                return Json(objUserInfo);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // 쿠폰등록
        [HttpPost]
        public JsonResult CouponRegist(long lngCpCode)
        {
            CheckInfo objCheckInfo = null;

            if (Session["strUserId"] != null)
            {
                UserInfo objUserInfo = new UserInfo();
                objUserInfo.strUserId = Session["strUserId"].ToString();

                objCheckInfo = _userCouponService.couponRegist(lngCpCode, objUserInfo.strUserId);

                return Json(objCheckInfo);
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}