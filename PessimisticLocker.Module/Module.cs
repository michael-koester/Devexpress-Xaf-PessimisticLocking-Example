using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;

namespace PessimisticLocker.Module
{
    public sealed partial class PessimisticLockerModule : ModuleBase 
    {
        public PessimisticLockerModule() {
            InitializeComponent();
            this.AdditionalExportedTypes.Add(typeof(PessimisticLocker));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) 
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) 
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
