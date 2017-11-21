using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BekUtils.Util;
using log4net;
using System.Threading;
using CommonBase.Model;
using CommonBase;
using System.Net.Sockets;

namespace SampleBase
{
    /// <summary>
    /// GPS 采样
    /// </summary>
    public class SampleGPS
    {
        private ILog logger = Log.GetLogger();
        private BekUtils.Communication.TCPUtil tcpClient = null;
        private Thread sampleThread = null;
        private GpsData gpsData = new GpsData();
        private Socket socket = null;
        private string ip = string.Empty;
        private int port = 0;
        private static readonly object lockObj = new object();

        //连接GPS服务端
        public void InitSocket(SocketParam sp)
        {
            ip = sp.Ip;
            port = sp.Port;

            tcpClient = new BekUtils.Communication.TCPUtil();
        }

        //开始采样
        public void StartSample()
        {
            sampleThread = new Thread(SampleThreadProc);
            sampleThread.Start();
        }

        //结束采样
        public void StopSample()
        {
            BekUtils.Util.BaseMethod.StopThread(sampleThread);
        }

        //获取采样数据
        public void RecvData(ref GpsData data)
        {
            try
            {
                lock (lockObj)
                {
                    data.Sd = gpsData.Sd;
                    data.Jd = gpsData.Jd;
                    data.Wd = gpsData.Wd;
                    data.Fxj = gpsData.Fxj;
                    data.Fyj = gpsData.Fyj;
                    data.Hgj = gpsData.Hgj;
                    data.Gd = gpsData.Gd;
                    data.Clqy = gpsData.Clqy;
                    data.Ctqy = gpsData.Ctqy;
                    data.Qjjl = gpsData.Qjjl;
                    data.Htjl = gpsData.Htjl;
                    data.Zt = gpsData.Zt;
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }
        }

        //GPS 采样线程函数
        private void SampleThreadProc()
        {
            while(true)
            {
                try
                {
                    if (null == socket || !socket.Connected)
                    {
                        socket = tcpClient.ConnectSocket(ip, port);
                        if (null == socket || !socket.Connected)
                        {
                            logger.ErrorFormat("GPS Thread ConnectSocket faild, retry after sleep");
                            Thread.Sleep(2500);
                            continue;
                        }
                    }

                    byte[] recvBuf = new byte[BaseDefine.COMMON_VALUE_1024];
                    if (!tcpClient.RecvData(socket, -1, ref recvBuf))
                    {
                        logger.ErrorFormat("GPS Thread RecvData faild, retry after sleep");
                        Thread.Sleep(2500);
                        continue;
                    }

                    string recvStr = Encoding.ASCII.GetString(recvBuf, 0, recvBuf.Length);
                    while(true)
                    {
                        #region 解析协议数据
                        int headbit = recvStr.IndexOf("$");
                        if (headbit < 0 || recvStr.Length <= 0)
                        {
                            logger.ErrorFormat("Socket receive data does not contain $, recvStr = {0}", recvStr);
                            break;
                        }
                        int tailbit = recvStr.IndexOf("*FF", headbit);
                        if (tailbit <= headbit)
                        {
                            logger.ErrorFormat("Socket receive data does not contain *FF, recvStr = {0}", recvStr);
                            break;
                        }

                        string strbuff = recvStr.Substring(headbit, tailbit - headbit);
                        recvStr = recvStr.Substring(tailbit, recvStr.Length - tailbit);
                        string[] revlist = strbuff.Split(',');

                        switch (revlist[0])
                        {
                            case "$SNDH2":
                                if (revlist.Length >= 14)
                                {
                                    float fFxj = 0;
                                    float.TryParse(revlist[4], out fFxj);
                                    float fFyj = 0;
                                    float.TryParse(revlist[5], out fFyj);
                                    float fHgj = 0;
                                    float.TryParse(revlist[6], out fHgj);
                                    double dJd = 0;
                                    double.TryParse(revlist[7], out dJd);
                                    double dWd = 0;
                                    double.TryParse(revlist[8], out dWd);
                                    double dGd = 0;
                                    double.TryParse(revlist[9], out dGd);
                                    float fSd = 0;
                                    float.TryParse(revlist[10], out fSd);
                                    double dQjjl = 0;
                                    double.TryParse(revlist[11], out dQjjl);
                                    double dHtjl = 0;
                                    double.TryParse(revlist[12], out dHtjl);

                                    lock (lockObj)
                                    {
                                        gpsData.Fxj = fFxj;
                                        gpsData.Fyj = fFyj;
                                        gpsData.Hgj = fHgj;
                                        gpsData.Jd = dJd;
                                        gpsData.Wd = dWd;
                                        gpsData.Gd = dGd;
                                        gpsData.Sd = fSd;
                                        gpsData.Qjjl = dQjjl;
                                        gpsData.Htjl = dHtjl;
                                    }
                                }
                                break;
                            case "$WWSN1":
                                if (revlist.Length >= 8)
                                {
                                    lock(lockObj)
                                    {
                                        gpsData.Ctqy = revlist[4];
                                        gpsData.Clqy = revlist[5];
                                        gpsData.Zt = revlist[7];
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }

                }
                catch(Exception e)
                {
                    logger.ErrorFormat("catch an error : {0}", e.Message);
                }
            }
        }
    }
}
