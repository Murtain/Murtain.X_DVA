using IdentityAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Admin.Controllers
{
    class EmbeddedHtmlResult : IActionResult
    {
        string path;
        string file;
        string authorization_endpoint;

        public EmbeddedHtmlResult(HttpRequest request, string file)
        {
            this.path = "/";
            this.file = file;
            this.authorization_endpoint = "http://localhost:5000" + Constants.AuthorizePath;
        }


        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.FromResult(GetResponseMessage());
        }

        public HttpResponseMessage GetResponseMessage()
        {
            var html = AssetManager.LoadResourceString(this.file,
                new
                {
                    pathBase = this.path,
                    model = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        PathBase = this.path,
                        ShowLoginButton = true,
                        SiteName = "Admini",
                        oauthSettings = new
                        {
                            authorization_endpoint = this.authorization_endpoint,
                            client_id = Constants.IdAdmMgrClientId
                        }
                    })
                });

            return new HttpResponseMessage()
            {
                Content = new StringContent(html, Encoding.UTF8, "text/html")
            };
        }
    }
}
