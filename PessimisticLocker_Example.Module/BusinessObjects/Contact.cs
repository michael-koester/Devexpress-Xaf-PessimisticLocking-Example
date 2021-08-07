using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using PessimisticLocker;

namespace PessimisticLocker_Example.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contact")]
    [OptimisticLocking(false)]
    public class Contact : BaseObject, IPessimisticLocking
    {
        public string Id => Oid.ToString();
        public Contact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }

        private string note;
        [Size(200)]
        public string Note
        {
            get { return note; }
            set { SetPropertyValue(nameof(Note), ref note, value); }
        }
        
    }
}