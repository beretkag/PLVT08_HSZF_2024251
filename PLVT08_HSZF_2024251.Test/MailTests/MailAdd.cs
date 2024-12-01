using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.MailTests
{
    [TestFixture]
    internal class MailAdd
    {
        private Mock<IMailProvider> mailProvider;
        private IMailService mailService;

        [SetUp]
        public void Init()
        {
            mailProvider = new Mock<IMailProvider>(MockBehavior.Strict);
            mailService = new MailService(mailProvider.Object);

            mailProvider.Setup(x => x.Add(It.IsAny<Mail>())).Returns((Mail x) => x).Verifiable();
        }

        [Test]
        public void AddTest()
        {
            Mail mail = TestData.mails.First();

            mailService.Add(mail);
            mailProvider.Verify(x => x.Add(mail), Times.Once());
        }
    }
}
