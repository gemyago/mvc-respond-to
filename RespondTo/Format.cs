using System;
using System.Web.Mvc;

namespace Mvc.RespondTo
{
    public class Format
    {
        public Func<ActionResult> ResponderForMime(string mimeType)
        {
            throw new NotImplementedException();
        }

        public Func<ActionResult> ResponderForContext(ControllerContext context)
        {
            throw new NotImplementedException();
        }

        public void Mime(string mimeType, Func<ActionResult> responder, bool atLeast = false)
        {
        }

        public void Html(Func<ActionResult> responder)
        {
            Mime("text/html", responder, true);
        }

        public void Json(Func<ActionResult> responder)
        {
            Mime("application/json", responder);
        }

        public void Xml(Func<ActionResult> responder)
        {
            Mime("application/xml", responder);
        }
    }
}