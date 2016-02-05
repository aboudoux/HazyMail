using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Hazymail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HazymailTest
{
    [TestClass]
    public class ParameterTest
    {
        public static string[] GetCommandArgument(string argumentsLine)
        {
            string str = argumentsLine.Trim();
            if (str == null || !(str.Length > 0)) return new string[0];
            int idx = str.IndexOf(" ");
            if (idx == -1) return new[] { str.Trim('"') };
            int count = str.Length;
            var list = new ArrayList();
            while (count > 0)
            {
                if (str[0] == '"')
                {
                    int temp = str.IndexOf("\"", 1, str.Length - 1);
                    while (temp > 0 && temp < str.Length && str[temp - 1] == '\\')
                    {
                        temp = str.IndexOf("\"", temp + 1, str.Length - temp - 1);
                    }
                    idx = temp + 1;
                }

                if (idx == 0)
                    break;

                string s = str.Substring(0, idx);
                int left = count - idx;
                str = str.Substring(idx, left).Trim();
                list.Add(s.Trim('"'));
                count = str.Length;
                idx = str.IndexOf(" ");
                if (idx == -1)
                {
                    string add = str.Trim('"', ' ');
                    if (add.Length > 0)
                    {
                        list.Add(add);
                    }
                    break;
                }
            }
            return (string[])list.ToArray(typeof(string));
        }      

        [TestMethod]
        public void TestSimpleParameters()
        {
            var args = GetCommandArgument("-s TEST -sn \"Aurélien BOUDOUX\" -sa aurelien@boudoux.fr");
            var parameters = new Parameters(args);

            Assert.AreEqual("TEST", parameters.Subject);
            Assert.AreEqual("Aurélien BOUDOUX", parameters.SenderName);
            Assert.AreEqual("aurelien@boudoux.fr", parameters.SenderAddress);
        }

        [TestMethod]
        public void RecipientAddressTest()
        {
            var addr = new RecipientAddress(" Aurélien BOUDOUX < aurelien@boudoux.fr > ");
            Assert.AreEqual(addr.Name,"Aurélien BOUDOUX");
            Assert.AreEqual(addr.Email, "aurelien@boudoux.fr");
        }

        [TestMethod]
        public void MultipleRecipientTest()
        {
            var args = GetCommandArgument("-ra \"Test account<test@test.com>;ROOT<root@test.com>\"");
            var param = new Parameters(args);
            Assert.AreEqual(2, param.Recipients.Count());
            Assert.IsTrue(param.Recipients.Any(a=>a.Name == "Test account"));
            Assert.IsTrue(param.Recipients.Any(a=>a.Name == "ROOT"));
            Assert.IsTrue(param.Recipients.Any(a => a.Email == "test@test.com"));
            Assert.IsTrue(param.Recipients.Any(a => a.Email == "root@test.com"));
        }
     
        [TestMethod]
        public void TestParamWitoutParameters()
        {
            var args = GetCommandArgument("-s");
            var param = new Parameters(args);
            Assert.AreEqual("", param.Subject);
        }

        [TestMethod]
        public void TestParamWitoutParameters2()
        {
            var args = GetCommandArgument("-s -ra");
            var param = new Parameters(args);
            Assert.AreEqual("", param.Subject);
            Assert.AreEqual(0, param.Recipients.Count());
        }

        [TestMethod]
        public void TestParamBodyText()
        {
            var args = GetCommandArgument("-b \"ceci est un test de malade mental ! ! !\"");
            var param = new Parameters(args);
            Assert.AreEqual("ceci est un test de malade mental ! ! !", param.Body);
        }

        [TestMethod]
        public void TestParamBodyFile()
        {
            using (var tmp = new TempFile(".txt"))
            {
                File.WriteAllText(tmp.FilePath, "bonjour, je suis en train de faire un test");
                var args = GetCommandArgument("-b \""+tmp.FilePath+"\"");
                var param = new Parameters(args);
                Assert.AreEqual("bonjour, je suis en train de faire un test", param.Body);
            }
        }

        [TestMethod]
        public void TestEnableSSl()
        {
            var args = GetCommandArgument("-ssl");
            var param = new Parameters(args);
            Assert.IsTrue(param.EnableSsl);
        }
    }
}
