using GATE_GUARD2.Dao;
using GATE_GUARD2.EF;
using GATE_GUARD2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATE_GUARD2.Db
{
    public class UserDao
    {
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=DTUPARKING;Integrated Security=True");
        private DbDTUContext context = null;
        public UserDao()
        {
            context = new DbDTUContext();
        }

        public bool addNewUser(AcceptUser u)
        {
            string q = "";
            bool checkParking = findParking(u.id) == null ? false : true;
            if (checkParking) {
                Console.WriteLine("Đang đỗ");
                return false;
            } 

            bool checkUser = findUser(u.id)==null ? false:true;
            bool checkPlate = findPlate(u.txtPlate) == null ? false : true;
           

            if ( !checkUser && !checkPlate )
            {
                q = "EXEC dbo.addAllUser @ids,@id,@name,@position,@imgPath,@imgPlatePath,@txtPlate,@dtIn,@status,@dtOut";
                using (SqlCommand command = new SqlCommand(q, conn))
                {
                    command.Parameters.AddWithValue("@ids", u.idT);
                    command.Parameters.AddWithValue("@id", u.id);
                    command.Parameters.AddWithValue("@name", u.name);
                    command.Parameters.AddWithValue("@position", u.position);
                    command.Parameters.AddWithValue("@imgPath", u.plateImg);
                    command.Parameters.AddWithValue("@imgPlatePath", "");
                    command.Parameters.AddWithValue("@txtPlate", u.txtPlate);
                    command.Parameters.AddWithValue("@dtIn", DateTime.Now);
                    command.Parameters.AddWithValue("@status", "true");
                    command.Parameters.AddWithValue("@dtOut", DateTime.Now);
                    conn.Open();
                    int result = command.ExecuteNonQuery();
                    conn.Close();
                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
                Console.WriteLine("Add all success");
                return true;
            }


            if (!checkUser)
            {
                q = "EXEC dbo.addUser @ids,@id,@name,@position";
                using (SqlCommand command = new SqlCommand(q, conn))
                {
                    command.Parameters.AddWithValue("@ids", u.idT);
                    command.Parameters.AddWithValue("@id", u.id);
                    command.Parameters.AddWithValue("@name", u.name);
                    command.Parameters.AddWithValue("@position", u.position);
                    conn.Open();
                    int result = command.ExecuteNonQuery();
                    conn.Close();
                    // Check Error
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return true;
                    }
                       
                }
                Console.WriteLine("Add user success");
            }
            if (!checkPlate)
            {
                q = "EXEC dbo.addPlate @imgPath,@imgPlatePath,@txtPlate";
                using (SqlCommand command = new SqlCommand(q, conn))
                {
                    command.Parameters.AddWithValue("@imgPath", u.plateImg);
                    command.Parameters.AddWithValue("@imgPlatePath", "");
                    command.Parameters.AddWithValue("@txtPlate", u.txtPlate);
                    conn.Open();
                    int result = command.ExecuteNonQuery();
                    conn.Close();
                    // Check Error
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return true;
                    }
                }
                Console.WriteLine("Add plate success");
            }
            if (!checkParking)
            {
                q = "EXEC dbo.addParking @id,@txtPlate,@dtIn,@status,@isInOK,@dtOut";
                using (SqlCommand command = new SqlCommand(q, conn))
                {
                    command.Parameters.AddWithValue("@id", u.id);
                    command.Parameters.AddWithValue("@txtPlate", u.txtPlate);
                    command.Parameters.AddWithValue("@dtIn", DateTime.Parse(u.dateSend));
                    command.Parameters.AddWithValue("@status", "true");
                    command.Parameters.AddWithValue("@isInOK", u.isInOK ? "true":"false");
                    command.Parameters.AddWithValue("@dtOut", DateTime.Now);
                    conn.Open();
                    int result = command.ExecuteNonQuery();
                    conn.Close();
                    // Check Error
                    if (result < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return true;
                    }
                    Console.WriteLine("Add parkinng success");
                }
                
            }
            return true;
        }
        public List<UserList> getListParking()
        {
            return context.Database.SqlQuery<UserList>("EXEC getList").ToList();
        }
        public USER_VEHICLE findUser(string id)
        {
            return context.USER_VEHICLE.Where(x => x.ID == id).SingleOrDefault();
        }
       
        public LICENSE_PLATE findPlate(string txtPlate)
        {
            return context.LICENSE_PLATE.Where(x => x.TxtPlate==txtPlate).SingleOrDefault();
        }
        public PARKING findParking(string id)
        {
            return context.PARKINGs.Where(x => x.ID == id && x.Status==true).FirstOrDefault();
        }
        public bool removeParking(string id)
        {
            PARKING pk = findParking(id.Trim());
            if (pk == null) return false;
            string q = "EXEC removeParking @id,@dateOut";
            using (SqlCommand command = new SqlCommand(q, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@dateOut", DateTime.Now);
                conn.Open();
                int result = command.ExecuteNonQuery();
                conn.Close();
                // Check Error
                return result > 0;
            }
        }

    }
}
