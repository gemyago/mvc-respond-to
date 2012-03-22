using System;
using System.Web.Mvc;
using Mvc.RespondTo.MultiMime;

namespace Mvc.RespondTo
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Respond to different mime types with different ActionResult.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ActionResult RespondTo(this ControllerBase controller, Action<MultiMimeFormat> format)
        {
            return new MultiMimeResult(new MultiMimeFormat(format));
        }
    }
}