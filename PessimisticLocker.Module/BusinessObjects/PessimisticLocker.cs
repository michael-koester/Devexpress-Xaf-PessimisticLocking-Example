using System;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace PessimisticLocker
{
    [MemberDesignTimeVisibility(false)]
    [OptimisticLocking(false)]
    public class PessimisticLocker : BaseObject
    {
        public PessimisticLocker(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            date = DateTime.Now;
        }

        private string objectOid;
        public string ObjectOid
        {
            get { return objectOid; }
            set { objectOid = value; }
        }

        private string type;
        [Size(256)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private DateTime date;
        public DateTime CreationDate
        {
            get { return date; }
            set { date = value; }
        }

        private PermissionPolicyUser owner;
        public PermissionPolicyUser Owner 
        { 
            get { return owner; } 
            set { owner = value; }
        }
    }
}
