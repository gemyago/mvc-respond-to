using System;
using System.Web.Mvc;
using Mvc.RespondTo.MultiMime;

namespace Mvc.RespondTo.Tests.Samples
{
    public static class MultiMimeFormatExtensions
    {
        public static void Pdf(this MultiMimeFormat format, Func<ActionResult> responder)
        {
            format.WithResolveHook(context =>
            {
                if (context.HttpContext.Request.QueryString["pdf"] == "true") return format.ResolveResult("application/pdf");
                return null;
            });
            format.Mime("application/pdf", responder);
        }
    }
}