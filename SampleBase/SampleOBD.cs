using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BekUtils.Util;
using log4net;
using System.Threading;
using CommonBase.Model;
using CommonBase;

namespace SampleBase
{
    /// <summary>
    /// OBD 采样
    /// </summary>
    public class SampleOBD : ComSampleBase
    {
        private ILog logger = Log.GetLogger();
        private BekUtils.Communication.ComUtil com = null;
        private Thread sampleThread = null;
        private SampleData sampleData = new SampleData();
        private string[] signalData = null;
        private static readonly object lockObj = new object();

        //设置串口参数
        public override void SetCom(ComParam param)
        {
            if (string.IsNullOrEmpty(param.Com) || param.Rate <= 0)
            {
                logger.ErrorFormat("SetCom param error, portname={0}, baudrate={1}", param.Com, param.Rate);
                return;
            }

            string errorMsg = string.Empty;
            signalData = BaseMethod.SplitString(param.Pwd, ',', out errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                logger.ErrorFormat("SetCom param error, pwd={0}", param.Pwd);
                return;
            }

            com = new BekUtils.Communication.ComUtil(param.Com, param.Rate, param.DataBits);
        }

        //开始采样
        public override void StartSample()
        {
            sampleThread = new Thread(SampleThreadProc);
            sampleThread.Start();
        }

        //结束采样
        public override void StopSample()
        {
            BekUtils.Util.BaseMethod.StopThread(sampleThread);
        }

