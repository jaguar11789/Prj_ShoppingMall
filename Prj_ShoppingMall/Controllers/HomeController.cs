using Prj_ShoppingMall.Models.AdminService;
using Prj_ShoppingMall.Models.ListViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prj_ShoppingMall.Controllers
{
    public class HomeController : Controller
    {
        AdminItemService _adminItemService = new AdminItemService();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // 메인
        [HttpGet]
        public ActionResult Main()
        {
            ItemListViewModel objResult = null;

            objResult = _adminItemService.getMainItemList();
            ViewBag.MainItemList = objResult;

            return View();
        }
    }
}