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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchOptions">搜索选项，格式如下</param>
        /// <returns></returns>
        static public List<TugDataModel.V_OrderInfor> SearchForOrderMange(string orderField, string orderMethod, string searchOptions)
        {
            List<V_OrderInfor> orders = null;
            try
            {
                //searchOptions的Json字符串格式
                //{
                //    "groupOp":"AND",
                //    "rules":[{"field":"IsGuest","op":"eq","data":"全部"}],
                //    "groups":[
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"BigTugNum"},{"data":"2","op":"le","field":"BigTugNum"}]},
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"MiddleTugNum"},{"data":"2","op":"le","field":"MiddleTugNum"}]},
                //        {"groupOp":"AND","groups":[],"rules":[{"data":"1","op":"ge","field":"SmallTugNum"},{"data":"2","op":"le","field":"SmallTugNum"}]}
                //    ]

                //}

                TugDataEntities db = new TugDataEntities();
                orders = db.V_OrderInfor.Select(u => u).ToList<V_OrderInfor>();

                JObject jsonSearchOption = (JObject)JsonConvert.DeserializeObject(searchOptions);
                string groupOp = (string)jsonSearchOption["groupOp"];
                JArray rules = (JArray)jsonSearchOption["rules"];

                
                if (rules != null)
                {
                    foreach (JObject item in rules)
                    {
                        string field = (string)item["field"];
                        string op = (string)item["op"];
                        string data = (string)item["data"];

                        #region 根据各自段条件进行搜索
                        switch (field)
                        {
                            #region IsGuest
                            case "IsGuest":
                                {
                                    switch(op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                if(data != "全部")
                                                    orders = orders.Where(u => u.IsGuest == data).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region Code
                            case "Code":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.Code.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.Code.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.Code.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.Code.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region CustomerName
                            case "CustomerName":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.CustomerName.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.CustomerName.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.CustomerName.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.CustomerName.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region WorkDate
                            case "WorkDate":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.WorkDate == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.WorkDate.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.WorkDate.CompareTo(data) == -1 || u.WorkDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.WorkDate.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.WorkDate.CompareTo(data) == 1 || u.WorkDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region WorkTime
                            case "WorkTime":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.WorkTime == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.WorkTime.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.WorkTime.CompareTo(data) == -1 || u.WorkTime.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.WorkTime.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.WorkTime.CompareTo(data) == 1 || u.WorkTime.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region EstimatedCompletionTime
                            case "EstimatedCompletionTime":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.EstimatedCompletionTime == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data) == -1 || u.EstimatedCompletionTime.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.EstimatedCompletionTime.CompareTo(data) == 1 || u.EstimatedCompletionTime.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region ShipName
                            case "ShipName":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.ShipName.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.ShipName.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.ShipName.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.ShipName.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region LinkMan
                            case "LinkMan":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.LinkMan.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.LinkMan.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.LinkMan.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.LinkMan.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region LinkPhone
                            case "LinkPhone":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.LinkPhone.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.LinkPhone.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.LinkPhone.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.LinkPhone.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region LinkEmail
                            case "LinkEmail":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.LinkEmail.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.LinkEmail.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.LinkEmail.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.LinkEmail.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region WorkPlace
                            case "WorkPlace":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.WorkPlace.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.WorkPlace.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.WorkPlace.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.WorkPlace.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region ServiceNatureNames
                            case "ServiceNatureNames":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.ServiceNatureNames.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.ServiceNatureNames.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.ServiceNatureNames.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.ServiceNatureNames.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region WorkStateLabel
                            case "WorkStateLabel":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                int workStateId = Convert.ToInt32(data.Split('~')[0]);
                                                if (workStateId != -1)
                                                {
                                                    orders = orders.Where(u => u.WorkStateID == workStateId).ToList();
                                                }
                                                
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region BigTugNum
                            case "BigTugNum":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.BigTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.BigTugNum < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.BigTugNum < Convert.ToInt32(data) || u.BigTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.BigTugNum > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.BigTugNum > Convert.ToInt32(data) || u.BigTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region MiddleTugNum
                            case "MiddleTugNum":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.MiddleTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.MiddleTugNum < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.MiddleTugNum < Convert.ToInt32(data) || u.MiddleTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.MiddleTugNum > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.MiddleTugNum > Convert.ToInt32(data) || u.MiddleTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region SmallTugNum
                            case "SmallTugNum":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.SmallTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.SmallTugNum < Convert.ToInt32(data) || u.SmallTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.SmallTugNum > Convert.ToInt32(data) || u.SmallTugNum == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region Remark
                            case "Remark":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.Remark.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.Remark.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.Remark.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.Remark.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region CreateDate
                            case "CreateDate":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.CreateDate == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.CreateDate.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.CreateDate.CompareTo(data) == -1 || u.CreateDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.CreateDate.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.CreateDate.CompareTo(data) == 1 || u.CreateDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;                            
                            #endregion

                            #region LastUpDate
                            case "LastUpDate":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.LastUpDate == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.LastUpDate.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.LastUpDate.CompareTo(data) == -1 || u.LastUpDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.LastUpDate.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.LastUpDate.CompareTo(data) == 1 || u.LastUpDate.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol1
                            case "UserDefinedCol1":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol1.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol1.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol1.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol1.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol2
                            case "UserDefinedCol2":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol2.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol2.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol2.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol2.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol3
                            case "UserDefinedCol3":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol3.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol3.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol3.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol3.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol4
                            case "UserDefinedCol4":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol4.ToLower().CompareTo(data.ToLower()) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol4.ToLower().StartsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_EW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol4.ToLower().EndsWith(data.ToLower())).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_CN:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol4.ToLower().Contains(data.ToLower())).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol5
                            case "UserDefinedCol5":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol5 == Convert.ToDouble(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol5 < Convert.ToDouble(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol5 < Convert.ToDouble(data) || u.UserDefinedCol5 == Convert.ToDouble(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol5 > Convert.ToDouble(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol5 > Convert.ToDouble(data) || u.UserDefinedCol5 == Convert.ToDouble(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol6
                            case "UserDefinedCol6":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol6 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol6 < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol6 < Convert.ToInt32(data) || u.UserDefinedCol6 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol6 > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol6 > Convert.ToInt32(data) || u.UserDefinedCol6 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol7
                            case "UserDefinedCol7":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol7 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol7 < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol7 < Convert.ToInt32(data) || u.UserDefinedCol7 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol7 > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol7 > Convert.ToInt32(data) || u.UserDefinedCol7 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol8
                            case "UserDefinedCol8":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol8 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol8 < Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol8 < Convert.ToInt32(data) || u.UserDefinedCol8 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol8 > Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol8 > Convert.ToInt32(data) || u.UserDefinedCol8 == Convert.ToInt32(data)).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol9
                            case "UserDefinedCol9":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol9 == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol9.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol9.CompareTo(data) == -1 || u.UserDefinedCol9.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol9.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol9.CompareTo(data) == 1 || u.UserDefinedCol9.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            #region UserDefinedCol10
                            case "UserDefinedCol10":
                                {
                                    switch (op)
                                    {
                                        case ConstValue.ComparisonOperator_EQ:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol10 == data).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol10.CompareTo(data) == -1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_LE:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol10.CompareTo(data) == -1 || u.UserDefinedCol10.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GT:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol10.CompareTo(data) == 1).ToList();
                                            }
                                            break;
                                        case ConstValue.ComparisonOperator_GE:
                                        case ConstValue.ComparisonOperator_BW:
                                            {
                                                orders = orders.Where(u => u.UserDefinedCol10.CompareTo(data) == 1 || u.UserDefinedCol10.CompareTo(data) == 0).ToList();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            #endregion

                            default:
                                break;
                        }
                        #endregion

                        #region 对搜索结果根据排序字段和方式进行排序
                        switch (orderField)
                        {
                            case "":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.IDX).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.IDX).ToList();
                                }
                                break;
                            case "IsGuest":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.IsGuest).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.IsGuest).ToList();
                                }
                                break;
                            case "Code":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.Code).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.Code).ToList();
                                }
                                break;
                            case "CustomerName":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.CustomerName).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                                }
                                break;
                            case "OrdTime":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.WorkDate).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                                }
                                break;
                            case "WorkTime":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.WorkTime).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                                }
                                break;
                            case "EstimatedCompletionTime":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                                }
                                break;
                            case "ShipName":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.ShipName).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.ShipName).ToList();
                                }
                                break;
                            case "LinkMan":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.LinkMan).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.LinkMan).ToList();
                                }
                                break;
                            case "LinkPhone":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.LinkPhone).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.LinkPhone).ToList();
                                }
                                break;
                            case "LinkEmail":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.LinkEmail).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.LinkEmail).ToList();
                                }
                                break;
                            case "WorkPlace":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.WorkPlace).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.WorkPlace).ToList();
                                }
                                break;
                            case "ServiceNatureNames":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                                }
                                break;
                            case "WorkStateLabel":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                                }
                                break;
                            case "BigTugNum":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.BigTugNum).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.BigTugNum).ToList();
                                }
                                break;
                            case "MiddleTugNum":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.MiddleTugNum).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.MiddleTugNum).ToList();
                                }
                                break;
                            case "SmallTugNum":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.SmallTugNum).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.SmallTugNum).ToList();
                                }
                                break;
                            case "Remark":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.Remark).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.Remark).ToList();
                                }
                                break;
                            case "CreateDate":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.CreateDate).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.CreateDate).ToList();
                                }
                                break;
                            case "LastUpDate":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.LastUpDate).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.LastUpDate).ToList();
                                }
                                break;
                            case "UserDefinedCol1":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol1).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol1).ToList();
                                }
                                break;
                            case "UserDefinedCol2":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol2).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol2).ToList();
                                }
                                break;
                            case "UserDefinedCol3":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol3).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol3).ToList();
                                }
                                break;
                            case "UserDefinedCol4":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol4).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol4).ToList();
                                }
                                break;
                            case "UserDefinedCol5":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol5).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol5).ToList();
                                }
                                break;
                            case "UserDefinedCol6":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol6).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol6).ToList();
                                }
                                break;
                            case "UserDefinedCol7":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol7).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol7).ToList();
                                }
                                break;
                            case "UserDefinedCol8":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol8).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol8).ToList();
                                }
                                break;
                            case "UserDefinedCol9":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol9).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol9).ToList();
                                }
                                break;
                            case "UserDefinedCol10":
                                {
                                    if (orderMethod.ToLower().Equals("asc"))
                                        orders = orders.OrderBy(u => u.UserDefinedCol10).ToList();
                                    else
                                        orders = orders.OrderByDescending(u => u.UserDefinedCol10).ToList();
                                }
                                break;
                            default:
                                break;
                        }
                        #endregion
                    }
                }

                JArray groups = (JArray)jsonSearchOption["groups"];
                if (groups != null)
                {
                    foreach (JObject item in groups)
                    {
                        string item_groupOp = (string)item["groupOp"];
                        JArray item_groups = (JArray)item["groups"];
                        JArray item_rules = (JArray)item["rules"];
                        string item_rule0_field = (string)(((JObject)item_rules[0])["field"]);
                        string item_rule0_op = (string)(((JObject)item_rules[0])["op"]);
                        string item_rule0_data = (string)(((JObject)item_rules[0])["data"]);

                        string item_rule1_field = (string)(((JObject)item_rules[1])["field"]);
                        string item_rule1_op = (string)(((JObject)item_rules[1])["op"]);
                        string item_rule1_data = (string)(((JObject)item_rules[1])["data"]);
                    }
                }

            }
            catch(Exception ex)
            {
                return null;
            }
            return orders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderMethod">排序方式asc升序；desc降序</param>
        /// <returns></returns>
        static public List<TugDataModel.V_OrderInfor> LoadDataForOrderManage(string orderField, string orderMethod)
        {
            List<V_OrderInfor> orders = null;

            try
            {
                TugDataEntities db = new TugDataEntities();
                orders = db.V_OrderInfor.Select(u => u).ToList<V_OrderInfor>();

                #region 根据排序字段和排序方式排序
                switch (orderField)
                {
                    case "":
                        {
                            if(orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.IDX).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.IDX).ToList();
                        }
                        break;
                    case "IsGuest":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.IsGuest).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.IsGuest).ToList();
                        }
                        break;
                    case "Code":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Code).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Code).ToList();
                        }
                        break;
                    case "CustomerName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CustomerName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CustomerName).ToList();
                        }
                        break;
                    case "OrdTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkDate).ToList();
                        }
                        break;
                    case "WorkTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkTime).ToList();
                        }
                        break;
                    case "EstimatedCompletionTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.EstimatedCompletionTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.EstimatedCompletionTime).ToList();
                        }
                        break;
                    case "ShipName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ShipName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ShipName).ToList();
                        }
                        break;
                    case "LinkMan":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.LinkMan).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.LinkMan).ToList();
                        }
                        break;
                    case "LinkPhone":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.LinkPhone).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.LinkPhone).ToList();
                        }
                        break;
                    case "LinkEmail":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.LinkEmail).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.LinkEmail).ToList();
                        }
                        break;
                    case "WorkPlace":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkPlace).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkPlace).ToList();
                        }
                        break;
                    case "ServiceNatureNames":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ServiceNatureNames).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ServiceNatureNames).ToList();
                        }
                        break;
                    case "WorkStateLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkStateLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkStateLabel).ToList();
                        }
                        break;
                    case "BigTugNum":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.BigTugNum).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.BigTugNum).ToList();
                        }
                        break;
                    case "MiddleTugNum":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.MiddleTugNum).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.MiddleTugNum).ToList();
                        }
                        break;
                    case "SmallTugNum":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.SmallTugNum).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.SmallTugNum).ToList();
                        }
                        break;
                    case "Remark":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Remark).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Remark).ToList();
                        }
                        break;
                    case "CreateDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CreateDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CreateDate).ToList();
                        }
                        break;
                    case "LastUpDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.LastUpDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.LastUpDate).ToList();
                        }
                        break;
                    case "UserDefinedCol1":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol1).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol1).ToList();
                        }
                        break;
                    case "UserDefinedCol2":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol2).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol2).ToList();
                        }
                        break;
                    case "UserDefinedCol3":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol3).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol3).ToList();
                        }
                        break;
                    case "UserDefinedCol4":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol4).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol4).ToList();
                        }
                        break;
                    case "UserDefinedCol5":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol5).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol5).ToList();
                        }
                        break;
                    case "UserDefinedCol6":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol6).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol6).ToList();
                        }
                        break;
                    case "UserDefinedCol7":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol7).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol7).ToList();
                        }
                        break;
                    case "UserDefinedCol8":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol8).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol8).ToList();
                        }
                        break;
                    case "UserDefinedCol9":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol9).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol9).ToList();
                        }
                        break;
                    case "UserDefinedCol10":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol10).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol10).ToList();
                        }
                        break;
                    default:
                        break;
                }

                #endregion
            }
            catch(Exception ex)
            {
                return null;
            }

            return orders;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderMethod">排序方式asc升序；desc降序</param>
        /// <returns></returns>
        static public List<TugDataModel.V_OrderScheduler> LoadDataForOrderScheduler(string orderField, string orderMethod)
        {
            List<V_OrderScheduler> orders = null;

            try
            {
                TugDataEntities db = new TugDataEntities();
                orders = db.V_OrderScheduler.Select(u => u).ToList<V_OrderScheduler>();

                orderField = orderField.Split(',')[1];
                orderField = orderField.Trim();
                #region 根据排序字段和排序方式排序
                switch (orderField)
                {
                    case "":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.IDX).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.IDX).ToList();
                        }
                        break;
                    case "ServiceNatureLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ServiceNatureLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ServiceNatureLabel).ToList();
                        }
                        break;
                    case "CnName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CnName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CnName).ToList();
                        }
                        break;
                    case "EnName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.EnName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.EnName).ToList();
                        }
                        break;
                    case "SimpleName":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.SimpleName).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.SimpleName).ToList();
                        }
                        break;
                    case "JobStateLabel":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.JobStateLabel).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.JobStateLabel).ToList();
                        }
                        break;
                    case "InformCaptainTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.InformCaptainTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.InformCaptainTime).ToList();
                        }
                        break;
                    case "CaptainConfirmTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CaptainConfirmTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CaptainConfirmTime).ToList();
                        }
                        break;
                    case "DepartBaseTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.DepartBaseTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.DepartBaseTime).ToList();
                        }
                        break;
                    case "ArrivalShipSideTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ArrivalShipSideTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ArrivalShipSideTime).ToList();
                        }
                        break;
                    case "WorkCommencedTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkCommencedTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkCommencedTime).ToList();
                        }
                        break;
                    case "WorkCompletedTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.WorkCompletedTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.WorkCompletedTime).ToList();
                        }
                        break;
                    case "ArrivalBaseTime":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.ArrivalBaseTime).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.ArrivalBaseTime).ToList();
                        }
                        break;
                    case "RopeUsed":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.RopeUsed).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.RopeUsed).ToList();
                        }
                        break;
                    case "RopeNum":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.RopeNum).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.RopeNum).ToList();
                        }
                        break;
                    
                    case "Remark":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.Remark).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.Remark).ToList();
                        }
                        break;
                    case "CreateDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.CreateDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.CreateDate).ToList();
                        }
                        break;
                    case "LastUpDate":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.LastUpDate).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.LastUpDate).ToList();
                        }
                        break;
                    case "UserDefinedCol1":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol1).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol1).ToList();
                        }
                        break;
                    case "UserDefinedCol2":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol2).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol2).ToList();
                        }
                        break;
                    case "UserDefinedCol3":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol3).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol3).ToList();
                        }
                        break;
                    case "UserDefinedCol4":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol4).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol4).ToList();
                        }
                        break;
                    case "UserDefinedCol5":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol5).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol5).ToList();
                        }
                        break;
                    case "UserDefinedCol6":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol6).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol6).ToList();
                        }
                        break;
                    case "UserDefinedCol7":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol7).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol7).ToList();
                        }
                        break;
                    case "UserDefinedCol8":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol8).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol8).ToList();
                        }
                        break;
                    case "UserDefinedCol9":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol9).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol9).ToList();
                        }
                        break;
                    case "UserDefinedCol10":
                        {
                            if (orderMethod.ToLower().Equals("asc"))
                                orders = orders.OrderBy(u => u.UserDefinedCol10).ToList();
                            else
                                orders = orders.OrderByDescending(u => u.UserDefinedCol10).ToList();
                        }
                        break;
                    default:
                        break;
                }

                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }

            return orders;
        }
    }
}
