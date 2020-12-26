using AutoMapper;
using Firebase.Storage;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using GATE_GUARD2.Common;
using GATE_GUARD2.Dao;
using GATE_GUARD2.Db;
using GATE_GUARD2.Models;
using shortid;
using shortid.Configuration;
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

namespace GATE_GUARD2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            config = new FirebaseConfig
            {
                AuthSecret = "60NwcUMNV5rvqZSD6adULFT2tkfJgRT4cbw4Q0hs",
                BasePath = "https://dtuparking.firebaseio.com/"
            };
            cl = new FirebaseClient(config);

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
        
            dataGridView2.DataSource = uD.getListParking();

            pnOK.Visible = false;
            tsBar.Maximum = 100;
            listen();
        }
        public Form1(Guard g, IFirebaseClient cl,Login login)
        {
            InitializeComponent();
            
            this.cl = cl;
            this.g = g;
            this.loginForm = login;
            var configMap = new MapperConfiguration(cfg => cfg.CreateMap<AcceptUserTemp, AcceptUser>());
            mapper= configMap.CreateMapper();

            lbCode.Text ="Code: "+g.Id;
            pbLoadingOut.Visible = false;
            pbLoadingIn.Visible = false;
            pbLoadPlate.Visible = false;
            pbloadAvatar.Visible = false;
            pbLoadingOut.Image = Image.FromFile(Application.StartupPath + @"\img\icon\loading.gif");
            pbLoadingIn.Image = Image.FromFile(Application.StartupPath + @"\img\icon\loading.gif");
            pbloadAvatar.Image = Image.FromFile(Application.StartupPath + @"\img\icon\loading.gif");
            pbLoadPlate.Image = Image.FromFile(Application.StartupPath + @"\img\icon\loading.gif");
            pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\img\logo\logo.png");
            pictureBox2.Image = Image.FromFile(Application.StartupPath + @"\img\logo\logo.png");
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            lUOK = uD.getListParking();
            dataGridView2.DataSource = uD.getListParking();

            string loca = "";
            switch (g.Place)
            {
                case 0: loca = "Quang Trung"; BLOCK = 0;  break;
                case 1: loca = "Hoa Khanh"; BLOCK = 1; break;
                case 2: loca = "254 Nguyen Van Linh"; BLOCK = 2; break;
                case 3: loca = "334 Nguyen Van Linh"; BLOCK = 3; break;
            }
            tsL.Text = loca;
            listen();
        }

        Login loginForm;
        Guard g;
        IFirebaseConfig config;
        IFirebaseClient cl;
        EventStreamResponse x;
        EventStreamResponse xErr;
        int BLOCK = 1;
        List<AcceptUser> lAU = new List<AcceptUser>();
        List<UserList> lUOK = new List<UserList>();
        UserDao uD = new UserDao();
        const float moneyPay = 1000;
        IMapper mapper;

        private async void listen()
        {
            x = await cl.OnAsync("APIParking/Parking/IdList/" + BLOCK, added: (s, args, d) =>
            {
                Console.WriteLine(args.Data);
                FirebaseResponse userFalse = cl.Get("APIParking/Parking/InfoList/" + BLOCK + "/" + args.Data);
                AcceptUserTemp rs = userFalse.ResultAs<AcceptUserTemp>();
                AcceptUser acU = mapper.Map<AcceptUserTemp,AcceptUser>(rs);
                //Nếu đó là lúc vào và lỗi 
                if (acU.lineOutIn == 1)
                {
                    if (acU.status == 0)
                    {
                        Thread t = new Thread(()=> {
                            loop:
                            Console.WriteLine("Loop image");
                            acU.imageInPlate = getDownloadImage(acU.id, true, true);
                            if (acU.imageInPlate == null) goto loop;

                                lAU.Add(acU);
                                updateGrid();
                                
                        });
                            t.Start();
                             
                    }
                    else
                    {
                        Console.WriteLine("Vo dayyyyy");
                       if(uD.addNewUser(acU)) updateGrid2();
                    }
                }


            }, changed: (s, args, d) =>
            {
                Console.WriteLine("CHANGED");
                Console.WriteLine(args.Data);
            }, removed: (s, args, d) =>
            {
                Console.WriteLine("REMOVE");
                //bởi vì id trả về có dạng "/idrootsv1"
                string idRm = args.Path.Substring(1);
                if (uD.removeParking(idRm))
                {
                    updateGrid2();
                    Console.WriteLine("Da xoa");
                }
                else
                {
                    Console.WriteLine("Chưa đỗ");
                }
            });


            xErr = await cl.OnAsync("APIParking/Parking/IdListErrOut/" + BLOCK, added: (s, args, d) =>
            {
                Console.WriteLine("OKOK IdListErrOut");
                //if (lAU.Find(x => x.id == args.Data)!=null) return;

                FirebaseResponse resCheck = cl.Get("APIParking/Parking/InfoList/" + BLOCK + "/" + args.Data);
                AcceptUserTemp rs = resCheck.ResultAs<AcceptUserTemp>();
                AcceptUser user = mapper.Map<AcceptUserTemp, AcceptUser>(rs);

                Console.WriteLine(args.Data);
                Console.WriteLine("Is in OK ? "+user.isInOK);
                lAU.RemoveAll(x=>x.id.Equals(user.id));

                

                Thread t = new Thread(() => {
                    bool isNotNull = true;
                    while (isNotNull)
                    {
                        Console.WriteLine("Loop image");
                        if (user.isInOK == false) user.imageInPlate = getDownloadImage(user.id, true, true);
                        else user.imageInPlate = getDownloadImage(user.id, false, true);


                        user.imageOutPlate = getDownloadImage(user.id, true, false);
                        if (user.imageInPlate != null && user.imageOutPlate != null) isNotNull = false;
                    }
                    
                    lAU.Add(user);
                    updateGrid();
                });
                

                t.Start();
                switch (user.codeErr)
                {
                    case 0: Console.WriteLine("Sai biển số"); break;
                    case 1: Console.WriteLine("Thiếu tiền"); break;
                    case 3: Console.WriteLine("Không thể add hisory"); break;
                    default: Console.WriteLine("Cái số 2"); break;
                }
            });
        }

        
        void updateGrid()
        {
            dataGridView1.Invoke(new Action(() => {
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                dataGridView1.DataSource = lAU;

            }));
        }
        void updateGrid2()
        {
            lUOK = uD.getListParking();
            dataGridView2.Invoke(new Action(() => {
                dataGridView2.DataSource = null;
                dataGridView2.Refresh();
                dataGridView2.DataSource = lUOK;

            }));
        }

        void updateParking(AcceptUser ac)
        {
            cl.Update("APIParking/Parking/InfoList/"+ac.block+"/"+ac.id, ac);
        }

        public void updateGridRemove(int index)
        {
            lAU.RemoveAll(r => r.id == lAU[index].id);
            dataGridView1.Invoke(new Action(() => {
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                dataGridView1.DataSource = lAU;
            }));
        }
        public bool removeIdErrList(AcceptUser ac)
        {
            FirebaseResponse res = cl.Delete(@"APIParking/Parking/IdListErrOut/" + BLOCK + "/" + ac.id);
            if (res.Body.Equals("null")) return false; // Loi dlt
            return true;
        }
        public bool removeUserIn(AcceptUser ac)
        {
            FirebaseResponse res = cl.Delete(@"APIParking/Parking/IdList/" + ac.block + "/" + ac.id);
            FirebaseResponse resId = cl.Delete(@"APIParking/Parking/InfoList/" + ac.block + "/" + ac.id);
            if (res.Body.Equals("null")) return false; // Loi dlt
            if (resId.Body.Equals("null")) return false; // Loi dlt
            return true;
        }
        public bool isOutOfMoney(string id)
        {
            FirebaseResponse res = cl.Get(@"User/information/parkingMan/" + id + "/money");
            float money = res.ResultAs<float>();
            return money < 1000;
        }
        public bool addHistory(AcceptUser ac,float money)
        {
            var options = new GenerationOptions
            {
                UseNumbers = true,
                Length = 9,
                UseSpecialCharacters = false
            };
            //Lấy ngày gửi về
            FirebaseResponse res = cl.Get(@"APIParking/Parking/InfoList/" + ac.block + "/" + ac.id + "/dateSend");
            if (res.Body.Equals("null")) return false;
            string dateS = res.ResultAs<string>();

            HistoryDTO his = new HistoryDTO
            {
                idPay = ShortId.Generate(options),
                dateSend = dateS,
                method = 0,
                place = ac.block,
                plateLicense = ac.txtPlate,
                payMoney = money+""
            };
            cl.Push(@"History/parkingMan/moneyOut/" + ac.id, his);
            return true;
        }
        public bool payMoney(string id, double payMoney)
        {
            int position = getPositionUser(id);
            //Nếu là không phải sinh viên thì k add, trả về 1 OK
            if (position != 3) return true;
            double money = getMoney(id);
            if (money < payMoney) return false;
            money -= payMoney;
            FirebaseResponse res = cl.Set(@"User/information/parkingMan/" + id + "/money", money+"");
            if (res.Body.Equals("null")) return false; // Loi thanh toan
            return true; //OK
        }
        public int getPositionUser(string id)
        {
            FirebaseResponse res = cl.Get(@"User/information/parkingMan/" + id + "/position");
            if (res.Body.Equals("null")) return -1;
            return res.ResultAs<int>();
        }
        public double getMoney(string id)
        {
            FirebaseResponse res = cl.Get(@"User/information/parkingMan/" + id + "/money");
            if (res.Body.Equals("null")) return -1;
            return res.ResultAs<double>();
        }
        public void acceptIn(AcceptUser u,int index, bool isOk)
        {
            Thread t = new Thread(()=>
            {
                if (isOk)
                {
                    u.status = 2;
                    u.codeErr = 2;
                    u.txtPlate = "NOD";
                    updateParking(u);
                    uD.addNewUser(u);
                    updateGrid2();
                }
                else
                {
                    u.status = 1;
                    u.codeErr = 2;
                    u.txtPlate = "NOD";
                    updateParking(u);
                    removeUserIn(u);
                }
            });
            updateGridRemove(index);
            t.Start();
        }
        public void acceptOut(AcceptUser u, int index, bool isOk) {
            Thread t = new Thread(() => {
                if (isOk)
                {
                    u.status = 2;
                    u.codeErr = 2;
                    u.txtPlate = "NOD";
                    //Đưa xác nhận sang cổng
                    updateParking(u);
                    //Thanh toán tiền
                    payMoney(u.id, moneyPay);
                    removeIdErrList(u);
                    //Add giao dịch
                    addHistory(u, u.position==3 ? moneyPay : 0);
                    //Xóa info trên APIParing
                    removeUserIn(u);
                }
                else
                {
                    UserList ul= lUOK.Find(x =>x.ID.Equals(u.id));
                    u.status = 1;
                    u.codeErr = 2;
                    u.txtPlate = ul.TxtPlate;
                    updateParking(u);
                    removeIdErrList(u);
                    u.status = 2;
                    updateParking(u);
                }
            });
            t.Start();
            updateGridRemove(index);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            pnOK.Visible = false;
            pnErr.Visible = true;
            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("Accept"))
            {
                int index = int.Parse(e.RowIndex.ToString());
                bool isOK;
                AcceptUser ac = lAU[index];
                if (MessageBox.Show("Accept ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    isOK = true;

                    if (ac.lineOutIn == 1) acceptIn(ac, index, isOK);
                    else acceptOut(ac, index, isOK);
                }
                else
                {
                    isOK = false;
                    if (ac.lineOutIn == 1) acceptIn(ac, index, isOK);
                    else acceptOut(ac, index, isOK);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            Application.Exit();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pnOK.Visible = false;
            pnErr.Visible = true;

            pbLoadingIn.Visible = true;
            pbLoadingOut.Visible = true;
            Console.WriteLine(lAU[e.RowIndex].id);
            lbTextLine.Text = lAU[e.RowIndex].line + "";
            lbTextGate.Text = lAU[e.RowIndex].lineOutIn==1 ? "Line In":"Line Out";

            string id = lAU[e.RowIndex].id;
            string inOut = "";
            if (lAU[e.RowIndex].lineOutIn == 1)
            {
                //await setDownloadImage(pictureBox1,id,true,true);
                pictureBox1.Image = lAU[e.RowIndex].imageInPlate;
                pbLoadingIn.Visible = false;
                pbLoadingOut.Visible = false;
            }
            else
            {
                pictureBox1.Image = lAU[e.RowIndex].imageInPlate;
                pictureBox2.Image = lAU[e.RowIndex].imageOutPlate;
                pbLoadingIn.Visible = false;
                pbLoadingOut.Visible = false;
                //await setDownloadImage(pictureBox2, id,true, false);
            }
            
        }
        
        private async void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tsBar.Value = 10;
            pnOK.Visible = true;
            pnErr.Visible = false;
            //UserList u = lUOK[e.RowIndex];
            //string id = u.ID;
            //Console.WriteLine(id);
            pbloadAvatar.Visible = true;
            pbLoadPlate.Visible = true;
           
            getDatatoInfoAsync(e.RowIndex);
            tsBar.Value = 100;
        }

        Image getDownloadImage(string id, bool isErr, bool isIn)
        {

            string errOrOK = isErr ? "imgErr" : "imgOK";
            string inOut = isIn ? "in" : "out";
            Console.WriteLine("Plate "+ errOrOK+"  "+inOut);
            Task<string> task2;
            try
            {
                task2 = new FirebaseStorage("dtuparking.appspot.com").Child("Plate").Child(errOrOK).Child(id + "-" + inOut + ".jpg").GetDownloadUrlAsync();
            }
            catch (Exception e)
            {
                return null;
            }

            try
            {
                Console.WriteLine("OKOKOKOKO   " + task2.Result);
            }
            catch (Exception e)
            {
                return null;
            }

            return GetImgUrl.DownloadImage(task2.Result);
        }
        
         Image getAvatarImage(string id)
        {
            
            FirebaseResponse res = cl.Get("User/information/parkingMan/"+id+ "/avatar");
            if (res.Body.Equals("null")) return null;
            string task2 = res.ResultAs<string>();
            Console.WriteLine("image avatar: "+task2);
            Image img = GetImgUrl.DownloadImage(task2);
            if (img != null) return img;
            return null;


        }
        void getDatatoInfoAsync(int index)
        {
           
            UserList u = lUOK[index];
            lbID.Text = u.IDS;
            lbName.Text = u.Name;
            lbDataSend.Text = u.DateIn.ToString("dd/MM/yyyy");
            lbHour.Text = u.DateIn.ToString("HH:mm:ss");

            switch (u.Position)
            {
                case 2: lbPosition.Text = "Lecturers"; break;
                case 3: lbPosition.Text = "Student"; break;
                case 5: lbPosition.Text = "Visitor"; break;
                default:
                    lbPosition.Text = "none"; break;
            }
           
            lbPlate.Text = u.TxtPlate + "";
            Image plate=null;
            Image ava=null;
            Thread avaTh = new Thread(()=> {
                ava = getAvatarImage(u.ID);
                pbAvatar.Image = ava;

                pbloadAvatar.Invoke(new Action(() => {
                    
                    pbloadAvatar.Visible = false;
                }));
               
            });

            
            Thread t = new Thread(() => {

                int timesLoop = 0;
                loop3:
                Console.WriteLine("Loop image");
                if (timesLoop == 6) { MessageBox.Show("Get image fail"); tsBar.Value = 0; return; }

               
                Console.WriteLine(u.IsInOk);
                if(u.IsInOk) plate= getDownloadImage(u.ID, false, true); 
                else plate = getDownloadImage(u.ID, true, true);
                timesLoop++;
                if (plate == null) goto loop3;
                pbPlate.Image = plate;

                pbLoadPlate.Invoke(new Action(() => {
                    
                    pbLoadPlate.Visible = false;
                }));
            });
            avaTh.Start();
            t.Start();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginForm.Show();
            this.Dispose();
        }
    }
}
