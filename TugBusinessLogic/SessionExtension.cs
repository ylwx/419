using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TugBusinessLogic
{
    public static class SessionExtension
    {
        public static T GetDataFromSession<T>(this HttpSessionStateBase session, string key)
        {
            return (T)session[key];
        }

        public static void SetDataInSession<T>(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = value;
        }
    }
}
