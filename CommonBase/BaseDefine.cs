using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    /// <summary>
    /// 一些通用的宏定义
    /// </summary>
    public class BaseDefine
    {
        public static readonly int COMMON_VALUE_1024 = 1024;
        public static readonly int COMMON_VALUE_512 = 512;

        public static readonly string DATA_TYPE_OBD_DT = @"OBD-DT";
        public static readonly string DATA_TYPE_OBD_INFO = @"OBD-INFO";
        public static readonly string DATA_TYPE_OBD_IO = @"OBD-IO";

        //档位
        public static readonly string DATA_TYPE_DW_MN = @"MN";
        public static readonly string DATA_TYPE_DW_M1 = @"M1";
        public static readonly string DATA_TYPE_DW_M2 = @"M2";
        public static readonly string DATA_TYPE_DW_M3 = @"M3";
        public static readonly string DATA_TYPE_DW_M4 = @"M4";
        public static readonly string DATA_TYPE_DW_M5 = @"M5";
        public static readonly string DATA_TYPE_DW_MR = @"MR";
        public static readonly string DATA_TYPE_DW_AN = @"AN";
        public static readonly string DATA_TYPE_DW_AP = @"AP";
        public static readonly string DATA_TYPE_DW_A1 = @"A1";
        public static readonly string DATA_TYPE_DW_A2 = @"A2";
        public static readonly string DATA_TYPE_DW_A3 = @"A3";
        public static readonly string DATA_TYPE_DW_A4 = @"A4";
        public static readonly string DATA_TYPE_DW_A5 = @"A5";
        public static readonly string DATA_TYPE_DW_AR = @"AR";
        public static readonly string DATA_TYPE_DW_AD = @"AD";
    }

    //档位定义
    public enum DwDefine
    {
        DW_N = 0,   //空档
        DW_1 = 1,
        DW_2 = 2,
        DW_3 = 3,
        DW_4 = 4,
        DW_5 = 5,
        DW_D = 8,
        DW_P = 9,
        DW_R = 10,
        DW_OTHER = 100
    }
}
