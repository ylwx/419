using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugBusinessLogic.Module
{
    public static class Util
    {
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
    }
}