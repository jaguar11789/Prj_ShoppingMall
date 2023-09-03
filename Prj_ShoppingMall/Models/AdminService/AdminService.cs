using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Prj_ShoppingMall.Models.AdminService
{
    public class AdminService
    {
        private static string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        public Tuple<int, string> adminLogin(AdminInfo objAdminInfo)
        {
            int intRetVal = 0;
            string strRetMsg = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ADMIN_NT_CHK", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strAdminId",  SqlDbType.VarChar).Value = objAdminInfo.strAdminId;
                    cmd.Parameters.Add("@pi_strAdminPwd", SqlDbType.VarChar).Value = objAdminInfo.strAdminPwd;

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

                        return Tuple.Create(intRetVal, strRetMsg);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}