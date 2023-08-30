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
            pl_strPostData.Add("pgcode"          , payMethod);
            pl_strPostData.Add("user_id"         , strUserId);
            pl_strPostData.Add("user_name"       , "강승원");
            pl_strPostData.Add("service_name"    , "페이레터");
                                                 
            pl_strPostData.Add("client_id"       , "pay_test");
            pl_strPostData.Add("order_no"        , orderNo);
            pl_strPostData.Add("amount"          , itemPrice);
            pl_strPostData.Add("product_Name"    , itemName);
            pl_strPostData.Add("email_flag"      , "Y");
                                                 
            pl_strPostData.Add("email_addr"      , "jaguar11789@naver.com");
            pl_strPostData.Add("autopay_flag"    , "N");
            pl_strPostData.Add("receipt_flag"    , "N");
            pl_strPostData.Add("custom_parameter", "");
            pl_strPostData.Add("return_url"      , "http://localhost:4707/User/ChargeSuccess");

            pl_strPostData.Add("callback_url"    , "");
            pl_strPostData.Add("cancel_url"      , "http://localhost:4707/User/ChargeFail");
            pl_strPostData.Add("taxfree_amount"  , 100);
            pl_strPostData.Add("tax_amount"      , 20);

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
    }
}