using Prj_ShoppingMall.Models.AdminService;
using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.ListViewModel;
using Prj_ShoppingMall.Models.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers.UserController
{
    public class UserController : Controller
    {
        UserService _userService = new UserService();
        UserCouponService _userCouponService = new UserCouponService();
        AdminItemService _adminItemService = new AdminItemService();
        AdminCouponService _adminCouponService = new AdminCouponService();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // 사용자 회원가입 페이지
        [HttpGet]
        public ActionResult UserRegister()
        {
            return View();
        }

        // 사용자 회원가입
        [HttpPost]
        public JsonResult Register(UserInfo objUserInfo)
        {
            objUserInfo = _userService.userRegister(objUserInfo);

            return Json(objUserInfo);
        }

        // 사용자 중복확인
        [HttpPost]
        public JsonResult DoubleCheck(string strUserId)
        {
            int intRetVal = 0;
            intRetVal = _userService.userIdDoubleCheck(strUserId);

            return Json(intRetVal);
        }

        // 회원가입 성공
        public ActionResult RgstSuccess()
        {
            return View();
        }

        // 사용자 로그인 페이지
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        // 사용자 로그인
        [HttpPost]
        public ActionResult Login(UserInfo objUserInfo)
        {
            if (objUserInfo.strUserId != null && objUserInfo.strUserPwd != null)
            {
                var result = _userService.userLogin(objUserInfo);
                TempData["Message"] = result.Item2;

                if (result.Item1 == 0)
                {
                    Session["strUserId"] = objUserInfo.strUserId.ToString();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");

                    return RedirectToAction("UserLogin", "User");
                }
            }
            return View();
        }

        // 사용자 로그아웃
        [HttpPost]
        public ActionResult UserLogOut()
        {
            Session.Remove("strUserID");

            return RedirectToAction("Index", "Home");
        }

        // ========================================================================
        // ==========================사용자 마이페이지==============================
        // ========================================================================
        // 사용자 마이페이지
        [HttpGet]
        public ActionResult UserMyPage()
        {
            if (Session["strUserId"] != null)
            {
                return View();
            }
            else
            {
                TempData["Error"] = "로그인이 필요합니다.";
                return RedirectToAction("UserLogin", "User");
            }
        }

        // 사용자정보(수정) 페이지
        [HttpGet]
        public ActionResult UserProfile()
        {
            UserInfo objUserInfo = new UserInfo();

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objUserInfo = _userService.getUserInfo(objUserInfo.strUserId);

                ViewBag.UserInfo = objUserInfo;

                return View();
            }
            else
            {
                TempData["Error"] = "로그인이 필요합니다.";
                return RedirectToAction("UserLogin", "User");
            }

        }

        // 쿠폰등록 & 조회
        [HttpGet]
        public ActionResult UserCouponHistory()
        {
            CouponListViewModel objResult = null;
            UserInfo objUserInfo = new UserInfo();

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();
                objResult = _userCouponService.getUserCouponList(objUserInfo.strUserId);

                ViewBag.CouponInfo = objResult;

                return View();
            }
            else
            {
                TempData["Error"] = "로그인이 필요합니다.";
                return RedirectToAction("UserLogin", "User");
            }
        }

        // 캐시충전 페이지
        [HttpGet]
        public ActionResult ChargeCash()
        {
            if (Session["strUserId"] != null)
            {
                return View();
            }
            else
            {
                TempData["Error"] = "로그인이 필요합니다.";
                return RedirectToAction("UserLogin", "User");
            }
        }

        // 충전성공
        [HttpGet]
        public ActionResult ChargeSuccess()
        {
            return View();
        }

        // 충전실패
        [HttpGet]
        public ActionResult ChargeFail()
        {
            return View();
        }

        // 상품상세 페이지
        [HttpGet]
        public ActionResult ItemDetail(int ItemNo)
        {
            ItemInfo objResult = _adminItemService.getItemDetail(ItemNo);

            // 상품 색상정보
            List<int> objResult2       = _adminItemService.getItemColorInfo(ItemNo);
            List<string> objColorList  = objResult2.Select(colorCode => ItemColorSize.ColorNoToString(colorCode)).ToList();
            objResult.objItemColorList = objColorList;
            objResult.intItemColorNo   = objResult2;

            // 상품 사이즈정보
            List<int> objResult3      = _adminItemService.getItemSizeInfo(ItemNo);
            List<string> objSizeList  = objResult3.Select(sizeCode => ItemColorSize.SizeNoToString(sizeCode)).ToList();
            objResult.objItemSizeList = objSizeList;
            objResult.intItemSizeNo   = objResult3;

            ViewBag.ItemInfo = objResult;

            if (objResult != null)
            {
                return View(objResult);
            }
            else
            {
                return RedirectToAction("Main", "Home");
            }
        }

        // 상품구매 페이지
        [HttpGet]
        public ActionResult ItemPurchase(int itemNo, string ItemColor, string ItemSize, UserInfo objUserInfo)
        {
            UserAcctInfo objUserAcctInfo = null;

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();

                // 색상 정보
                ViewBag.ItemColor = ItemColor;
                // 사이즈 정보
                ViewBag.ItemSize  = ItemSize;

                // 사용자 정보
                objUserInfo = _userService.getUserInfo(objUserInfo.strUserId);
                ViewBag.UserInfo = objUserInfo;

                // 사용자 계좌정보
                objUserAcctInfo = _userService.getUserAcctInfo(objUserInfo.strUserId);
                ViewBag.UserAcctInfo = objUserAcctInfo;

                // 상품정보
                ViewBag.ItemInfo = _adminItemService.getItemDetail(itemNo);

                // 사용자 쿠폰목록
                ViewBag.CouponListViewModel = _userCouponService.getUserCouponList(objUserInfo.strUserId);
            }
            return View();
        }

        // 배송지 수정(팝업)
        [HttpGet]
        public ActionResult AddrModify()
        {
            return View();
        }

        // 상품구매(선물) 페이지
        [HttpGet]
        public ActionResult GiftPurchase(int itemNo, string ItemColor, string ItemSize, UserInfo objUserInfo)
        {
            UserAcctInfo objUserAcctInfo = null;

            if (Session["strUserId"] != null)
            {
                objUserInfo.strUserId = Session["strUserId"].ToString();

                // 색상 정보
                ViewBag.ItemColor = ItemColor;
                // 사이즈 정보
                ViewBag.ItemSize = ItemSize;

                // 사용자 정보
                objUserInfo = _userService.getUserInfo(objUserInfo.strUserId);
                ViewBag.UserInfo = objUserInfo;

                // 사용자 계좌정보
                objUserAcctInfo = _userService.getUserAcctInfo(objUserInfo.strUserId);
                ViewBag.UserAcctInfo = objUserAcctInfo;

                // 상품정보
                ViewBag.ItemInfo = _adminItemService.getItemDetail(itemNo);

                // 사용자 쿠폰목록
                ViewBag.CouponListViewModel = _userCouponService.getUserCouponList(objUserInfo.strUserId);
            }
            return View();
        }
    }
}