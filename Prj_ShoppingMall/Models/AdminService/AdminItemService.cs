using Prj_ShoppingMall.Models.Info;
using Prj_ShoppingMall.Models.ListViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.AdminService
{
    public class AdminItemService
    {
        private static string strConnectionString = ConfigurationManager.ConnectionStrings["strConnectionString"].ConnectionString;

        // 상품 조회
        public ItemListViewModel getItemList(int intPCategoryNo, int intSubCategoryNo)
        {
            DataTable objDt = new DataTable();
            List<ItemInfo> objItemInfo = new List<ItemInfo>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEM_AR_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_strCpName", SqlDbType.VarChar).Value = intPCategoryNo;
                    cmd.Parameters.Add("@pi_intCpUnit", SqlDbType.Int).Value = intSubCategoryNo;

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
                                objItemInfo.Add(new ItemInfo
                                {

                                });
                            }
                            var viewItem = new ItemListViewModel
                            {
                                Item = objItemInfo
                            };
                            return viewItem;
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
        }

        // 상품 등록
        public CheckInfo itemRegist(ItemInfo objItemInfo, string strAdminId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            string strItemColorNo = String.Join(",", objItemInfo.intItemColorNo.ToArray());
            string strItemSizeNo  = String.Join(",", objItemInfo.intItemSizeNo.ToArray());

            if(objItemInfo.strItemImg != null && objItemInfo.strItemImg.Length > 0)
            {
                string strUploadPath = "/Content/Img/";
                string strFileName   = Path.GetFileName(objItemInfo.strItemImg);
                string strFullPath   = Path.Combine(strUploadPath, strFileName);

                using (SqlConnection conn = new SqlConnection(strConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UP_ITEM_TX_INS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pi_strItemName",      SqlDbType.VarChar).Value = objItemInfo.strItemName;
                        cmd.Parameters.Add("@pi_strAdminId",       SqlDbType.VarChar).Value = strAdminId;
                        cmd.Parameters.Add("@pi_intItemPrice",     SqlDbType.Int).Value     = objItemInfo.intItemPrice;
                        cmd.Parameters.Add("@pi_strItemImg",       SqlDbType.VarChar).Value = strFullPath;
                        cmd.Parameters.Add("@pi_strItemColorNo",   SqlDbType.VarChar).Value = strItemColorNo;
                                                                                             
                        cmd.Parameters.Add("@pi_strItemSizeNo",    SqlDbType.VarChar).Value = strItemSizeNo;
                        cmd.Parameters.Add("@pi_intPCategoryNo",   SqlDbType.Int).Value     = objItemInfo.intPCategoryNo;
                        cmd.Parameters.Add("@pi_intSubCategoryNo", SqlDbType.Int).Value     = objItemInfo.intSubCategoryNo;

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
            else
            {
                return null;
            }
        }

        // 상품 수정
        public CheckInfo itemModify(ItemInfo objItemInfo, string strAdminId)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            string strItemColorNo = String.Join(",", objItemInfo.intItemColorNo.ToArray());
            string strItemSizeNo = String.Join(",", objItemInfo.intItemSizeNo.ToArray());

            return null;
        }

        // 상품 삭제
        public CheckInfo itemDelete()
        {
            return null;
        }
    }
}