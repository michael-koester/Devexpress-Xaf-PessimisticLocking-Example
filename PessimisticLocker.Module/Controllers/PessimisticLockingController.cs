using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace PessimisticLocker.Module.Controllers
{
    public partial class PessimisticLockingController : ViewController
    {
        public PessimisticLockingController()
        {
            InitializeComponent();
            TargetObjectType = typeof(IPessimisticLocking);
            TargetViewType = ViewType.DetailView;
        }
        
        private List<PessimisticLocker> lockers = null;
        private bool lockedByOtherUser = true;
        protected override void OnActivated()
        {
            base.OnActivated();
            if (!ObjectSpace.IsNewObject(View.CurrentObject))
                CheckLockingByUser();
        }
        private void CheckLockingByUser()
        {
            lockers = PessimisticLockerHelper.GetAllLockers(ObjectSpace, ((IPessimisticLocking)View.CurrentObject));
            if (lockers.Count == 0)
            {
                PessimisticLockerHelper.Lock(ObjectSpace, ((IPessimisticLocking)View.CurrentObject));
                lockedByOtherUser = false;
            }
            else
            if (lockers.Count > 0)
            {
                if (lockers[0].Owner.Oid == (Guid)SecuritySystem.CurrentUserId)
                    lockedByOtherUser = false;
                else
                    Application.ShowViewStrategy.ShowMessage(((IPessimisticLocking)View.CurrentObject).GetType().ToString() + " is used by " + lockers[0].Owner.UserName);
            }
            View.AllowEdit["CanEdit"] = !lockedByOtherUser;
            if (lockedByOtherUser)
                View.Caption += string.Format(" [Locked by {0} at {1}]", lockers[0].Owner.UserName, lockers[0].CreationDate);
        }
        protected override void OnDeactivated()
        {
            if (View.AllowEdit["CanEdit"] == true)
                PessimisticLockerHelper.Unlock(ObjectSpace, (IPessimisticLocking)View.CurrentObject);
            lockers = null;
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            UnlockAction.Enabled["IsLocked"] = !View.AllowEdit["CanEdit"];
        }
        private void UnlockAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PessimisticLockerHelper.UnlockAllButMe(ObjectSpace, (IPessimisticLocking)View.CurrentObject);
            View.AllowEdit["CanEdit"] = true;
            View.Refresh();
            Tracing.Tracer.LogText(string.Format("{0} {1} unlocked from {2}", View.CurrentObject.GetType().ToString(),((IPessimisticLocking)View.CurrentObject).Id, SecuritySystem.CurrentUserName));
        }
    }
}
