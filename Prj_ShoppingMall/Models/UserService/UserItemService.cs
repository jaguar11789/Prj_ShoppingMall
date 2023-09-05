using Prj_ShoppingMall.Models.Common;
using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.UserService
{
    public class UserItemService
    {
        private static string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        // 배송지 수정
        public CheckInfo userAddrUpdate(UserInfo objUserInfo, string strUserId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USERADDR_TX_UPD", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId",     SqlDbType.VarChar).Value = strUserId;
                    cmd.Parameters.Add("@pi_strUserName",   SqlDbType.VarChar).Value = objUserInfo.strUserName;
                    cmd.Parameters.Add("@pi_strAddrPost",   SqlDbType.VarChar).Value = objUserInfo.strAddrPost;
                    cmd.Parameters.Add("@pi_strAddrBase",   SqlDbType.VarChar).Value = objUserInfo.strAddrBase;
                    cmd.Parameters.Add("@pi_strAddrDtl",    SqlDbType.VarChar).Value = objUserInfo.strAddrDtl;

                    cmd.Parameters.Add("@pi_strUserTelNum", SqlDbType.VarChar).Value = objUserInfo.strUserTelNum;

                    SqlParameter outputParamRetVal  = new SqlParameter();
                    outputParamRetVal.ParameterName = "@po_intRetVal";
                    outputParamRetVal.SqlDbType     = SqlDbType.Int;
                    outputParamRetVal.Direction     = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParamRetVal);

                    SqlParameter outputParamRetMsg  = new SqlParameter();
                    outputParamRetMsg.ParameterName = "@po_strRetMsg";
                    outputParamRetMsg.SqlDbType     = SqlDbType.VarChar;
                    outputParamRetMsg.Direction     = ParameterDirection.Output;
                    outputParamRetMsg.Size          = 256;
                    cmd.Parameters.Add(outputParamRetMsg);

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();

                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        objCheckInfo = new CheckInfo
                        {
                            intRetVal = intRetVal,
                            strRetMsg = strRetMsg
                        };
                        return objCheckInfo;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[UserItemService][userAddrUpdate] ex : " + ex);

                        return objCheckInfo;
                    }
                }
            }
        }

        // 상품 구매
        public CheckInfo purchaseItem(UserInfo objUserInfo, ItemInfo objItemInfo ,string strItemColor, string strItemSize, long lngCpCode, string strReceiveUserId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_PURCHASE_TX_INS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId",        SqlDbType.VarChar).Value = objUserInfo.strUserId;
                    cmd.Parameters.Add("@pi_intItemNo",        SqlDbType.Int).Value     = objItemInfo.intItemNo;
                    cmd.Parameters.Add("@pi_strItemName",      SqlDbType.VarChar).Value = objItemInfo.strItemName;
                    cmd.Parameters.Add("@pi_strItemColor",     SqlDbType.VarChar).Value = strItemColor;
                    cmd.Parameters.Add("@pi_strItemSize",      SqlDbType.VarChar).Value = strItemSize;

                    cmd.Parameters.Add("@pi_intItemPrice",     SqlDbType.Int).Value     = objItemInfo.intItemPrice;
                    cmd.Parameters.Add("@pi_strReceiveUserId", SqlDbType.VarChar).Value = strReceiveUserId;
                    cmd.Parameters.Add("@pi_intCpCode",        SqlDbType.BigInt).Value  = lngCpCode;

                    SqlParameter outputParamRetVal  = new SqlParameter();
                    outputParamRetVal.ParameterName = "@po_intRetVal";
                    outputParamRetVal.SqlDbType     = SqlDbType.Int;
                    outputParamRetVal.Direction     = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParamRetVal);

                    SqlParameter outputParamRetMsg  = new SqlParameter();
                    outputParamRetMsg.ParameterName = "@po_strRetMsg";
                    outputParamRetMsg.SqlDbType     = SqlDbType.VarChar;
                    outputParamRetMsg.Direction     = ParameterDirection.Output;
                    outputParamRetMsg.Size          = 256;
                    cmd.Parameters.Add(outputParamRetMsg);

                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();

                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        objCheckInfo = new CheckInfo
                        {
                            intRetVal = intRetVal,
                            strRetMsg = strRetMsg
                        };
                        return objCheckInfo;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[UserItemService][userAddrUpdate] ex : " + ex);

                        return objCheckInfo;
                    }
                }
            }
        }
    }
}