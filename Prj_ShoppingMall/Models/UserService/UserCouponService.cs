using Prj_ShoppingMall.Models.Common;
using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.ListViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.UserService
{
    public class UserCouponService
    {
        private static string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        public CheckInfo couponRegist(long lngCpCode, string strUserId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USERCOUPON_TX_INS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_bintCpCode", SqlDbType.BigInt).Value  = lngCpCode;
                    cmd.Parameters.Add("@pi_strUserId",  SqlDbType.VarChar).Value = strUserId;

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
                        Log.Info("[UserCouponService][couponRegist] ex : " + ex);

                        return objCheckInfo;
                    }
                }
            }
        }

        // 사용자 쿠폰목록
        public CouponListViewModel getUserCouponList(string strUserId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            DataTable objDt  = new DataTable();
            List<CouponInfo> objListCouponInfo = new List<CouponInfo>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USERCOUPON_AR_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId", SqlDbType.VarChar).Value = strUserId;

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
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);

                            foreach (DataRow row in objDt.Rows)
                            {
                                objListCouponInfo.Add(new CouponInfo
                                {
                                    strCpName = row["CpName"].ToString(),
                                    lngCpCode = Convert.ToInt64(row["CpCode"]),
                                });
                            }
                        }

                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        var viewCoupon = new CouponListViewModel
                        {
                            Coupon    = objListCouponInfo,
                            intRetVal = intRetVal,
                            strRetMsg = strRetMsg
                        };

                        return viewCoupon;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[UserCouponService][getUserCouponList] ex : " + ex);

                        return null;
                    }
                }
            }
        }
    }
}