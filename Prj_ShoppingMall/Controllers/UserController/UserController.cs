using Prj_ShoppingMall.Models.Info;
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

        [HttpGet]
        public ActionResult ChargeSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChargeFail()
        {
            return View();
        }
    }
}