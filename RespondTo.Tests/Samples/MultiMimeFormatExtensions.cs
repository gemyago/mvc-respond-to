using System;
using System.Web.Mvc;
using Mvc.RespondTo.MultiMime;

namespace Mvc.RespondTo.Tests.Samples
{
    public static class MultiMimeFormatExtensions
    {
        public static void Pdf(this MultiMimeFormat format, Func<ActionResult> responder)
        {
            format.Mime("text/pdf", responder);
        }
    }
}