        //获取采样信号
        public override void RecvData(ref SampleData data)
        {
            try
            {
                lock (lockObj)
                {
                    data.Aqd = sampleData.Aqd;
                    data.Zzxd = sampleData.Zzxd;
                    data.Yzxd = sampleData.Yzxd;
                    data.Xh = sampleData.Xh;
                    data.Dh = sampleData.Dh;
                    data.Ss = sampleData.Ss;
                    data.Gm = sampleData.Gm;
                    data.Zs = sampleData.Zs;
                    data.Ycsd = sampleData.Ycsd;
                    data.Dw = sampleData.Dw;
                    data.Jgd = sampleData.Jgd;
                    data.Ygd = sampleData.Ygd;
                    data.Js = sampleData.Js;
                    data.Dc = sampleData.Dc;
                    data.Wd = sampleData.Wd;
                    data.Lb = sampleData.Lb;
                    data.Yx = sampleData.Yx;
                    data.Fs = sampleData.Fs;
                    data.Lh = sampleData.Lh;
                    data.Gjd = sampleData.Gjd;
                    data.Skd = sampleData.Skd;
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }
        }

        //采样线程函数
        private void SampleThreadProc()
        {
            while (true)
            {
                #region 连接串口
                if (!com.IsOpen)
                {
                    int nRet = com.ConnectSerialPort();
                    if (-1 == nRet)
                    {
                        logger.ErrorFormat("ConnectSerialPort faild, retry after sleep");
                        Thread.Sleep(2500);
                        continue;
                    }
                    else if (-2 == nRet)
                    {
                        logger.ErrorFormat("ConnectSerialPort faild, abort sample thread");
                        sampleThread.Abort();
                        return;
                    }
                }
                #endregion

                //读 OBD 数据
                string errorMsg = string.Empty;
                string retStr = string.Empty;
                int nLen = com.ReadSerialPort(ref retStr);
                string[] destList = BaseMethod.SplitString(retStr, '$', out errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    logger.ErrorFormat(errorMsg);
                    Thread.Sleep(200);
                    continue;
                }

                //解析 OBD 数据
                for (int i = 0; i < destList.Length; i++)
                {
                    string tempError = string.Empty;
                    string[] dest = BaseMethod.SplitString(destList[i], ',', out tempError);
                    if (!string.IsNullOrEmpty(tempError))
                    {
                        logger.InfoFormat("OBD 存在空数据，destList[{0}]={1}", i, destList[i]);
                        continue;
                    }

                    if (BaseDefine.DATA_TYPE_OBD_DT == dest[0])
                    {
                        if (dest.Length >= 22)
                        {
                            #region 从协议取值
                            lock (lockObj)
                            {
                                #region 安全带
                                if ("1" == dest[12])
                                {
                                    sampleData.Aqd = 1;
                                }
                                else if ("#" == dest[12])
                                {
                                    sampleData.Aqd = 9;
                                }
                                else
                                {
                                    sampleData.Aqd = 0;
                                }
                                #endregion

                                #region 脚刹
                                if ("1" == dest[7])
                                {
                                    sampleData.Js = 1;
                                }
                                else if ("#" == dest[7])
                                {
                                    sampleData.Js = 9;
                                }
                                else
                                {
                                    sampleData.Js = 0;
                                }
                                #endregion

                                #region 关门
                                if ("1" == dest[13])
                                {
                                    sampleData.Gm = 1;
                                }
                                else if ("#" == dest[13])
                                {
                                    sampleData.Gm = 9;
                                }
                                else
                                {
                                    sampleData.Gm = 0;
                                }
                                #endregion

                                #region 离合
                                if ("1" == dest[6])
                                {
                                    sampleData.Lh = 1;
                                }
                                else if ("#" == dest[6])
                                {
                                    sampleData.Lh = 9;
                                }
                                else
                                {
                                    sampleData.Lh = 0;
                                }
                                #endregion

                                #region 左转向灯
                                if ("1" == dest[14])
                                {
                                    sampleData.Zzxd = 1;
                                }
                                else if ("#" == dest[14])
                                {
                                    sampleData.Zzxd = 9;
                                }
                                else
                                {
                                    sampleData.Zzxd = 0;
                                }
                                #endregion

                                #region 右转向灯
                                if ("1" == dest[15])
                                {
                                    sampleData.Yzxd = 1;
                                }
                                else if ("#" == dest[15])
                                {
                                    sampleData.Yzxd = 9;
                                }
                                else
                                {
                                    sampleData.Yzxd = 0;
                                }
                                #endregion

                                #region 喇叭
                                if ("1" == dest[10])
                                {
                                    sampleData.Lb = 1;
                                }
                                else if ("#" == dest[10])
                                {
                                    sampleData.Lb = 9;
                                }
                                else
                                {
                                    sampleData.Lb = 0;
                                }
                                #endregion

                                #region 近光灯
                                if ("1" == dest[17])
                                {
                                    sampleData.Jgd = 1;
                                }
                                else if ("#" == dest[17])
                                {
                                    sampleData.Jgd = 9;
                                }
                                else
                                {
                                    sampleData.Jgd = 0;
                                }
                                #endregion

                                #region 远光灯
                                if ("1" == dest[18])
                                {
                                    sampleData.Ygd = 1;
                                }
                                else if ("#" == dest[18])
                                {
                                    sampleData.Ygd = 9;
                                }
                                else
                                {
                                    sampleData.Ygd = 0;
                                }
                                #endregion

                                #region 危险告警灯
                                if ("1" == dest[19])
                                {
                                    sampleData.Gjd = 1;
                                }
                                else if ("#" == dest[19])
                                {
                                    sampleData.Gjd = 9;
                                }
                                else
                                {
                                    sampleData.Gjd = 0;
                                }
                                #endregion

                                #region 雾灯
                                if ("1" == dest[20])
                                {
                                    sampleData.Wd = 1;
                                }
                                else if ("#" == dest[20])
                                {
                                    sampleData.Wd = 9;
                                }
                                else
                                {
                                    sampleData.Wd = 0;
                                }
                                #endregion

                                #region 倒车
                                if ("1" == dest[21])
                                {
                                    sampleData.Dc = 1;
                                }
                                else if ("#" == dest[21])
                                {
                                    sampleData.Dc = 9;
                                }
                                else
                                {
                                    sampleData.Dc = 0;
                                }
                                #endregion

                                #region 速度
                                if ("#" == dest[3])
                                {
                                    sampleData.Ycsd = -1;
                                }
                                else
                                {
                                    float fSd = 0;
                                    float.TryParse(dest[3], out fSd);
                                    sampleData.Ycsd = fSd;
                                }
                                #endregion

                                #region 转速
                                if ("#" == dest[2])
                                {
                                    sampleData.Zs = -1;
                                }
                                else
                                {
                                    int nZs = 0;
                                    int.TryParse(dest[2], out nZs);
                                    sampleData.Zs = nZs;
                                }
                                #endregion

                                #region 档位
                                if (BaseDefine.DATA_TYPE_DW_MN == dest[5] || BaseDefine.DATA_TYPE_DW_AN == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_N;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_M1 == dest[5] || BaseDefine.DATA_TYPE_DW_A1 == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_1;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_M2 == dest[5] || BaseDefine.DATA_TYPE_DW_A2 == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_2;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_M3 == dest[5] || BaseDefine.DATA_TYPE_DW_A3 == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_3;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_M4 == dest[5] || BaseDefine.DATA_TYPE_DW_A4 == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_4;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_M5 == dest[5] || BaseDefine.DATA_TYPE_DW_A5 == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_5;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_MR == dest[5] || BaseDefine.DATA_TYPE_DW_AR == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_R;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_AP == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_P;
                                }
                                else if (BaseDefine.DATA_TYPE_DW_AD == dest[5])
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_D;
                                }
                                else
                                {
                                    sampleData.Dw = (byte)DwDefine.DW_OTHER;
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else if (BaseDefine.DATA_TYPE_OBD_INFO == dest[0])
                    {
                        logger.InfoFormat("dest[0]={0}, continue", BaseDefine.DATA_TYPE_OBD_INFO);
                        continue;
                    }
                    else if (BaseDefine.DATA_TYPE_OBD_IO == dest[0])
                    {
                        if (signalData != null && dest.Length >= signalData.Length + 1)
                        {
                            tempError = string.Empty;
                            for (int j = 0; j < signalData.Length; j++)
                            {
                                string[] tempSz = BaseMethod.SplitString(signalData[j], '@', out tempError);
                                if (tempSz != null && tempSz.Length >= 2)
                                {
                                    #region 从协议取值
                                    lock (lockObj)
                                    {
                                        if ("1" == tempSz[1])
                                        {
                                            if ("1" == tempSz[0])
                                            {
                                                tempSz[0] = "0";
                                            }
                                            else
                                            {
                                                tempSz[0] = "1";
                                            }
                                        }

                                        byte byteTemp;
                                        byte.TryParse(dest[j + 1], out byteTemp);
                                        switch (tempSz[0])
                                        {
                                            case "1":   //安全带
                                                sampleData.Aqd = byteTemp;
                                                break;
                                            case "2":   //左转向灯
                                                sampleData.Zzxd = byteTemp;
                                                break;
                                            case "3":   //右转向灯
                                                sampleData.Yzxd = byteTemp;
                                                break;
                                            case "4":   //熄火
                                                sampleData.Yzxd = byteTemp;
                                                break;
                                            case "5":   //点火
                                                sampleData.Dh = byteTemp;
                                                break;
                                            case "6":   //手刹
                                                sampleData.Ss = byteTemp;
                                                break;
                                            case "7":   //右转向灯
                                                sampleData.Yzxd = byteTemp;
                                                break;
                                            case "8":   //档位？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？
                                                sampleData.Dw = byteTemp;
                                                break;
                                            case "9":   //近光灯
                                                sampleData.Jgd = byteTemp;
                                                break;
                                            case "10":   //远光灯
                                                sampleData.Ygd = byteTemp;
                                                break;
                                            case "11":   //脚刹
                                                sampleData.Js = byteTemp;
                                                break;
                                            case "12":   //倒车
                                                sampleData.Dc = byteTemp;
                                                break;
                                            case "13":   //雾灯
                                                sampleData.Wd = byteTemp;
                                                break;
                                            case "14":   //喇叭
                                                sampleData.Lb = byteTemp;
                                                break;
                                            case "15":   //油箱
                                                sampleData.Yx = byteTemp;
                                                break;
                                            case "16":   //副刹
                                                sampleData.Fs = byteTemp;
                                                break;
                                            case "17":   //离合
                                                sampleData.Lh = byteTemp;
                                                break;
                                            case "18":   //危险告警灯
                                                sampleData.Gjd = byteTemp;
                                                break;
                                            case "19":   //示廓灯
                                                sampleData.Skd = byteTemp;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(200);

            }
        }
    }
}
