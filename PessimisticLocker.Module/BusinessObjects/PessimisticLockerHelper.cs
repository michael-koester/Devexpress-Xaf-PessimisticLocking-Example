using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.Collections.Generic;
using System.Linq;

namespace PessimisticLocker
{
    public static class PessimisticLockerHelper
    {
        public static PessimisticLocker GetLocker(IObjectSpace os, IPessimisticLocking obj, PermissionPolicyUser holder)
        {
            if (obj == null) return null;
            return os.FindObject<PessimisticLocker>(
                CriteriaOperator.Parse("ObjectOid = ? And Type = ? and Owner.Oid = ?",
                obj.Id, obj.GetType().ToString(), holder.Oid));
        }
        public static void Lock(IObjectSpace os, IPessimisticLocking obj, PermissionPolicyUser holder)
        {
            PessimisticLocker locker = GetLocker(os, obj, holder);
            if (locker == null)
            {
                locker = os.CreateObject<PessimisticLocker>();
                locker.Owner = holder;
                locker.Type = obj.GetType().ToString();
                locker.ObjectOid = obj.Id;
                locker.Save();
                os.CommitChanges();
            }
        }
        public static void Unlock(IObjectSpace os, IPessimisticLocking obj, PermissionPolicyUser holder)
        {
                PessimisticLocker locker = GetLocker(os, obj, holder);
                if (locker != null)
                {
                    locker.Delete();
                    os.CommitChanges();
                }
        }
        public static void UnlockAllButMe(IObjectSpace os, IPessimisticLocking obj, PermissionPolicyUser holder)
        {
            var theobj = os.GetObjectByKey(obj.GetType(), obj.Id);
            if (theobj == null) return;
            PermissionPolicyUser lockingHolder = os.GetObject(holder);
            List<PessimisticLocker> allLockers = GetAllLockers(os, obj);
            foreach (PessimisticLocker locker in allLockers)
                if (locker.Owner != lockingHolder) locker.Delete();
            os.CommitChanges();
        }

        public static List<PessimisticLocker> GetAllLockers(IObjectSpace os, IPessimisticLocking obj)
        {
            if (obj == null) return new List<PessimisticLocker>();
            CriteriaOperator criteria = CriteriaOperator.Parse("ObjectOid = ? And Type = ?", obj.Id, obj.GetType().ToString());
            return os.GetObjects<PessimisticLocker>(criteria).ToList();
        }
        public static void Lock(IObjectSpace os, IPessimisticLocking obj)
        {
            Lock(os, obj, os.GetObject(SecuritySystem.CurrentUser as PermissionPolicyUser));
        }
        public static void Unlock(IObjectSpace os, IPessimisticLocking obj)
        {
            Unlock(os, obj, os.GetObject(SecuritySystem.CurrentUser as PermissionPolicyUser));
        }
        public static void UnlockAllButMe(IObjectSpace os, IPessimisticLocking obj)
        {
            UnlockAllButMe(os, obj, os.GetObject(SecuritySystem.CurrentUser as PermissionPolicyUser));
        }
    }
}
