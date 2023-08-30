using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.ListViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.AdminService
{
    public class AdminCouponService
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        public CheckInfo couponIssued(CouponInfo objCouponInfo, string strAdminId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_COUPON_TX_INS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strCpName",  SqlDbType.VarChar).Value = objCouponInfo.strCpName;
                    cmd.Parameters.Add("@pi_intCpUnit",  SqlDbType.Int).Value     = objCouponInfo.intCpUnit;
                    cmd.Parameters.Add("@pi_bintCpCode", SqlDbType.BigInt).Value  = objCouponInfo.lngCpCode;
                    cmd.Parameters.Add("@pi_intCpCount", SqlDbType.Int).Value     = objCouponInfo.intCpCount;
                    cmd.Parameters.Add("@pi_strAdminId", SqlDbType.VarChar).Value = strAdminId;

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
                        return objCheckInfo;
                    }
                }
            }
        }

        public CouponListViewModel getCouponList(int intCpUnit, int intCpState)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            DataTable objDt  = new DataTable();
            List<CouponInfo> objListCouponInfo = new List<CouponInfo>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_COUPON_AR_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intCpUnit",  SqlDbType.VarChar).Value = intCpUnit;
                    cmd.Parameters.Add("@pi_intCpState", SqlDbType.Int).Value     = intCpState;

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
                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);

                            foreach (DataRow row in objDt.Rows)
                            {
                                objListCouponInfo.Add(new CouponInfo
                                {
                                    intCpNo    = Convert.ToInt32(row["CpNo"]),
                                    strCpName  = row["CpName"].ToString(),
                                    strCpUnit  = row["CpUnit"].ToString(),
                                    lngCpCode  = Convert.ToInt64(row["CpCode"]),
                                    strRegDate = row["RegDate"].ToString(),

                                    strUseDate = row["UseDate"].ToString(),
                                    strUserId  = row["UserId"].ToString(),
                                    strCpState = row["CpState"].ToString()
                                });
                            }
                            var viewCoupon = new CouponListViewModel
                            {
                                Coupon = objListCouponInfo
                            };
                            return viewCoupon;
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                }
            }
        }
    }
}