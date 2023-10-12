using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ServiceReference1;
using System.IO;
using System.Net;
using System.Text.Json;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoProject2.Models;

namespace UmbracoProject2.Controllers
{
	public class LoginSurfaceController : SurfaceController
	{
		public const string PARTIALS_VIEWS_FOLDER = "~/Views/Partials";
		public LoginSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) 
			: base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{

		}

		public IActionResult RenderForm()
		{
			return PartialView(PARTIALS_VIEWS_FOLDER + "Login.cshtml");
		}

		[HttpPost]

		public async Task<IActionResult> Submit(User user) 
		{ 
			if (ModelState.IsValid)
			{
				ICUTechClient client = new ICUTechClient();
				var response = await client.LoginAsync(user.UserName, user.Password, GetIPAddress());
				//string jsonString = JsonSerializer.Serialize(response.@return);
        UserResponse userResponse = JsonSerializer.Deserialize<UserResponse>(response.@return)!;
				if (userResponse != null && userResponse.EntityId !=0)
				{ 
					TempData["EntityId"] = userResponse.EntityId;
          TempData["FirstName"] = userResponse.FirstName;
          TempData["LastName"] = userResponse.LastName;
          TempData["Email"] = userResponse.Email;
          TempData["LogginSuccess"] = true;
          return RedirectToCurrentUmbracoPage();
        }
      } 
      TempData["LogginSuccess"] = false;
      return CurrentUmbracoPage();
		}

    public string GetIPAddress()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); 
      IPAddress ipAddress = ipHostInfo.AddressList[0];

      return ipAddress.ToString();
    }
  }
}
