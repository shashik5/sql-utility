using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using SqlUtilityCore;

namespace SqlUtilityWeb
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
                Response.End();
            }
            else
            {
                string sessionId = Request.Headers.Get("sessionId");
                if (!string.IsNullOrEmpty(sessionId))
                {
                    if (!HttpContext.Current.Application.AllKeys.Contains(sessionId))
                    {
                        MsSqlUtility msSqlUtility = new MsSqlUtility();
                        HttpContext.Current.Application.Add(sessionId, msSqlUtility); 
                    }
                }
                else
                {
                    //TODO: Create new session Id instead of throwing error.
                    throw new Exception("Invalid Session Id");
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}