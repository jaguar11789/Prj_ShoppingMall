using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.Common;

namespace Prj_ShoppingMall.Models.CashService
{
    public class CashService
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        // 결제 요청
        public string requestPayment(string payMethod, int itemPrice, string itemName, string strUserId, int orderNo)
        {
            string strToken = null;
            JObject pl_objResponse = null;
            JObject pl_strPostData = new JObject();
            string strURL = "https://testpgapi.payletter.com/v1.0/payments/request";

            //pl_strPostData.Add("Key", "Value");
            pl_strPostData.Add("pgcode", payMethod);
            pl_strPostData.Add("user_id", strUserId);
            pl_strPostData.Add("user_name", "testuser");
            pl_strPostData.Add("service_name", "페이레터");

            pl_strPostData.Add("client_id", "pay_test");
            pl_strPostData.Add("order_no", orderNo);
            pl_strPostData.Add("amount", itemPrice);
            pl_strPostData.Add("product_Name", itemName);
            pl_strPostData.Add("email_flag", "Y");

            pl_strPostData.Add("email_addr", "jaguar11789@naver.com");
            pl_strPostData.Add("autopay_flag", "N");
            pl_strPostData.Add("receipt_flag", "N");
            pl_strPostData.Add("custom_parameter", "");
            pl_strPostData.Add("return_url", "http://localhost:4707/User/ChargeSuccess");

            pl_strPostData.Add("callback_url", "http://localhost:4707/CashNotification/CashNotification");
            pl_strPostData.Add("cancel_url", "http://localhost:4707/User/ChargeFail");
            pl_strPostData.Add("taxfree_amount", 100);
            pl_strPostData.Add("tax_amount", 20);

            try
            {
                // 요청 처리 성공인 경우                                       
                // Response Parameters (성공시) : token, online_url, mobile_url

                HttpWebRequest objWRequest = (HttpWebRequest)WebRequest.Create(strURL);
                objWRequest.Method = "POST";
                objWRequest.ContentType = "application/json";
                objWRequest.Headers.Add("Authorization", "PLKEY MTFBNTAzNTEwNDAxQUIyMjlCQzgwNTg1MkU4MkZENDA="); // Authorization 설정

                string test = pl_strPostData.ToString();

                byte[] objResultByte = Encoding.UTF8.GetBytes(test);
                objWRequest.ContentLength = objResultByte.Length;

                Stream objStream = objWRequest.GetRequestStream();
                objStream.Write(objResultByte, 0, objResultByte.Length);
                objStream.Close();

                objWRequest.Timeout = 20000;

                // Response Parameters (성공시) : token, online_url, mobile_url
                HttpWebResponse objWResponse = (HttpWebResponse)objWRequest.GetResponse();

                StreamReader objStreamReader = new StreamReader(objWResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string strResponse = objStreamReader.ReadToEnd();

                pl_objResponse = JObject.Parse(strResponse);
                strToken = pl_objResponse.GetValue("token").ToString();

                objStreamReader.Close();
                objWResponse.Close();

                return strToken;

            }
            // 성공이 아닌 경우
            // Response Parameters (실패시) : code, message
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    //Response.Write(reader.ReadToEnd());
                    return null;
                }
            }
        }

        public int ChargeCash(PayResult payResult)
        {
            int intResult = 0;
            string strRetMsg = null;

            try
            {
                string strUserID = payResult.user_id;                      //--가맹점 결제자(회원) 아이디(email, 영문 및 숫자 가능)
                string strUserName = payResult.user_name;                    //--가맹점 결제자(회원) 이름
                int intOrderNo = Int32.Parse(payResult.order_no);        //--가맹점의 주문 번호
                string strServiceName = payResult.service_name;                 //--결제 서비스명
                string strProductName = payResult.product_name;                 //--결제상품명

                string strCustomParameter = payResult.custom_parameter;             //--주문요청시 가맹점이 전송한 값    
                string strTID = payResult.tid;                          //--결제고유번호
                string strCID = payResult.cid;                          //--승인번호
                double dblAmt = payResult.amount;                       //--결제요청 금액
                string strPayInfo = payResult.pay_info;                     //--결제 부가정보

                string strPGCode = payResult.pgcode;                       //--결제요청한 pg명
                string strBillKey = payResult.billkey;                      //--자동결제 재결제용 빌키
                string strDomesticFlag = payResult.domestic_flag;                //--국내 / 해외 신용카드 구분 (Y : 국내, N : 해외)
                string strTransactionDate = payResult.transaction_date;             //--거래일시(YYYY-MM-DD HH:MM:SS)
                string strCardInfo = payResult.card_info;                    //--카드 번호 (중간 4자리 masking 처리)

                string strPayHash = payResult.payhash;                      //--파라메터 검증을 위한 SHA256 hash 값 SHA256(user_id +amount + tid +결제용 API Key) * 일부 결제 수단은 전달되지 않습니다.(가상계좌 등)
                int strInstall_Month = payResult.install_month;                //--할부개월수

                Log.Info("[CashNotificationController][CashNotification] strUserID : " + strUserID);
                Log.Info("[CashNotificationController][CashNotification] strUserName : " + strUserName);
                Log.Info("[CashNotificationController][CashNotification] strOrderNo : " + intOrderNo);
                Log.Info("[CashNotificationController][CashNotification] strServiceName : " + strServiceName);
                Log.Info("[CashNotificationController][CashNotification] strProductName : " + strProductName);

                Log.Info("[CashNotificationController][CashNotification] strCustomParameter : " + strCustomParameter);
                Log.Info("[CashNotificationController][CashNotification] strTID : " + strTID);
                Log.Info("[CashNotificationController][CashNotification] strCID : " + strCID);
                Log.Info("[CashNotificationController][CashNotification] dblAmt : " + dblAmt);
                Log.Info("[CashNotificationController][CashNotification] strPayInfo : " + strPayInfo);

                Log.Info("[CashNotificationController][CashNotification] strPGCode : " + strPGCode);
                Log.Info("[CashNotificationController][CashNotification] strBillKey : " + strBillKey);
                Log.Info("[CashNotificationController][CashNotification] strDomesticFlag : " + strDomesticFlag);
                Log.Info("[CashNotificationController][CashNotification] strTransactionDate : " + strTransactionDate);
                Log.Info("[CashNotificationController][CashNotification] strCardInfo : " + strCardInfo);

                Log.Info("[CashNotificationController][CashNotification] strPayHash : " + strPayHash);
                Log.Info("[CashNotificationController][CashNotification] strInstall_Month : " + strInstall_Month);


                return intResult;
            }
            catch (Exception)
            {

                return intResult;
            }

        }
        public void updatePGLog(PayResult payResult)
        {
            int intResult = 0;
            string strRetMsg = null;
            

            try
            {
                string strUserID = payResult.user_id;                      //--가맹점 결제자(회원) 아이디(email, 영문 및 숫자 가능)
                string strUserName = payResult.user_name;                    //--가맹점 결제자(회원) 이름
                int intOrderNo = Int32.Parse(payResult.order_no);                     //--가맹점의 주문 번호
                string strServiceName = payResult.service_name;                 //--결제 서비스명
                string strProductName = payResult.product_name;                 //--결제상품명

                string strCustomParameter = payResult.custom_parameter;             //--주문요청시 가맹점이 전송한 값    
                string strTID = payResult.tid;                          //--결제고유번호
                string strCID = payResult.cid;                          //--승인번호
                double dblAmt = payResult.amount;                       //--결제요청 금액
                string strPayInfo = payResult.pay_info;                     //--결제 부가정보

                string strPGCode = payResult.pgcode;                       //--결제요청한 pg명
                string strBillKey = payResult.billkey;                      //--자동결제 재결제용 빌키
                string strDomesticFlag = payResult.domestic_flag;                //--국내 / 해외 신용카드 구분 (Y : 국내, N : 해외)
                string strTransactionDate = payResult.transaction_date;             //--거래일시(YYYY-MM-DD HH:MM:SS)
                string strCardInfo = payResult.card_info;                    //--카드 번호 (중간 4자리 masking 처리)

                string strPayHash = payResult.payhash;                      //--파라메터 검증을 위한 SHA256 hash 값 SHA256(user_id +amount + tid +결제용 API Key) * 일부 결제 수단은 전달되지 않습니다.(가상계좌 등)
                int strInstall_Month = payResult.install_month;                //--할부개월수


            }
            catch (Exception)
            {

            }
        }
    }
}