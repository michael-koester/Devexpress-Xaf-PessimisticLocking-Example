using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using PessimisticLocker_Example.Module.BusinessObjects;

namespace PessimisticLocker_Example.Module
{
    public sealed partial class PessimisticLocker_ExampleModule : ModuleBase {
        public PessimisticLocker_ExampleModule() {
            InitializeComponent();
            this.AdditionalExportedTypes.Add(typeof(Contact));
            this.AdditionalExportedTypes.Add(typeof(Company));
            this.RequiredModuleTypes.Add(typeof(PessimisticLocker.Module.PessimisticLockerModule));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
