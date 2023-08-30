using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class AdminInfo
    {
        public int    intAdminNo   { get; set; } // 관리자 번호
        public string strAdminId   { get; set; } // 관리자 아이디
        public string strAdminPwd  { get; set; } // 관리자 비밀번호
        public string strAdminName { get; set; } // 관리자 이름
        public string strRegDate   { get; set; } // 관리자 등록일
                                                    
        public string strUpdDate   { get; set; } // 관리자 수정일
        public int    intRetVal    { get; set; }
        public string strRetMsg    { get; set; }
    }
}