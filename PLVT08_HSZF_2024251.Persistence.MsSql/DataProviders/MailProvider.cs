using PLVT08_HSZF_2024251.Model;


namespace PLVT08_HSZF_2024251.Persistence.MsSql.DataProviders
{
    public interface IMailProvider
    {
        public Mail Add(Mail mail);                     // C
        public ICollection<Mail> GetAll();              // R
        public void Update(int id, Mail mail);          // U
        public void DeleteById(int id);                 // D
    }

    public class MailProvider : IMailProvider
    {
        //FIELDS
        private readonly HouseholdContext householdContext;

        //CONSTRUCTORS
        public MailProvider(HouseholdContext householdContext)
        {
            this.householdContext = householdContext;
        }

        //METHODS
        public Mail Add(Mail mail)
        {
            Mail newMail = householdContext.Mails.Add(mail).Entity;
            householdContext.SaveChanges();
            return newMail;
        }
        public ICollection<Mail> GetAll()
        {
            return householdContext.Mails.ToHashSet();
        }

        public void Update(int id, Mail mail)
        {
            Mail updating = householdContext.Mails.First(x => x.Id == id);
            updating = mail;
            householdContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Mail deleting = householdContext.Mails.First(x => x.Id == id);
            householdContext.Remove(deleting);
            householdContext.SaveChanges();
        }
    }
}
