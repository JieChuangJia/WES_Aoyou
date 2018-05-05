using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CtlDBAccess.BLL;
using CtlDBAccess.Model;
namespace NbssECAMS
{
    public partial class LoginView2 : Form
    {
        private readonly User_ListBll bllUser = new User_ListBll();
        public LoginView2()
        {
            InitializeComponent();
        }

        private void LoginView2_Load(object sender, EventArgs e)
        {
            this.cb_UserRole.Items.Clear();
            List<User_ListModel> userList= bllUser.GetModelList("");
            foreach(User_ListModel m in userList)
            {
                this.cb_UserRole.Items.Add(m.UserName);
            }
         //   this.cb_UserRole.Items.AddRange(new string[] {"操作员","管理员","系统维护"});
            this.cb_UserRole.SelectedIndex = 0;
        }
        public int GetLoginRole(ref string userName)
        {

            userName = this.cb_UserRole.Text;
            User_ListModel userModel = bllUser.GetModel(this.cb_UserRole.Text);
            if(userModel == null)
            {
                MessageBox.Show("用户不存在");
                return -1;
            }
            if(userModel.RoleID == 3)
            {
                return userModel.RoleID;
            }
            if(userModel.UserPassWord != this.tb_userPassword.Text)
            {
                MessageBox.Show("密码错误");
                return -2;
            }
            return userModel.RoleID ;
        }

        private void bt_login_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
