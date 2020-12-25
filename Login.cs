using GATE_GUARD2.Dao;
using GATE_GUARD2.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace GATE_GUARD2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
           pbLogo.Image = Image.FromFile(Application.StartupPath + @"\img\logo\logo.png");
           pbLoading.Image= Image.FromFile(Application.StartupPath + @"\img\icon\loading.gif");
            pbLoading.Visible = false;
            config = new FirebaseConfig
            {
                AuthSecret = "60NwcUMNV5rvqZSD6adULFT2tkfJgRT4cbw4Q0hs",
                BasePath = "https://dtuparking.firebaseio.com/"
            };
            cl = new FirebaseClient(config);
            BLOCK= comboBox1.SelectedIndex;
        }
        IFirebaseConfig config;
        IFirebaseClient cl;
        int BLOCK=0;
        Thread t;
        bool isLoading = true;
        private async void button1_Click(object sender, EventArgs e)
        {
                if (!checkBlank()) return;
           
                pbLoading.Visible =true;
                GuardDao gd = new GuardDao(this.cl);
                JObject result = await gd.Login(txtUserName.Text.Trim(), txtPwd.Text.Trim());
                if (result == null)
                {
                        lbWarning.Text = "Username or password invalid";
                        lbWarning.Visible = true;
                        pbLoading.Visible = false;
                    return;
                }
                if (cbRememberMe.Checked) rememberUser(true);
                else rememberUser(false);
                Console.WriteLine(result.ToString());
               Console.WriteLine("OKKKKKKKKKK");
            //lg = new Form1(new Guard
            //{
            //    Id = result["idGuardB"].ToString(),
            //    Place = result["place"].ToString(),
            //    Positon = result["position"].ToString()
            //});
            //Console.WriteLine("OK XUONG DAY ROI");

            Guard g = new Guard
            {
                Id = result["idGuardB"].ToString(),
                Place = int.Parse(result["place"].ToString()),
                Positon = result["position"].ToString()
            };

            pbLoading.Visible = false;
            if (g.Place != BLOCK) {
                lbWarning.Visible = true;
                lbWarning.Text = "The account not valid in this place";
                return; 
            }
            Form1 lg = new Form1(g, this.cl,this);

            lbWarning.Visible = false;
            lg.Show();
            this.Hide();
        }
        private bool checkBlank()
        {
            if (string.IsNullOrEmpty(txtUserName.Text)) { MessageBox.Show("Please input user name"); return false; }
            if (string.IsNullOrEmpty(txtPwd.Text)) { MessageBox.Show("Please input password"); return false; }
            return true;
        }
        private void rememberUser(bool check)
        {
            if (check)
            {
                Properties.Settings.Default.UserName = txtUserName.Text;
                Properties.Settings.Default.Password = txtPwd.Text;
                Properties.Settings.Default.Remember = "true";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Remember = "false";
                Properties.Settings.Default.Save();
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 1;
            if (Properties.Settings.Default.Remember == "true")
            {
                txtUserName.Text = Properties.Settings.Default.UserName;
                txtPwd.Text = Properties.Settings.Default.Password;
                cbRememberMe.Checked = true;
            }
        }

        private void pbLogo_Click(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null) t.Abort();
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BLOCK = comboBox1.SelectedIndex;
        }
    }

}
