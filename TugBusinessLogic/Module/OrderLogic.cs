using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TugDataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TugBusinessLogic.Module
{
    public class OrderLogic
    {
        static public List<TugDataModel.V_OrderInfor> SearchForOrderMange(string searchOptions)
        {
            //string s = Request.QueryString["filters"];

            string jsonText = "[{'a':'aaa','b':'bbb','c':'ccc'},{'a':'aaa2','b':'bbb2','c':'ccc2'}]";
            try
            {
                
                //{
                //  "groupOp": "AND",
                //  "rules": [
                //    {
                //      "field": "IsGuest",
                //      "op": "eq",
                //      "data": "是"
                //    }
                //  ]
                //}

                JObject jsonSearchOption = (JObject)JsonConvert.DeserializeObject(searchOptions);
                string groupOp = (string)jsonSearchOption["groupOp"];
                JArray rules = (JArray)jsonSearchOption["rules"];
                foreach(JObject item in rules)
                {
                    string field = (string)item["field"];
                    string op = (string)item["op"];
                    string data = (string)item["data"];
                }
                //object o = ja.rules;
            }
            catch(Exception ex)
            {
                return null;
            }
            //JObject o = (JObject)ja[1];
            //Console.WriteLine(o["a"]);
            //Console.WriteLine(ja[1]["a"]);

            TugDataEntities db = new TugDataEntities();
            List<V_OrderInfor> orders = db.V_OrderInfor.Where(u => u.IDX == -1).Select(u => u).OrderByDescending(u => u.IDX).ToList<V_OrderInfor>();

            return orders;
        }
    }
}
