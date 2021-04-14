using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data;

namespace RK.Security
{
    public class Ion_Auth
    {
       
        public Ion_Auth()
        {

        }
        public static bool Login(string username="", string password="")
        {
              rekursosEntities db = new rekursosEntities();
            var result_user = db.users.Where(w => w.username == username || w.email == username)
                                    .Join(db.groups, u => u.group_id, g => g.id, (u, g) => new { u, g }).SingleOrDefault() ;


            if (result_user != null)
            {
               
                if (result_user.u.password == Sha1.SHA1HashStringForUTF8String(password))
                {

                    SetLastLogin(result_user.u.id);
                    return true;
                }

                return false;
            }
            return false;
        }
        public static users GetUser(string username,bool group=true)
        {
            rekursosEntities db = new rekursosEntities();

            if (username != null)
            {
                users user = db.users.Where(w => w.username == username).FirstOrDefault();

                if(group)
                    user.groups = db.groups.Find(user.group_id);

                return user;
            }
            return null;
        }
        public static void SetLastLogin(int id)
        {
             rekursosEntities db = new rekursosEntities();
             users user = db.users.Find(id);

             user.last_login = DateTime.Now;

             db.Entry(user).State = EntityState.Modified;
             db.SaveChanges();
        }
    }
}