using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugBusinessLogic.Module
{
    public static class Util
    {
        public static string AmountFormat(string val)
        {
            double s = Util.tonumeric(val);
            if (s != 0)
                return s.ToString("N");
            else
                return val;
        }
        public static string GetSequence(string type)
        {
            string sequence = null;
            DateTime? now = DateTime.Now;

            if (now != null)
            {
                sequence = string.Format("{0}{1:D4}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}{7:D3}", type, now.Value.Year, now.Value.Month, now.Value.Day, now.Value.Hour, now.Value.Minute, now.Value.Second, now.Value.Millisecond);
                //sequence = type + now.Value.Year + now.Value.Month + now.Value.Day + now.Value.Hour + now.Value.Minute + now.Value.Second + now.Value.Millisecond;
            }
            return sequence;
        }
        public static string checkdbnull(object obj)
        {
            try
            {
                if (obj is DBNull)
                    return "";
                else if (obj == null)
                    return "";
                else
                    return obj.ToString().Trim();
            }
            catch (Exception er)
            {
                return "";
            }
        }

        public static int toint(object obj)
        {
            int rValue;
            if (obj is DBNull) rValue = 0;
            else if (obj == null) rValue = 0;
            else
                try
                {
                    rValue = Convert.ToInt32(obj);
                }
                catch (Exception)
                {
                    rValue = 0;
                }
            return rValue;
        }

        public static double tonumeric(object obj)
        {
            double rValue;
            if (obj is DBNull) rValue = 0;
            else if (obj == null) rValue = 0;
            else
                try
                {
                    rValue = Convert.ToDouble(obj);
                }
                catch (Exception)
                {
                    rValue = 0;
                }
            return rValue;
        }
        public static void test()
        {
            //
            System.Data.DataTable t1 = TugBusinessLogic.Module.SqlHelper.GetDatatableBySql("SELECT [IDX],[Amount] FROM [hds153625288_db].[dbo].[Billing]  where [InvoiceType] <> \'其他账单\' order by idx");
            System.Data.DataTable t2 = TugBusinessLogic.Module.SqlHelper.GetDatatableBySql("SELECT [BillingID],sum([Amount]) As Amt FROM [hds153625288_db].[dbo].[V_AmountSum_Service] where CONVERT(datetime,ServiceWorkDate) >='2017-5-01 00:00:00' and CONVERT(datetime,ServiceWorkDate) < '2017-6-1 00:00:00' group by [BillingID] order by billingid");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("BillingID");
            dt.Columns.Add("Amount1");
            dt.Columns.Add("Amount2");

            for (int i = 0; i < t1.Rows.Count; i++)
            {
                int billingIdx1 = Convert.ToInt32(t1.Rows[i]["IDX"]);
                double amount1 = TugBusinessLogic.Module.Util.tonumeric(t1.Rows[i]["Amount"]);

                int j = 0;
                for (; j < t2.Rows.Count; j++)
                {
                    int billingIdx2 = Convert.ToInt32(t2.Rows[j]["BillingID"]);
                    double amount2 = TugBusinessLogic.Module.Util.tonumeric(t2.Rows[j]["Amt"]);

                    if (billingIdx1 == billingIdx2)
                    {
                        if (amount1 == amount2) { break; }
                        else
                        {
                            //shuchu
                            System.Data.DataRow r = dt.NewRow();
                            r["BillingID"] = billingIdx1;
                            r["Amount1"] = amount1;
                            r["Amount2"] = amount2;

                            dt.Rows.Add(r);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (j >= t2.Rows.Count)
                {
                    continue;
                    //shuchu
                    System.Data.DataRow r = dt.NewRow();
                    r["BillingID"] = billingIdx1;
                    r["Amount1"] = amount1;
                    r["Amount2"] = -1;

                    dt.Rows.Add(r);
                }
            }
        }
    }
}