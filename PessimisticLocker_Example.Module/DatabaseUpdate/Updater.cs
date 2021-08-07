using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace PessimisticLocker_Example.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            PermissionPolicyUser user = ObjectSpace.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("UserName = 'admin'"));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<PermissionPolicyUser>();
                user.UserName = "admin";
                PermissionPolicyRole role = ObjectSpace.CreateObject<PermissionPolicyRole>();
                role.IsAdministrative = true;
                user.Roles.Add(role);
                user.Save();
                user = ObjectSpace.CreateObject<PermissionPolicyUser>();
                user.UserName = "sam";
                user.Roles.Add(role);
                ObjectSpace.CommitChanges();
            }
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
