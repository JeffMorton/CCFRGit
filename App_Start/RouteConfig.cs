using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace CCFRW19
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings() { AutoRedirectMode = RedirectMode.Permanent };
            routes.EnableFriendlyUrls(settings);
        }
    }
}