using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace RK.Libraries
{
    public class BDSequence
    {
        public static int GetSequence(string table = "")
        {

          

            rekursosEntities db = new rekursosEntities();

            try
            {
                var result = db.Database.SqlQuery<int>("SELECT NEXT VALUE FOR " + table + "_sequence");
                return result.SingleOrDefault();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                SetSequence(table);
                return 1;
            }


        }
        public static void SetSequence(string table = "")
        {
            rekursosEntities db = new rekursosEntities();
            var result = db.Database.ExecuteSqlCommand("create SEQUENCE  " + table + "_sequence AS int start with 2 increment by 1 minvalue 0 no maxvalue");

        }
        public static void RestartSequence(string table = "", string restart = "1")
        {
            //alter SEQUENCE empleados_sequence restart with 1
            rekursosEntities db = new rekursosEntities();
            var result = db.Database.ExecuteSqlCommand("alter SEQUENCE " + table + "_sequence restart with " + restart);

        }
    }
}