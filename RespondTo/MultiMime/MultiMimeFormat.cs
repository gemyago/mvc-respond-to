using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.RespondTo.MultiMime
{
    /// <summary>
    /// Class that is used to register responders for multiple MIME types.
    /// </summary>
    public class MultiMimeFormat
    {
        public MultiMimeFormat()
        {
        }

        public MultiMimeFormat(Action<MultiMimeFormat> initializer)
        {
            initializer(this);
        }

        private readonly IDictionary<string, Func<ActionResult>> _respondersByMime = new Dictionary<string, Func<ActionResult>>();


        /// <summary>
        /// Returns ActionResult for a particular MIME type
        /// </summary>
        /// <param name="mimeTypes">MIME types to return ActionResult for.</param>
        /// <returns></returns>
        public ActionResult ResultFor(params string[] mimeTypes)
        {

            var responder = (from mimeType in mimeTypes
                             where _respondersByMime.ContainsKey(mimeType)
                             select _respondersByMime[mimeType]).FirstOrDefault();
            if (responder == null) throw new HttpException(406, "Not Acceptable.");
            return responder();
        }

        /// <summary>
        /// Returns ActionResult for controller context.
        /// </summary>
        /// <param name="context">Context of the controller.</param>
        /// <returns></returns>
        public ActionResult ResultFor(ControllerContext context)
        {
            return ResultFor(context.HttpContext.Request.AcceptTypes);
        }

        /// <summary>
        /// Registers a responder for a supplied MIME type.
        /// </summary>
        /// <param name="mimeType">MIME type specific for the responder. Sample: text/html, application/json</param>
        /// <param name="responder">Just a function that returns ActionResult appropriate for the MIME</param>
        /// <param name="canRespondToAll">true means that responder can respond to */*</param>
        public void Mime(string mimeType, Func<ActionResult> responder, bool canRespondToAll = false)
        {
            _respondersByMime.Add(mimeType, responder);
            if (canRespondToAll) _respondersByMime.Add("*/*", responder);
        }

        /// <summary>
        /// Registers a responder for text/html MIME type. Also handle */*.
        /// </summary>
        /// <param name="responder"></param>
        public void Html(Func<ActionResult> responder)
        {
            Mime("text/html", responder, true);
        }

        /// <summary>
        /// Registers a responder for application/json MIME type.
        /// </summary>
        /// <param name="responder"></param>
        public void Json(Func<ActionResult> responder)
        {
            Mime("application/json", responder);
        }

        /// <summary>
        /// Registers a responder for application/xml MIME type.
        /// </summary>
        /// <param name="responder"></param>
        public void Xml(Func<ActionResult> responder)
        {
            Mime("application/xml", responder);
        }
    }
}