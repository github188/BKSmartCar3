using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using BekUtils.Util;
using log4net;

namespace BekUtils.Util.Com
{
    public class ComUtil
    {
        private SerialPort serialPort;
        private ILog logger = Log.GetLogger();

        public ComUtil(string portName, int rate, int dataBits = 8)
        {
            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = rate;
            serialPort.DataBits = dataBits; //数据位
            serialPort.StopBits = System.IO.Ports.StopBits.One; //停止位
            serialPort.Parity = System.IO.Ports.Parity.None;    //无奇偶校验位

            serialPort.ReadTimeout = 100;
            serialPort.WriteTimeout = -1;
        }

        public int ConnectSerialPort()
        {
            try
            {
                CloseSerialPort();
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    return 0;
                }
                else
                {
                    logger.ErrorFormat("ConnectSerialPort failed");
                    return -1;
                }
            }
            catch(Exception e)
            {
                logger.ErrorFormat("ConnectSerialPort catch an error : {0}", e.Message);
                return -2;
            }
        }

        public void CloseSerialPort()
        {
            try
            {
                serialPort.Close();
            }
            catch(Exception e)
            {
                logger.ErrorFormat("CloseSerialPort catch an error : {0}", e.Message);
            }
        }

        public int ReadSerialPort(ref string buff)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    int count = serialPort.BytesToRead;
                    if (count > 0)
                    {
                        byte[] readBuffer = new byte[count];
                        try
                        {
                            serialPort.Read(readBuffer, 0, count);
                            buff = System.Text.Encoding.Default.GetString(readBuffer);
                        }
                        catch (Exception e)
                        {
                            logger.ErrorFormat("serialPort.Read catch an error : {0}", e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("ReadSerialPort catch an error : {0}", e.Message);
            }
            return buff.Length;
        }

        public void WriteSerialPort(string buff)
        {
            if (string.IsNullOrEmpty(buff))
            {
                return;
            }

            try
            {
                if (IsOpen)
                {
                    byte[] send = System.Text.Encoding.Default.GetBytes(buff);
                    serialPort.Write(send, 0, send.Length);
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("WriteSerialPort catch an error : {0}", e.Message);
            }
        }

        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }


    }
}
