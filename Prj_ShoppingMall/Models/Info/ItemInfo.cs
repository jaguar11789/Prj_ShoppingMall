using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class ItemInfo
    {
        public int          intItemNo          { get; set; }
        public int          intAdminNo         { get; set; }
        public string       strAdminId         { get; set; }
        public int          intCategoryNo      { get; set; }
        public string       strItemName        { get; set; }
                                            
        public int          intItemPrice       { get; set; }
        public string       strItemImg         { get; set; }
        public string       strRegDate         { get; set; }
        public string       strUpdDate         { get; set; }
        public List<int>    intItemColorNo     { get; set; }
                                            
        public string       strItemColor       { get; set; }
        public List<int>    intItemSizeNo      { get; set; }
        public string       strItemSize        { get; set; }
        public int          intPCategoryNo     { get; set; }
        public string       strPCategoryName   { get; set; }
                                            
        public int          intSubCategoryNo   { get; set; }
        public string       strSubCategoryName { get; set; }
        public List<string> objItemColorList   { get; set; }
        public List<string> objItemSizeList    { get; set; }
        public int          intItemState       { get; set; }

        public int          intDeliveryPrice   { get; set; } = 2500;
    }
}