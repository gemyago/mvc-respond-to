using System.Collections.Generic;
using System.Web.Mvc;
using Mvc.RespondTo.MultiMime;
using NUnit.Framework;

namespace Mvc.RespondTo.Tests.Samples
{
    [TestFixture]
    public class PeopleControllerTest
    {
        [Test]
        public void TestIndex()
        {
            var controller = new PeopleController();
            var result = controller.Index();
            Assert.That(result, Is.InstanceOf<MultiMimeResult>());
            var multiMimeResult = (MultiMimeResult)result;

            var jsonResult = (JsonResult)multiMimeResult.Format.ResultFor("application/json");
            Assert.That(jsonResult.Data, Is.Not.Null);
            Assert.That(jsonResult.Data, Is.InstanceOf<IEnumerable<string>>());

            var viewResult = (ViewResult)multiMimeResult.Format.ResultFor("text/html");
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.InstanceOf<IEnumerable<string>>());

            var textResult = (ContentResult)multiMimeResult.Format.ResultFor("text/plain");
            Assert.That(textResult.Content, Is.EqualTo("Larry, Garry, Sam"));
        }
    }
}