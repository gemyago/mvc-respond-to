using System.Collections.Generic;
using System.Web.Mvc;

namespace Mvc.RespondTo.Tests.Samples
{
    public class PeopleController : Controller
    {
        public ActionResult Index()
        {
            var people = new List<string> {"Larry", "Garry", "Sam"};
            return this.RespondTo(format =>
            {
                format.Html(() => View(people));
                format.Json(() => Json(people));

                //Handling custom mime type
                format.Mime("text/plain", () => Content(string.Join(", ", people)));
            });
        }
    }
}