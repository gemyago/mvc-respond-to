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
}