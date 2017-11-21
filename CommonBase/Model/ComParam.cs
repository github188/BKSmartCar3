using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase.Model
{
    /// <summary>
    /// 串口参数
    /// </summary>
    public class ComParam
    {
        private string com;    //串口号
        private int rate;   //波特率
        private int dataBits;   //数据位

        private string pwd; // 密码位？OBD采样时需要该字段，具体要问下郑义

        public ComParam(string _com, int _rate, string _pwd, int _dataBits = 8)
        {
            com = _com;
            rate = _rate;
            pwd = _pwd;
            dataBits = _dataBits;
        }

        public string Com
        {
            get { return com; }
            set { com = value; }
        }

        public int Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public int DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }
}
