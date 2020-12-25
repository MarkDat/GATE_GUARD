using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATE_GUARD2.Dao
{
    public class AcceptUserTemp
    {
        public string id { get; set; }
        public string idT { get; set; }
        public bool parking { get; set; } = true;
        public string name { get; set; }
        //0 là đang chờ bảo vệ accept
        //1 là bảo vệ k cho qua
        //2 là đc cho qua
        //Khi satus false, khong quet duoc bien so
        public int status { get; set; } = 2;

        //0: Sai biển số xe
        //1: Thiếu tiền
        //2: OK
        //3: Không thể add history
        public int codeErr { get; set; } = 2;


        public string img { get; set; } = "";
        public string plateImg { get; set; } = "";
        public string plateErrImg { get; set; } = "";

        public int line { get; set; } = 1;

        //1 là line vào
        //0 là line ra
        public int lineOutIn { get; set; } = 1;

        public int block { get; set; } = 1;
        public string dateSend { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string txtPlate { get; set; } = "none";
        public int position { get; set; }


        //Nếu đi vào không có lỗi thì true, ngược lại
        public bool isInOK { get; set; }
    }
}
