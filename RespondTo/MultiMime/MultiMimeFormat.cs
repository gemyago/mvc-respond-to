using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.RespondTo.MultiMime
{
    public class MultiMimeFormat
    {
        public MultiMimeFormat()
        {
        }

        public MultiMimeFormat(Action<MultiMimeFormat> initializer)
        {
            initializer(this);
        }

        private readonly List<Func<string[], Func<ActionResult>>> _responderLocators = new List<Func<string[], Func<ActionResult>>>();

        public ActionResult ResultFor(params string[] mimeTypes)
        {
            Func<ActionResult> responder = null;
            foreach (var resultLocator in _responderLocators)
            {
                responder = resultLocator(mimeTypes);
                if (responder != null) break;
            }
            if (responder == null)
                throw new HttpException(406, "Not Acceptable.");
            return responder();
        }

        public ActionResult ResultFor(ControllerContext context)
        {
            return ResultFor(context.HttpContext.Request.AcceptTypes);
        }

        public void Mime(string mimeType, Func<ActionResult> responder, bool canRespondToAll = false)
        {
            _responderLocators
                .Add(acceptTypes => acceptTypes.Any(type => type == "*/*" && canRespondToAll || type == mimeType) ? responder : null);
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