using Prj_ShoppingMall.Models.Info;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data;
using System.Configuration;

namespace Prj_ShoppingMall.Models.UserService
{
    public class UserService
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;
        // 사용자 회원가입
        public UserInfo userRegister(UserInfo objUserInfo)
        {
            int intRetVal     = -1;
            string strRetMsg  = null;
            string strUserPwd = null;

            SHA256 sha  = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(objUserInfo.strUserPwd));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            strUserPwd = stringBuilder.ToString();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USER_TX_INS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId",     SqlDbType.VarChar).Value = objUserInfo.strUserId;
                    cmd.Parameters.Add("@pi_strUserPwd",    SqlDbType.VarChar).Value = strUserPwd;
                    cmd.Parameters.Add("@pi_strUserName",   SqlDbType.VarChar).Value = objUserInfo.strUserName;
                    cmd.Parameters.Add("@pi_strUserTelNum", SqlDbType.VarChar).Value = objUserInfo.strUserTelNum;
                    cmd.Parameters.Add("@pi_strAddrPost",   SqlDbType.VarChar).Value = objUserInfo.strAddrPost;
                                                            
                    cmd.Parameters.Add("@pi_strAddrBase",   SqlDbType.VarChar).Value = objUserInfo.strAddrBase;
                    cmd.Parameters.Add("@pi_strAddrDtl",    SqlDbType.VarChar).Value = objUserInfo.strAddrDtl;
                    cmd.Parameters.Add("@pi_strAcctHoler",  SqlDbType.VarChar).Value = objUserInfo.strAcctHolder;
                    cmd.Parameters.Add("@pi_strBankName",   SqlDbType.VarChar).Value = objUserInfo.strBankName;
                    cmd.Parameters.Add("@pi_strAcctNo",     SqlDbType.VarChar).Value = objUserInfo.strAcctNo;

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

                        objUserInfo = new UserInfo
                        {
                            intRetVal = intRetVal,
                            strRetMsg = strRetMsg
                        };

                        return objUserInfo;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        // 사용자 아이디 중복확인
        public int userIdDoubleCheck(string strUserId)
        {
            int intRetVal = -1;
            string strRetMsg = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USERID_TX_CHK", conn))
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
                        cmd.ExecuteNonQuery();
                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        return (intRetVal);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }


        // 사용자 로그인
        public Tuple<int, string> userLogin(UserInfo objUserInfo)
        {
            int intRetVal     = 0;
            string strRetMsg  = null;
            string strUserPwd = null;

            SHA256 sha  = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(objUserInfo.strUserPwd));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            strUserPwd = stringBuilder.ToString();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USER_NT_CHK", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId",  SqlDbType.VarChar).Value = objUserInfo.strUserId;
                    cmd.Parameters.Add("@pi_strUserPwd", SqlDbType.VarChar).Value = strUserPwd;

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

        // 사용자 정보
        public UserInfo getUserInfo(string strUserId)
        {
            int intRetVal        = 0;
            string strRetMsg     = null;
            UserInfo objUserInfo = null;
            DataTable objDt      = new DataTable();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USERINFO_AR_LST", conn))
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
                        intRetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                        strRetMsg = Convert.ToString(cmd.Parameters["@po_strRetMsg"].Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            DataRow row = objDt.Rows[0];

                            objUserInfo = new UserInfo
                            {
                                intUserNo     = Convert.ToInt32(row["UserNo"]),
                                strUserId     = row["UserId"].ToString(),
                                strUserName   = row["UserName"].ToString(),
                                strAddrPost   = row["AddrPost"].ToString(),
                                strAddrBase   = row["AddrBase"].ToString(),

                                strAddrDtl    = row["AddrDtl"].ToString(),
                                strUserTelNum = row["UserTelNum"].ToString(),
                                strAcctHolder = row["AcctHolder"].ToString(),
                                strBankName   = row["BankName"].ToString(),
                                strAcctNo     = row["AcctNo"].ToString(),

                                strRegDate    = row["RegDate"].ToString(),
                                strUpdDate    = row["UpdDate"].ToString(),

                                intRetVal     = intRetVal,
                                strRetMsg     = strRetMsg
                            };
                        }
                        return objUserInfo;
                    }
                    catch (Exception)
                    {
                        return objUserInfo;
                    }
                }
            }
        }

        public UserInfo UserModify(UserInfo objUserInfo, string strUserId)
        {
            int intRetVal        = -1;
            string strRetMsg     = null;
            string strUserPwd    = null;

            SHA256 sha  = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(objUserInfo.strUserPwd));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            strUserPwd = stringBuilder.ToString();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_USER_TX_UPD", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strUserId",     SqlDbType.VarChar).Value = strUserId;
                    cmd.Parameters.Add("@pi_strUserPwd",    SqlDbType.VarChar).Value = strUserPwd;
                    cmd.Parameters.Add("@pi_strUserName",   SqlDbType.VarChar).Value = objUserInfo.strUserName;
                    cmd.Parameters.Add("@pi_strAddrPost",   SqlDbType.VarChar).Value = objUserInfo.strAddrPost;
                    cmd.Parameters.Add("@pi_strAddrBase",   SqlDbType.VarChar).Value = objUserInfo.strAddrBase;

                    cmd.Parameters.Add("@pi_strAddrDtl",    SqlDbType.VarChar).Value = objUserInfo.strAddrDtl;
                    cmd.Parameters.Add("@pi_strUserTelNum", SqlDbType.VarChar).Value = objUserInfo.strUserTelNum;
                    cmd.Parameters.Add("@pi_strAcctHoler",  SqlDbType.VarChar).Value = objUserInfo.strAcctHolder;
                    cmd.Parameters.Add("@pi_strBankName",   SqlDbType.VarChar).Value = objUserInfo.strBankName;
                    cmd.Parameters.Add("@pi_strAcctNo",     SqlDbType.VarChar).Value = objUserInfo.strAcctNo;

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

                        objUserInfo = new UserInfo
                        {
                            intRetVal = intRetVal,
                            strRetMsg = strRetMsg
                        };

                        return objUserInfo;
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