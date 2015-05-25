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
            _httpContext = new Mock<HttpContextBase>();
            _httpRequest = new Mock<HttpRequestBase>();
            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            var requestContext = new RequestContext { HttpContext = _httpContext.Object };
            _controllerContext = new ControllerContext { RequestContext = requestContext };
        }

        #endregion

        private MultiMimeFormat _format;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _httpRequest;
        private ControllerContext _controllerContext;

        [Test]
        public void TestMime()
        {
            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml);
            Assert.That(_format.ResolveResult("text/html"), Is.EqualTo(textHtml));
            var exception = Assert.Throws<HttpException>(() => _format.ResolveResult("application/json"));
            Assert.That(exception.GetHttpCode(), Is.EqualTo(406));
        }
        
        [Test]
        public void TestNullMime()
        {
            var exception = Assert.Throws<HttpException>(() => _format.ResolveResult((string[]) null));
            Assert.That(exception.GetHttpCode(), Is.EqualTo(406));
        }

        [Test]
        public void TestMimeAll()
        {
            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml, true);
            Assert.That(_format.ResolveResult("*/*", "application/xml"), Is.EqualTo(textHtml));
        }

        [Test]
        public void TestResultForContext()
        {
            _httpRequest.Setup(ar => ar.AcceptTypes).Returns(new[] { "text/html" });
            var textHtml = new ViewResult();
            _format.Mime("text/html", () => textHtml);
            Assert.That(_format.ResolveResult(_controllerContext), Is.EqualTo(textHtml));
        }

        [Test]
        public void TestWithResolveHook()
        {
            var hookInvoked = false;
            var hookResult = new EmptyResult();
            _format.WithResolveHook(context =>
            {
                hookInvoked = true;
                return hookResult;
            });
            Assert.That(_format.ResolveResult(_controllerContext), Is.SameAs(hookResult));
            Assert.That(hookInvoked);
        }

        [Test]
        public void TestWithResolveHookDefaultFallback()
        {
            _format.WithResolveHook(context => null);
            _format.Html(() => new ViewResult());
            _httpRequest.Setup(ar => ar.AcceptTypes).Returns(new[] { "text/html" });
            Assert.That(_format.ResolveResult(_controllerContext), Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void TestResultForMimeReturnsFirstRegistered()
        {
            var textHtml = new ViewResult();
            var applicationXml = new ContentResult();
            _format.Mime("text/html", () => textHtml);
            _format.Mime("application/xml", () => applicationXml);
            Assert.That(_format.ResolveResult("text/html", "application/xml"), Is.EqualTo(textHtml));
        }
    }
}