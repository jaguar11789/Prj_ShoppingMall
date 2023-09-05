using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.UserService
{
    public class UserItemService
    {
        private static string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;
    }
}