using System;
using System.Web.Mvc;

namespace Mvc.RespondTo.MultiMime
{
    public class MultiMimeResult : ActionResult
    {
        public readonly MultiMimeFormat Format;

        public MultiMimeResult(Action<MultiMimeFormat> format)
        {
            Format = new MultiMimeFormat(format);
        }

        public MultiMimeResult(MultiMimeFormat format)
        {
            Format = format;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Format.ResolveResult(context).ExecuteResult(context);
        }
    }
}