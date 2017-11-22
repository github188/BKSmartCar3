using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using BekUtils;
using CommonBase;
using SampleBase;

namespace JudgeBase
{
    /// <summary>
    /// 通用评判
    /// </summary>
    public class JudgeCommon : JudgeBase
    {
        private ILog logger = BekUtils.Util.Log.GetLogger();
        private static readonly object lockObj = new object();
        private Thread judgeThread = null;

        public void StartJudge()
        {
            judgeThread = new Thread(JudgeCommonThreadProc);
            judgeThread.Start();
        }

        public void StopJudge()
        {
            BekUtils.Util.BaseMethod.StopThread(judgeThread);
        }

        //通用评判线程函数
        private void JudgeCommonThreadProc()
        {

        }
    }
}
