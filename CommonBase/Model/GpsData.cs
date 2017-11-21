using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase.Model
{
    /// <summary>
    /// GPS 数据
    /// </summary>
    public class GpsData
    {
        public float sd;    //速度
        public double jd;    //经度
        public double wd;    //纬度
        public float fxj;    //航向角
        public float fyj;    //俯仰角
        public float hgj;             //横滚角
        public double gd;             //高度
        public string clqy;            //车轮区域
        public string ctqy;            //车体区域
        public double qjjl;             //前进距离
        public double htjl;             //后退距离
        public string zt;          //GPS状态 

        public GpsData()
        {
            sd = 0;
            jd = 0;
            wd = 0;
            fxj = 0;
            fyj = 0;
            hgj = 0;
            gd = 0;
            clqy = "";
            ctqy = "";
            qjjl = 0;
            htjl = 0;
            zt = "";
        }

        public float Sd
        {
            get { return sd; }
            set { sd = value; }
        }

        public double Jd
        {
            get { return jd; }
            set { jd = value; }
        }

        public double Wd
        {
            get { return wd; }
            set { wd = value; }
        }

        public float Fxj
        {
            get { return fxj; }
            set { fxj = value; }
        }

        public float Fyj
        {
            get { return fyj; }
            set { fyj = value; }
        }

        public float Hgj
        {
            get { return hgj; }
            set { hgj = value; }
        }

        public double Gd
        {
            get { return gd; }
            set { gd = value; }
        }

        public string Clqy
        {
            get { return clqy; }
            set { clqy = value; }
        }

        public string Ctqy
        {
            get { return ctqy; }
            set { ctqy = value; }
        }

        public double Qjjl
        {
            get { return qjjl; }
            set { qjjl = value; }
        }

        public double Htjl
        {
            get { return htjl; }
            set { htjl = value; }
        }

        public string Zt
        {
            get { return zt; }
            set { zt = value; }
        }

    }
}
