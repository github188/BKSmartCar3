using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BekUtils.Util
{
    public class BaseMethod
    {
        //字符串分割
        public static string[] SplitString(string data, char cOperator, out string errorMsg)
        {
            errorMsg = string.Empty;
            string[] retArray = null;

            if (string.IsNullOrEmpty(data))
            {
                errorMsg = string.Format("data参数为空");
                return null;
            }

            try
            {
                retArray = data.Split(cOperator);
            }
            catch (Exception e)
            {
                errorMsg = string.Format("分割字符串时产生异常：{0}", e.Message);
            }

            if (null == retArray || 0 == retArray.Length)
            {
                errorMsg = string.Format("Split方法返回为空");
                return null;
            }

            return retArray;
        }

        //结束线程
        public static void StopThread(Thread t)
        {
            try
            {
                t.Join(1000);

                if (t.IsAlive)
                {
                    t.Abort();
                }
            }
            catch
            {
            }
        }
    }
}
