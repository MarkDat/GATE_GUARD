using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATE_GUARD2.Dao
{
    public class GuardDao
    {
        public GuardDao(IFirebaseClient cl)
        {
            this.cl = cl;
        }

        IFirebaseClient cl;
        public async Task<JObject> Login(string username, string pwd)
        {
            FirebaseResponse res = await cl.GetAsync(@"User/account/" + username);
            if (res.Body.Equals("null")) return null;
            JObject dt = res.ResultAs<JObject>();
            if (!pwd.Equals(dt["pwd"].ToString())) return null;
            if (!dt["position"].ToString().Equals("0")) return null;
            //Nếu có thông tin thì lấy thông tin theo id guard
            FirebaseResponse resInfo = cl.Get(@"User/information/guardBOT/" + dt["id"].ToString());
            Console.WriteLine(((JObject)resInfo.ResultAs<JObject>()).ToString());
            JObject j = resInfo.ResultAs<JObject>();
            return await Task.FromResult<JObject>(j);
        }
    }
}
