using Prj_ShoppingMall.Models.Common;
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

        // 상품 리스트(메인)
        public ItemListViewModel getMainItemList()
        {
            DataTable objDt = new DataTable();
            List<ItemInfo> objLisgItemInfo = new List<ItemInfo>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEM_TX_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);

                            foreach (DataRow row in objDt.Rows)
                            {
                                objLisgItemInfo.Add(new ItemInfo
                                {
                                    intItemNo    = Convert.ToInt32(row["ItemNo"]),
                                    strItemName  = row["ItemName"].ToString(),
                                    intItemPrice = Convert.ToInt32(row["ItemPrice"]),
                                    strItemImg   = row["ItemImg"].ToString()
                                });
                            }
                            var viewItem = new ItemListViewModel
                            {
                                Item = objLisgItemInfo
                            };
                            return viewItem;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[AdminItemService][getMainItemList] ex : " + ex);

                        return null;
                    }
                }
            }
        }

        // 상품 조회
        public ItemListViewModel getItemList(int intPCategoryNo, int intSubCategoryNo, int intItemState)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            DataTable objDt  = new DataTable();
            List<ItemInfo> objLisgItemInfo = new List<ItemInfo>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEM_AR_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intPCategoryNo",   SqlDbType.VarChar).Value = intPCategoryNo;
                    cmd.Parameters.Add("@pi_intSubCategoryNo", SqlDbType.Int).Value     = intSubCategoryNo;
                    cmd.Parameters.Add("@pi_intItemState",     SqlDbType.TinyInt).Value = intItemState;

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
                                objLisgItemInfo.Add(new ItemInfo
                                {
                                    intItemNo          = Convert.ToInt32(row["ItemNo"]),
                                    strPCategoryName   = row["PCategoryName"].ToString(),
                                    strSubCategoryName = row["SubCategoryName"].ToString(),
                                    strItemName        = row["ItemName"].ToString(),
                                    intItemPrice       = Convert.ToInt32(row["ItemPrice"]),
                                                   
                                    strAdminId         = row["AdminId"].ToString(),
                                    strRegDate         = row["RegDate"].ToString()
                                });
                            }
                            var viewItem = new ItemListViewModel
                            {
                                Item = objLisgItemInfo
                            };
                            return viewItem;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[AdminItemService][getItemList] ex : " + ex);

                        return null;
                    }
                }
            }
        }

        // 상품상세
        public ItemInfo getItemDetail(int intItemNo)
        {
            ItemInfo objItemInfo = null;
            DataTable objDt = new DataTable();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEMDTL_AR_LST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intItemNo", SqlDbType.VarChar).Value = intItemNo;

                    conn.Open();

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            DataRow row = objDt.Rows[0];

                            objItemInfo = new ItemInfo
                            {
                                intItemNo          = Convert.ToInt32(row["ItemNo"]),
                                strPCategoryName   = row["PCategoryName"].ToString(),
                                strSubCategoryName = row["SubCategoryName"].ToString(),
                                strItemName        = row["ItemName"].ToString(),
                                intItemPrice       = Convert.ToInt32(row["ItemPrice"]),
                                                  
                                strItemImg         = row["ItemImg"].ToString(),
                            };
                        }
                        return objItemInfo;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[AdminItemService][getItemDetail] ex : " + ex);

                        return null;
                    }
                }
            }
        }

        // 상품 색상
        public List<int> getItemColorInfo(int intItemNo)
        {
            DataTable objDt = new DataTable();
            List<int> lstItemColorInfo = new List<int>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEMCOLOR_AR_DTL", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intItemNo", SqlDbType.Int).Value = intItemNo;

                    conn.Open();

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);
                            foreach (DataRow row in objDt.Rows)
                            {
                                lstItemColorInfo.Add(Convert.ToInt32(row["ItemColorNo"]));
                            }
                        }
                        return lstItemColorInfo;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[AdminItemService][getItemColorInfo] ex : " + ex);

                        return null;
                    }
                }
            }
        }

        // 상품 사이즈정보
        public List<int> getItemSizeInfo(int intItemNo)
        {
            DataTable objDt = new DataTable();
            List<int> lstItemSizeInfo = new List<int>();

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEMSIZE_AR_DTL", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intItemNo", SqlDbType.Int).Value = intItemNo;

                    conn.Open();

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            objDt.Load(reader);
                            foreach (DataRow row in objDt.Rows)
                            {
                                lstItemSizeInfo.Add(Convert.ToInt32(row["ItemSizeNo"]));
                            }
                        }
                        return lstItemSizeInfo;
                    }
                    catch (Exception ex)
                    {
                        Log.Info("[AdminItemService][getItemSizeInfo] ex : " + ex);

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
                            Log.Info("[AdminItemService][itemRegist] ex : " + ex);

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
        public int itemModify(ItemInfo objItemInfo)
        {
            int intRetVal    = -1;
            string strRetMsg = null;

            string strItemColorNo = String.Join(",", objItemInfo.intItemColorNo.ToArray());
            string strItemSizeNo  = String.Join(",", objItemInfo.intItemSizeNo.ToArray());

            if (objItemInfo.strItemImg != null && objItemInfo.strItemImg.Length > 0)
            {
                string strUploadPath = "/Content/Img/";
                string strFileName = Path.GetFileName(objItemInfo.strItemImg);
                string strFullPath = Path.Combine(strUploadPath, strFileName);

                using (SqlConnection conn = new SqlConnection(strConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UP_ITEM_TX_UPD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pi_intItemNo",        SqlDbType.Int).Value     = objItemInfo.intItemNo;
                        cmd.Parameters.Add("@pi_strItemName",      SqlDbType.VarChar).Value = objItemInfo.strItemName;
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

                            return intRetVal;
                        }
                        catch (Exception ex)
                        {
                            Log.Info("[AdminItemService][itemRegist] ex : " + ex);

                            return intRetVal;
                        }
                    }
                }
            }
            else
            {
                return intRetVal;
            }
        }

        // 상품 삭제(미등록)
        public CheckInfo itemDelete(int intItemNo)
        {
            int intRetVal    = -1;
            string strRetMsg = null;
            CheckInfo objCheckInfo = null;

            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UP_ITEM_TX_DEL", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pi_intItemNo", SqlDbType.VarChar).Value = intItemNo;

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
                        Log.Info("[AdminItemService][itemDelete] ex : " + ex);

                        return objCheckInfo;
                    }
                }
            }
        }
    }
}