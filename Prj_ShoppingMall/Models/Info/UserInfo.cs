using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class UserInfo
    {
        public int    intUserNo     { get; set; } // 회원 번호
        public string strUserId     { get; set; } // 회원 아이디
        public string strUserPwd    { get; set; } // 회원 비밀번호
        public string strUserName   { get; set; } // 회원 이름
        public string strUserTelNum { get; set; } // 회원 전화번호

        public string strAddrPost   { get; set; } // 우편번호
        public string strAddrBase   { get; set; } // 기본주소
        public string strAddrDtl    { get; set; } // 상세주소
        public string strAcctHolder { get; set; } // 예금주
        public string strBankName   { get; set; } // 은행명
                                    
        public string strAcctNo     { get; set; } // 계좌번호
        public string strRegDate    { get; set; } // 회원 등록일
        public string strUpdDate    { get; set; } // 회원 수정일
        public int    intRetVal     { get; set; }
        public string strRetMsg     { get; set; }
    }
}