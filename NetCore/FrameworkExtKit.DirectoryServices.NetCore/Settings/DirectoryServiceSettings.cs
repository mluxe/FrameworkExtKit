using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices.Settings {
    public class DirectoryServiceSettingItem {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string ObjectClass { get; set; }
        public bool Enabled { get; set; }
    }

    public class DirectoryServiceSettings {
        public DirectoryServiceSettingItem AccountService { get; set; }
        public DirectoryServiceSettingItem DistributionListService { get; set; }
        public DirectoryServiceSettingItem GroupService { get; set; }
        public DirectoryServiceSettingItem RoleAccountService { get; set; }
        public DirectoryServiceSettingItem BuildingService { get; set; }
    }
}
