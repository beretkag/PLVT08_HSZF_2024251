using Moq;
using PLVT08_HSZF_2024251.Application.Services;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLVT08_HSZF_2024251.Test.MailTests
{
    [TestFixture]
    internal class MailGetByFilter
    {
        private Mock<IMailProvider> mailProvider;
        private IMailService mailService;

        [SetUp]
        public void Init()
        {
            mailProvider = new Mock<IMailProvider>(MockBehavior.Strict);
            mailService = new MailService(mailProvider.Object);

            mailProvider.Setup(x => x.GetAll()).Returns(TestData.mails).Verifiable();
        }

        [Test]
        public void TestGetByFilter()
        {
            var mails = mailService.GetByFilter(x => x.PersonId == "0");
            mailProvider.Verify(x => x.GetAll(), Times.Once());
            Assert.That(() => mails.Count, Is.EqualTo(1));
            Assert.That(mails.First().PersonId, Is.EqualTo("0"));
        }
    }
}
