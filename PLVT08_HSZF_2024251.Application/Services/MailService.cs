﻿using PLVT08_HSZF_2024251.Model;
using PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders;

namespace PLVT08_HSZF_2024251.Application.Services
{
    public interface IMailService
    {
        public Mail Add(Mail mail);
        public ICollection<Mail> GetByFilter(Func<Mail, bool> filter);
        public void Delete(Mail mail);

    }


    public class MailService : IMailService
    {
        private readonly IMailProvider mailProvider;

        public MailService(IMailProvider mailProvider)
        {
            this.mailProvider = mailProvider;
        }

        public Mail Add(Mail mail)
        {
            return mailProvider.Add(mail);
        }

        public ICollection<Mail> GetByFilter(Func<Mail, bool> filter)
        {
            return mailProvider.GetAll().Where(filter).ToHashSet();
        }
        public void Delete(Mail mail)
        {
            mailProvider.DeleteById(mail.Id);
        }
    }
}
