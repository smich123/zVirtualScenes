//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zvsModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    public partial class device_types
    {
        public device_types()
        {
            this.device_type_commands = new ObservableCollection<device_type_commands>();
            this.devices = new ObservableCollection<device>();
        }
    
        public int id { get; set; }
        public int plugin_id { get; set; }
        public string name { get; set; }
        public bool show_in_list { get; set; }
        public string friendly_name { get; set; }
    
        public virtual ObservableCollection<device_type_commands> device_type_commands { get; set; }
        public virtual plugin plugin { get; set; }
        public virtual ObservableCollection<device> devices { get; set; }
    }
}
