using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BekUtils;
using log4net;

namespace BKSmartCar3
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            // log4net 初始化
            log4net.Config.XmlConfigurator.Configure();


        }
    }
}
