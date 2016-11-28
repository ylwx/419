using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TugManagementSystem
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //////修复全包情况未向AmountSum写入数据的Bug
            ////TugBusinessLogic.Utils.QBInsertToAmount();
            //测试billing表的Amount字段的值是否正确
            //TugBusinessLogic.Utils.Billing_Amount_Value_HasError();
        }
    }
}
