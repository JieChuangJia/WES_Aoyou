using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using ModuleCrossPnP;
using LogInterface;
//using CtlDBAccess.BLL;
using MesDBAccess.Model;
using MesDBAccess.BLL;
namespace ConfigManage
{
    public partial class SysSettingView : BaseChildView
    {
       
        #region  公有接口
       // public string CaptionText { get { return captionText; } set { captionText = value; this.Text = captionText; } }
        public SysSettingView(string captionText):base(captionText)
        {
            InitializeComponent();
            //sysCfg = new SysCfgsettingModel();
            this.Text = captionText;
            //this.captionText = captionText;
        }
        public void SetCfgNodes(List<FlowCtlBaseModel.CtlNodeBaseModel> cfgNodes)
        {
            
        }
        public override void ChangeRoleID(int roleID)
        {
           
            
        }
        #endregion

        private void buttonCfgApply_Click(object sender, EventArgs e)
        {
           
            MessageBox.Show("设置已保存！");
            
        }

        private void buttonCancelSet_Click(object sender, EventArgs e)
        {
            
        }

        private void SysSettingView_Load(object sender, EventArgs e)
        {
            //this.checkBoxHouseA.Checked = SysCfg.SysCfgModel.HouseEnabledA;
            //this.checkBoxHouseB.Checked = SysCfg.SysCfgModel.HouseEnabledB;
         //   this.textBoxA1BurninTime.Text = SysCfg.SysCfgModel.AsrsStoreTime.ToString();
        //    this.checkBoxUnbind.Checked = SysCfg.SysCfgModel.UnbindMode;
            OnRefreshCfg();

        }
        private void OnRefreshCfg()
        {
            LishenMesDBAccess.BLL.MatProcessBll matCfgBll = new LishenMesDBAccess.BLL.MatProcessBll();
            LishenMesDBAccess.Model.MatProcessModel matCfgModel= matCfgBll.GetModel("1号车间");
           // this.radioButtonZ11.Checked = true;
            this.radioButtonZ11.Checked=(matCfgModel.ZhengjiHongkao == 0 ? true:false);
            this.radioButtonZ12.Checked = (matCfgModel.ZhengjiHongkao == 1 ? true : false);
            this.radioButtonZ13.Checked = (matCfgModel.ZhengjiHongkao == 2 ? true : false);
            this.radioButtonF11.Checked = (matCfgModel.FujiHongkao == 0 ? true : false);
            this.radioButtonF12.Checked = (matCfgModel.FujiHongkao == 1 ? true : false);
            this.radioButtonF13.Checked = (matCfgModel.FujiHongkao == 2 ? true : false);
            this.radioButtonG11.Checked = (matCfgModel.GemoHongkao == 0 ? true : false);
            this.radioButtonG12.Checked = (matCfgModel.GemoHongkao == 1 ? true : false);
            this.radioButtonG13.Checked = (matCfgModel.GemoHongkao == 2 ? true : false);

            matCfgModel = matCfgBll.GetModel("2号车间");
            this.radioButtonZ21.Checked = (matCfgModel.ZhengjiHongkao == 0 ? true : false);
            this.radioButtonZ22.Checked = (matCfgModel.ZhengjiHongkao == 1 ? true : false);
            this.radioButtonZ23.Checked = (matCfgModel.ZhengjiHongkao == 2 ? true : false);
            this.radioButtonF21.Checked = (matCfgModel.FujiHongkao == 0 ? true : false);
            this.radioButtonF22.Checked = (matCfgModel.FujiHongkao == 1 ? true : false);
            this.radioButtonF23.Checked = (matCfgModel.FujiHongkao == 2 ? true : false);
            this.radioButtonG21.Checked = (matCfgModel.GemoHongkao == 0 ? true : false);
            this.radioButtonG22.Checked = (matCfgModel.GemoHongkao == 1 ? true : false);
            this.radioButtonG23.Checked = (matCfgModel.GemoHongkao == 2 ? true : false);
            matCfgModel = matCfgBll.GetModel("3号车间");
            this.radioButtonZ31.Checked = (matCfgModel.ZhengjiHongkao == 0 ? true : false);
            this.radioButtonZ32.Checked = (matCfgModel.ZhengjiHongkao == 1 ? true : false);
            this.radioButtonZ33.Checked = (matCfgModel.ZhengjiHongkao == 2 ? true : false);
            this.radioButtonF31.Checked = (matCfgModel.FujiHongkao == 0 ? true : false);
            this.radioButtonF32.Checked = (matCfgModel.FujiHongkao == 1 ? true : false);
            this.radioButtonF33.Checked = (matCfgModel.FujiHongkao == 2 ? true : false);
            this.radioButtonG31.Checked = (matCfgModel.GemoHongkao == 0 ? true : false);
            this.radioButtonG32.Checked = (matCfgModel.GemoHongkao == 1 ? true : false);
            this.radioButtonG33.Checked = (matCfgModel.GemoHongkao == 2 ? true : false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OnRefreshCfg();
        }
        private void OnModifyCfg()
        {
            LishenMesDBAccess.BLL.MatProcessBll matCfgBll = new LishenMesDBAccess.BLL.MatProcessBll();
            LishenMesDBAccess.Model.MatProcessModel matCfgModel = matCfgBll.GetModel("1号车间");
            if(this.radioButtonZ11.Checked)
            {
                matCfgModel.ZhengjiHongkao=0;
            }
            else if(this.radioButtonZ12.Checked)
            {
                matCfgModel.ZhengjiHongkao = 1;
            }
            else
            {
                matCfgModel.ZhengjiHongkao = 2;
            }
            if (this.radioButtonF11.Checked)
            {
                matCfgModel.FujiHongkao = 0;
            }
            else if (this.radioButtonF12.Checked)
            {
                matCfgModel.FujiHongkao = 1;
            }
            else
            {
                matCfgModel.FujiHongkao = 2;
            }
            if (this.radioButtonG11.Checked)
            {
                matCfgModel.GemoHongkao = 0;
            }
            else if (this.radioButtonG12.Checked)
            {
                matCfgModel.GemoHongkao = 1;
            }
            else
            {
                matCfgModel.GemoHongkao = 2;
            }
            matCfgBll.Update(matCfgModel);

            matCfgModel = matCfgBll.GetModel("2号车间");
            if (this.radioButtonZ21.Checked)
            {
                matCfgModel.ZhengjiHongkao = 0;
            }
            else if (this.radioButtonZ22.Checked)
            {
                matCfgModel.ZhengjiHongkao = 1;
            }
            else
            {
                matCfgModel.ZhengjiHongkao = 2;
            }
            if (this.radioButtonF21.Checked)
            {
                matCfgModel.FujiHongkao = 0;
            }
            else if (this.radioButtonF22.Checked)
            {
                matCfgModel.FujiHongkao = 1;
            }
            else
            {
                matCfgModel.FujiHongkao = 2;
            }
            if (this.radioButtonG21.Checked)
            {
                matCfgModel.GemoHongkao = 0;
            }
            else if (this.radioButtonG22.Checked)
            {
                matCfgModel.GemoHongkao = 1;
            }
            else
            {
                matCfgModel.GemoHongkao = 2;
            }
            matCfgBll.Update(matCfgModel);


            matCfgModel = matCfgBll.GetModel("3号车间");
            if (this.radioButtonZ31.Checked)
            {
                matCfgModel.ZhengjiHongkao = 0;
            }
            else if (this.radioButtonZ32.Checked)
            {
                matCfgModel.ZhengjiHongkao = 1;
            }
            else
            {
                matCfgModel.ZhengjiHongkao = 2;
            }
            if (this.radioButtonF31.Checked)
            {
                matCfgModel.FujiHongkao = 0;
            }
            else if (this.radioButtonF32.Checked)
            {
                matCfgModel.FujiHongkao = 1;
            }
            else
            {
                matCfgModel.FujiHongkao = 2;
            }
            if (this.radioButtonG31.Checked)
            {
                matCfgModel.GemoHongkao = 0;
            }
            else if (this.radioButtonG32.Checked)
            {
                matCfgModel.GemoHongkao = 1;
            }
            else
            {
                matCfgModel.GemoHongkao = 2;
            }
            matCfgBll.Update(matCfgModel);
            MessageBox.Show("设置已保存！");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OnModifyCfg();
        }

    }
}
