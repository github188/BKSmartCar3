using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;
using CommonBase.Model;

namespace SampleBase
{
    /// <summary>
    ///  串口采样基类
    /// </summary>
    public class ComSampleBase
    {
        public virtual void SetCom(ComParam param) { }

        public virtual void StartSample() { }

        public virtual void StopSample() { }

        public virtual void RecvData(ref SampleData sampleData) { }

        //public virtual void GetStatus(ref DeviceSingle DeviceSingle_T) { }
    }


}
