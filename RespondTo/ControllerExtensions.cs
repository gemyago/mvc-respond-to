using System;
using System.Web.Mvc;

namespace Mvc.RespondTo
{
    public static class ControllerExtensions
    {
        public static ActionResult RespondTo(this ControllerBase controller, Action<MultiMimeFormat> format)
        {
            return new MultiMimeResult(new MultiMimeFormat(format));
        }
    }

    public class MultiMimeResult : ActionResult
    {
        public readonly MultiMimeFormat Format;

        public MultiMimeResult(MultiMimeFormat format)
        {
            Format = format;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Format.ResultFor(context).ExecuteResult(context);
        }
    }
}