using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Text;

namespace RK.Libraries
{
    public class Csv
    {
        public bool status { get; set; }
        public List<Dictionary<string,string>> data { get; set; }
        public string message { get; set; }

        public static Csv LoadCsvFile(string filePath)
        {
            Csv result = new Csv();
            var reader = new StreamReader(System.IO.File.OpenRead(filePath),Encoding.Default,true);
           
           
            List<string> head = new List<string>();
            
            
            try
            {
                List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
                bool first = true;
                while (!reader.EndOfStream)
                {
                    ///string value = reader.ReadToEnd();
                    //if (reader.ReadLine() != null)
                    //{
                        Dictionary<string, string> row = new Dictionary<string, string>();
                        string[] values = reader.ReadLine().Split("\",\"".ToCharArray());
                        

                        if (first)
                        {
                            first = false;
                            foreach (var value in values)
                            {
                                
                                head.Add(value);
                            }
                        }
                        else
                        {
                            
                            for (int i = 0; i < head.Count; i++)
                            {
                                var str = values[i];//.Replace("\"","");
                                //str = str.Replace(",","|");
                                row.Add(head[i], str);
                            }
                                //foreach (var value in values)
                                ///{
                                //values[index] = values[index]==null?"": value.Replace("\"", "");
                                //row.Add(head[index], value);
                                //  row.Add(index.ToString(), value);
                                //index++;
                                /// }
                                data.Add(row);
                        }
                       
                        

                }

                if (data.Count > 0)
                {
                    result.status = true;
                    result.data = data;
                    return result;
                }
                else
                {
                    result.status = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.status = false;
                result.message = ex.Message;
                return result;
            }

            //return Result(false,null,"Error inesperado");
        }
        

    }
}