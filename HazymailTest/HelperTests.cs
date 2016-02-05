using System;
using System.IO;
using System.Linq;
using Hazymail;
using Hazymail.Exceptions;
using Hazymail.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HazymailTest
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void ExtractDomainTest()
        {
            var result = MessageHelper.ExtractDomainFromEmail("aurelien@boudoux.fr");
            Assert.AreEqual("boudoux.fr", result);
        }

        [TestMethod]
        public void ExtractDomainWithSpace()
        {
            var result = MessageHelper.ExtractDomainFromEmail(" aurelien@boudoux.fr ");
            Assert.AreEqual("boudoux.fr", result);
        }

        [TestMethod]
        [ExpectedException(typeof(EmailNotValidException))]
        public void ExtractDomainWithoutAt()
        {
            var result = MessageHelper.ExtractDomainFromEmail("aurelienboudoux.fr");
        }

        [TestMethod]
        public void CreateMessageTest()
        {
            var param = new Parameters(ParameterTest.GetCommandArgument("-sn BOUDOUX -sa aurelien@boudoux.fr -ra test@test.com -s TEST -b ok"));
            var message = MessageHelper.CreateMessage(param, param.Recipients.First());
            Assert.AreEqual("TEST", message.Subject);
            Assert.AreEqual(1, message.To.Count);
            Assert.AreEqual("test@test.com", message.To.First().Address);
            Assert.AreEqual("", message.To.First().DisplayName);
            Assert.AreEqual("ok", message.Body);
        }
        

        [TestMethod]
        [ExpectedException(typeof(EmailNotValidException))]
        public void MultipleRecipientWithoutCommat()
        {
            var args = ParameterTest.GetCommandArgument("-ra \"Test account<test@test.com>ROOT<root@test.com>\"");
            var param = new Parameters(args);
            var message = MessageHelper.CreateMessage(param, param.Recipients.First());
        }

        [TestMethod]
        public void CreateMessageWithAttachementTest()
        {
            using (var t = new TempFile(".txt")) {
                File.AppendAllText(t.FilePath, "ceci est un test");
                var param = new Parameters(ParameterTest.GetCommandArgument("-sn BOUDOUX -sa aurelien@boudoux.fr -ra test@test.com -s TEST -b ok -a " + t.FilePath));
                var message = MessageHelper.CreateMessage(param, param.Recipients.First());
                Assert.AreEqual(1, message.To.Count);
                Assert.AreEqual("test@test.com", message.To.First().Address);
                Assert.AreEqual("", message.To.First().DisplayName);
                Assert.AreEqual("ok", message.Body);
                Assert.AreEqual(1,message.Attachments.Count);                
            }                       
        }

        [TestMethod]
        [ExpectedException(typeof(AttachementFileNotFoundException))]
        public void CreateMessageWithAttachementErrorTest()
        {
            var param = new Parameters(ParameterTest.GetCommandArgument("-sn BOUDOUX -sa aurelien@boudoux.fr -ra test@test.com -s TEST -b ok -a c:\\test_attachement.tes"));
        }
    }
}
