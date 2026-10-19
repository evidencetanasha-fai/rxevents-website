using System;

namespace RxEvents.Models
{
    public class Setting
    {
        public int SettingID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}