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
    internal class MailDelete
    {
        private Mock<IMailProvider> mailProvider;
        private IMailService mailService;

        [SetUp]
        public void Init()
        {
            mailProvider = new Mock<IMailProvider>(MockBehavior.Strict);
            mailService = new MailService(mailProvider.Object);

            mailProvider.Setup(x => x.DeleteById(It.IsAny<int>())).Verifiable();
        }

        [Test]
        public void DeleteTest()
        {
            Mail mail = TestData.mails.First();

            mailService.Delete(mail);
            mailProvider.Verify(x => x.DeleteById(mail.Id), Times.Once());
        }
    }
}
