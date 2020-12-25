using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATE_GUARD2.Models
{
    public class UserList
    {
        public string ID { get; set; }
        public string IDS { get; set; }
        public string Name { get; set; }
        public string TxtPlate { get; set; }
        public DateTime DateIn { get; set; }
        public int Position { get; set; }
        public bool IsInOk { get; set; }
    }
}
