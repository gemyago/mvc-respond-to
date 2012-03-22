using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Mvc.RespondTo.MultiMime;
using NUnit.Framework;

namespace Mvc.RespondTo.Tests
{
    [TestFixture]
    public class MultiMimeFormatTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _format = new MultiMimeFormat();
        }

        #endregion

        private MultiMimeFormat _format;

        [Test]
        public void TestMime()
        {
            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml);
            Assert.That(_format.ResultFor("text/html"), Is.EqualTo(textHtml));
            var exception = Assert.Throws<HttpException>(() => _format.ResultFor("application/json"));
            Assert.That(exception.GetHttpCode(), Is.EqualTo(406));
        }

        [Test]
        public void TestMimeAll()
        {
            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml, true);
            Assert.That(_format.ResultFor("*/*", "application/xml"), Is.EqualTo(textHtml));
        }

        [Test]
        public void TestResultForContext()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpRequest = new Mock<HttpRequestBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpRequest.Setup(ar => ar.AcceptTypes).Returns(new[] {"text/html"});

            var requestContext = new RequestContext {HttpContext = httpContext.Object};
            var controllerContext = new ControllerContext {RequestContext = requestContext};

            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml);
            Assert.That(_format.ResultFor(controllerContext), Is.EqualTo(textHtml));
        }

        [Test]
        public void TestResultForMimeReturnsFirstRegistered()
        {
            var textHtml = new ViewResult();
            var applicationXml = new ContentResult();
            _format.Mime("text/html", () => textHtml);
            _format.Mime("application/xml", () => applicationXml);
            Assert.That(_format.ResultFor("text/html", "application/xml"), Is.EqualTo(textHtml));
        }
    }
}