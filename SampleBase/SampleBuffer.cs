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
    /// 采样数据存储区
    /// </summary>
    public class SampleBuffer
    {
        private static SampleBuffer pSampleBuffer = null;
        private Thread sampleThread = null;
        private static readonly object lockOBD= new object();
        private static readonly object lockGPS = new object();
        private ILog logger = Log.GetLogger();

        private int sampleMaxCount;     //采样缓存数据最大条目
        private List<GpsData> listGpsData = null;  //GPS信号缓存数据
        private List<SampleData> listSampleData = null;    //OBD信号缓存数据

        public static SampleBuffer CreateSampleBuffer()
        {
            if (null == pSampleBuffer)
            {
                pSampleBuffer = new SampleBuffer();

                //开始采样
                pSampleBuffer.StartSample();    
            }
            return pSampleBuffer;
        }

        public GpsData GetLastGpsData()
        {
            GpsData gpsData = null;

            try
            {
                lock(lockGPS)
                {
                    if (listGpsData.Count > 0)
                    {
                        gpsData = listGpsData[listGpsData.Count - 1];
                    }
                    else
                    {
                        logger.ErrorFormat("GetLastGpsData failed, listGpsData is null");
                    }
                }
            }
            catch(Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }

            return gpsData;
        }

        public List<GpsData> GetGpsDataList()
        {
            List<GpsData> retList = null;

            try
            {
                lock(lockGPS)
                {
                    retList = listGpsData;
                }
            }
            catch(Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }

            return retList;
        }

        public SampleData GetLastSampleData()
        {
            SampleData sampleData = new SampleData();

            try
            {
                lock (lockOBD)
                {
                    if (listSampleData.Count > 0)
                    {
                        sampleData = listSampleData[listSampleData.Count - 1];
                    }
                    else
                    {
                        logger.ErrorFormat("GetLastSampleData failed, listSampleData is null");
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }

            return sampleData;
        }

        public List<SampleData> GetSampleDataList()
        {
            List<SampleData> retList = null;

            try
            {
                lock (lockOBD)
                {
                    retList = listSampleData;
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }

            return retList;
        }

        private SampleBuffer()
        {
            sampleMaxCount = BaseDefine.COMMON_VALUE_SAMPLE_MAX_COUNT;

            listGpsData = new List<GpsData>();
            listSampleData = new List<SampleData>();
        }

        private void StartSample()
        {
            sampleThread = new Thread(SampleThreadProc);
            sampleThread.Start();
        }

        private void StopSample()
        {
            BaseMethod.StopThread(sampleThread);
        }

        private void SampleThreadProc()
        {
            // TO DO : 这里要结合业务代码，获取IP、端口、串口等参数
            string ip = string.Empty;
            int port = 0;
            string com = string.Empty;
            int rate = 0;
            string pwd = string.Empty;

            SampleGPS sampleGps = new SampleGPS();
            SocketParam socketParam = new SocketParam(ip, port);
            sampleGps.InitSocket(socketParam);
            sampleGps.StartSample();

            ComSampleBase sampleOBD = new SampleOBD();
            ComParam comParam = new ComParam(com, rate, pwd);
            sampleOBD.SetCom(comParam);
            sampleOBD.StartSample();

            while (true)
            {
                try
                {
                    GpsData gpsData = new GpsData();
                    sampleGps.RecvData(ref gpsData);
                    lock(lockGPS)
                    {
                        listGpsData.Add(gpsData);
                        if (listGpsData.Count > sampleMaxCount)
                        {
                            listGpsData.RemoveAt(0);
                        }
                    }

                    SampleData sampleData = new SampleData();
                    sampleOBD.RecvData(ref sampleData);
                    lock(lockOBD)
                    {
                        listSampleData.Add(sampleData);
                        if (listSampleData.Count > sampleMaxCount)
                        {
                            listSampleData.RemoveAt(0);
                        }
                    }
                }
                catch(Exception e)
                {
                    logger.ErrorFormat("SampleThreadProc catch an error and continue, {0}", e.Message);
                }

                Thread.Sleep(BaseDefine.COMMON_VALUE_SLEEP_200);
            }
        }
    }
}
