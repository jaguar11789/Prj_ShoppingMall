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
        public JsonResult ItemCheck(int intPCategoryNo, int intSubCategoryNo, int intItemState)
        {
            if (Session["strAdminId"] != null)
            {
                ItemListViewModel objResult = null;
                objResult = _adminItemSerivce.getItemList(intPCategoryNo, intSubCategoryNo, intItemState);

                return Json(objResult);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // 상품 상태변경
        [HttpPost]
        public JsonResult ItemDelete(int intItemNo)
        {
            CheckInfo objCheckInfo = null;

            objCheckInfo = _adminItemSerivce.itemDelete(intItemNo);

            return Json(objCheckInfo);
        }

        // 상품 수정
        [HttpPost]
        public ActionResult ItemModify(ItemInfo objItemInfo)
        {
            if (Session["strAdminId"] != null)
            {
                int intResult = _adminItemSerivce.itemModify(objItemInfo);

                if (intResult == 0)
                {
                    return RedirectToAction("ItemModifySuccess", "Admin");
                }
                else
                {
                    return RedirectToAction("ItemModifyFail", "Admin");
                }
            }
            else
            {
                return RedirectToAction("AdminIndex", "Admin");
            }
        }

        
    }
}