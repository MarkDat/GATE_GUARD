using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATE_GUARD2.Models
{
    public class HistoryDTO
    {

        public string idPay { get; set; }
        public string dateGet { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string dateSend { get; set; }
        public int method { get; set; } = 0;
        public bool isNoti { get; set; } = false;
        public string payMoney { get; set; } = "1000";
        public int place { get; set; }
        public string plateLicense { get; set; } = "none";
    }
}
