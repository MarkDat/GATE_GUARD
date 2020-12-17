using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
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
         
            listen();
        }
        IFirebaseConfig config;
        IFirebaseClient cl;
        EventStreamResponse x;
        EventStreamResponse xErr;
        const int BLOCK = 1;
        List<AcceptUser> lAU = new List<AcceptUser>();
        List<UserList> lUOK = new List<UserList>();
        UserDao uD = new UserDao();
        const float moneyPay = 1000;

        async void listen()
        {
            x = await cl.OnAsync("APIParking/Parking/IdList/" + BLOCK, added: (s, args, d) =>
            {
                Console.WriteLine(args.Data);
                FirebaseResponse userFalse = cl.Get("APIParking/Parking/InfoList/" + BLOCK + "/" + args.Data);
                AcceptUser acU = userFalse.ResultAs<AcceptUser>();
                //Nếu đó là lúc vào và lỗi 
                if (acU.lineOutIn == 1)
                {
                    if (acU.status == 0)
                    {
                            lAU.Add(acU);
                            updateGrid();
                    }
                    else
                    {
                        uD.addNewUser(acU);
                        updateGrid2();
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
                AcceptUser user = resCheck.ResultAs<AcceptUser>();
                Console.WriteLine(args.Data);
                lAU.RemoveAll(x=>x.id.Equals(user.id));
                lAU.Add(user);
                updateGrid();
                
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

        void acceptIn(AcceptUser ac)
        {
            cl.Update("APIParking/Parking/InfoList/"+ac.block+"/"+ac.id, ac);
        }

        void updateGridRemove(int index)
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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("Accept"))
            {
                int index = int.Parse(e.RowIndex.ToString());
                
                if (MessageBox.Show("Accept ?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Console.WriteLine("OK Accept " + lAU[index].id);
                    AcceptUser ac = lAU[index];
                    //Check line vao hay line râ
                    if (ac.lineOutIn == 1)
                    {
                        Console.WriteLine("Cổng vào");
                        //Check type loi
                        //Nếu đây là lỗi đọc sai biển số
                        if(ac.codeErr==0)
                        {
                            ac.txtPlate = "NOD";
                            ac.codeErr = 2;
                            ac.status = 2;
                            acceptIn(ac);
                            updateGridRemove(index);
                            //Them vao SQL
                            uD.addNewUser(ac);
                            updateGrid2();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cổng ra");
                        //Có 2 lỗi đặc biệt
                        //Không đọc được biển số
                        //Kiểm tra tiền không đủ
                        //Nếu không đọc được biển số
                        //Kiểm tra tiền, nếu còn tiền thì cho qua, không còn tiền thì không cho qua
                        switch (ac.codeErr)
                        {
                            case 0:
                                
                                ac.status = 2;
                                ac.codeErr = 2;
                                ac.txtPlate = "NOD";
                                //Đưa xác nhận sang cổng
                                acceptIn(ac);
                                //Thanh toán tiền
                                payMoney(ac.id, moneyPay);
                                removeIdErrList(ac);
                                //Add giao dịch
                                addHistory(ac, moneyPay);
                                //Xóa info trên APIParing
                                removeUserIn(ac);
                                //Cập nhật lại danh sách lỗi gridview
                                updateGridRemove(index);
                                //Xóa  IdErrList
                                return;
                            case 1:
                                ac.status = 2;
                                ac.codeErr = 2;
                                acceptIn(ac);
                                removeIdErrList(ac);
                                addHistory(ac, 0);
                                removeUserIn(ac);
                                updateGridRemove(index);
                                Console.WriteLine("OK da cho qua vi thieu tien");
                                return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Khong cho di qua");
                    AcceptUser ac = lAU[index];
                    if (ac.lineOutIn == 1)
                    {
                        if (ac.codeErr == 0)
                        {
                            ac.status = 1;
                            ac.codeErr = 2;
                            ac.txtPlate = "NOD";
                            acceptIn(ac);
                            removeUserIn(ac);
                            updateGridRemove(index);
                        }
                    }
                    else
                    {

                    }
                    
                }
               
            }
        }
    }
}
