using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BekUtils.Util;
using log4net;
using System.Net.Sockets;
using System.Net;

namespace BekUtils.Communication
{
    public class TCPUtil
    {
        private ILog logger = Log.GetLogger();

        public Socket ConnectSocket(string ip, int port)
        {
            if (string.IsNullOrEmpty(ip) || port <= 0)
            {
                logger.ErrorFormat("param error, ip ={0}, port={1}", ip, port);
                return null;
            }

            Socket socket = null;

            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
                socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipEndPoint);
                if (!socket.Connected)
                {
                    logger.ErrorFormat("ConnectSocket failed, ip = {0}, port = {1}", ip, port);
                    socket = null;
                }
            }
            catch(Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
                return null;
            }

            return socket;
        }

        /// <summary>
        /// 接收远程主机发送的数据
        /// </summary>
        /// <param name="socket">要接收数据且已经连接到远程主机的 socket</param>
        /// <param name="buffer">接收数据的缓冲区</param>
        /// <param name="timeout">接收数据的超时时间，以秒为单位，可以精确到微秒</param>
        /// <returns></returns>
        /// <remarks >
        /// 1、当 timeout 指定为-1时，将一直等待直到有数据需要接收；
        /// 2、需要接收的数据的长度，由 buffer 的长度决定。
        /// </remarks>
        public bool RecvData(Socket socket, int timeout, ref byte[] buffer)
        {
            bool bRet = false;

            if (null == socket || !socket.Connected)
            {
                logger.ErrorFormat("param error");
                return bRet;
            }

            buffer.Initialize();
            int left = buffer.Length;
            int curRcv = 0;

            try
            {
                while (true)
                {
                    if (socket.Poll(timeout * 1000000, SelectMode.SelectRead) == true)
                    {        
                        // 已经有数据等待接收
                        curRcv = socket.Receive(buffer, curRcv, left, SocketFlags.None);
                        left -= curRcv;
                        if (left == 0)  // 数据已经全部接收 
                        {
                            bRet = true;
                            break;
                        }
                        else
                        {
                            if (curRcv > 0) // 数据已经部分接收
                            {                                
                                continue;
                            }
                            else
                            {                                            
                                bRet = false;   //出现错误
                                logger.ErrorFormat("RecvData error, break");
                                break;
                            }
                        }
                    }
                    else
                    {
                        bRet = false;
                        logger.ErrorFormat("RecvData timeout, break");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("catch an error : {0}", e.Message);
                bRet = false;
            }

            return bRet;
        }

        /// <summary>
        /// 向远程主机发送数据
        /// </summary>
        /// <param name="socket">要发送数据且已经连接到远程主机的 Socket</param>
        /// <param name="timeout">发送数据的超时时间，以秒为单位，可以精确到微秒</param>
        /// <param name="buffer">待发送的数据</param>
        /// <returns></returns>
        /// <remarks >
        /// 当 outTime 指定为-1时，将一直等待直到有数据需要发送
        /// </remarks>
        public bool SendData(Socket socket, int timeout, byte[] buffer)
        {
            bool bRet = false;
            if (null == socket || !socket.Connected || null == buffer || 0 == buffer.Length)
            {
                logger.ErrorFormat("param error");
                return bRet;
            }

            try
            {
                int left = buffer.Length;
                int sndLen = 0;

                while (true)
                {
                    if ((socket.Poll(timeout * 1000000, SelectMode.SelectWrite) == true))
                    {        
                        // 收集了足够多的传出数据后开始发送
                        sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                        left -= sndLen;
                        if (left == 0)
                        {                                        
                            // 数据已经全部发送
                            bRet = true;
                            break;
                        }
                        else
                        {
                            if (sndLen > 0)
                            {                                    
                                // 数据部分已经被发送
                                continue;
                            }
                            else
                            {
                                // 发送数据发生错误
                                bRet = false;
                                logger.ErrorFormat("SendData error, break");
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 超时退出
                        bRet = false;
                        logger.ErrorFormat("SendData timeout, break");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                bRet = false;
                logger.ErrorFormat("catch an error : {0}", e.Message);
            }

            return bRet;
        }
    }
}
