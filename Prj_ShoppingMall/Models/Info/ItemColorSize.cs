using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prj_ShoppingMall.Models.Info
{
    public class ItemColorSize
    {
        public static string ColorNoToString(int colorCode)
        {
            switch (colorCode)
            {
                case 1: return "Black";
                case 2: return "Navy";
                case 3: return "Cream";
                case 4: return "Gray";
                case 5: return "Lemon";
                case 6: return "Pink";
                case 7: return "Mint";
                case 8: return "White";
                case 9: return "Brick";
                case 10: return "Brown";
                case 11: return "Yellow";
                case 12: return "Beige";
                case 13: return "Blue";
                case 14: return "Green";
                case 15: return "Red";
                case 16: return "Ivory";

                default: return "Unknown";
            }
        }
        public static string SizeNoToString(int sizeCode)
        {
            switch (sizeCode)
            {
                case 1: return "M";
                case 2: return "L";
                case 3: return "XL";
                case 4: return "2XL";
                case 5: return "3XL";
                case 6: return "FREE";
                default: return "Unknown";
            }
        }
    }
}