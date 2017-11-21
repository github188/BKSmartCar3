using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase.Model
{
    /// <summary>
    /// 采样数据
    /// </summary>
    public class SampleData
    {
        private byte aqd;                //安全带
        private byte zzxd;               //左转向灯
        private byte yzxd;               //右转向灯
        private byte xh;                 //熄火
        private byte dh;                 //点火
        private byte ss;                 //手刹
        private byte gm;                 //关门信号

        private int zs;                  //转速
        private float ycsd;              //云车速度
        private byte dw;              //档位信号
        private byte jgd;              //近光灯
        private byte ygd;              //远光灯
        private byte js;              //脚刹
        private byte dc;              //倒车

        private byte wd;              //雾灯
        private byte lb;              //喇叭
        private byte yx;              //油箱
        private byte fs;              //副刹
        private byte lh;              //离合
        private byte gjd;       //危险告警灯
        private byte skd; //示廓灯

        public SampleData()
        {
            aqd = 0;
            zzxd = 0;
            yzxd = 0;
            xh = 0;
            dh = 0;
            ss = 0;
            gm = 0;
            zs = 0;
            ycsd = 0;
            dw = 0;
            jgd = 0;
            ygd = 0;
            js = 0;
            dc = 0;
            wd = 0;
            lb = 0;
            yx = 0;
            fs = 0;
            lh = 0;
            gjd = 0;
            skd = 0;
        }

        public byte Aqd
        {
            get { return aqd; }
            set { aqd = value; }
        }

        public byte Zzxd
        {
            get { return zzxd; }
            set { zzxd = value; }
        }

        public byte Yzxd
        {
            get { return yzxd; }
            set { yzxd = value; }
        }

        public byte Xh
        {
            get { return xh; }
            set { xh = value; }
        }

        public byte Dh
        {
            get { return dh; }
            set { dh = value; }
        }

        public byte Ss
        {
            get { return ss; }
            set { ss = value; }
        }

        public byte Gm
        {
            get { return gm; }
            set { gm = value; }
        }

        public int Zs
        {
            get { return zs; }
            set { zs = value; }
        }

        public float Ycsd
        {
            get { return ycsd; }
            set { ycsd = value; }
        }

        public byte Dw
        {
            get { return dw; }
            set { dw = value; }
        }

        public byte Jgd
        {
            get { return jgd; }
            set { jgd = value; }
        }

        public byte Ygd
        {
            get { return ygd; }
            set { ygd = value; }
        }

        public byte Js
        {
            get { return js; }
            set { js = value; }
        }

        public byte Dc
        {
            get { return dc; }
            set { dc = value; }
        }

        public byte Wd
        {
            get { return wd; }
            set { wd = value; }
        }

        public byte Lb
        {
            get { return lb; }
            set { lb = value; }
        }

        public byte Yx
        {
            get { return yx; }
            set { yx = value; }
        }

        public byte Fs
        {
            get { return fs; }
            set { fs = value; }
        }

        public byte Lh
        {
            get { return lh; }
            set { lh = value; }
        }

        public byte Gjd
        {
            get { return gjd; }
            set { gjd = value; }
        }

        public byte Skd
        {
            get { return skd; }
            set { skd = value; }
        }

    }
}
