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
    public class AdminItemController : Controller
    {
        AdminItemService _adminItemSerivce = new AdminItemService();
        // GET: AdminItem
        public ActionResult Index()
        {
            return View();
        }

        // 상품조회
        [HttpPost]
        public JsonResult ItemCheck(int intPCategoryNo, int intSubCategoryNo)
        {
            if (Session["strAdminId"] != null)
            {
                ItemListViewModel objResult = null;
                objResult = _adminItemSerivce.getItemList(intPCategoryNo, intSubCategoryNo);

                return Json(objResult);
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}