using System.Web.Mvc;
using Moq;
using Mvc.RespondTo.MultiMime;
using NUnit.Framework;

namespace Mvc.RespondTo.Tests
{
    [TestFixture]
    public class ControllerExtensionsTest
    {
        [Test]
        public void TestRespondTo()
        {
            MultiMimeFormat expectedFormat = null;
            var actionResult = new Mock<ControllerBase>().Object.RespondTo(format => { expectedFormat = format; });
            Assert.That(actionResult, Is.InstanceOf<MultiMimeResult>());
            Assert.That(((MultiMimeResult)actionResult).Format, Is.EqualTo(expectedFormat));
        }
    }
}