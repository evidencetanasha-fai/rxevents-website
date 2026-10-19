using System;
using System.Collections.Generic;
using RxEvents.Data;
using RxEvents.Models;

namespace RxEvents
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContactSettings();
            }
        }

        private void LoadContactSettings()
        {
            try
            {
                SettingsRepository repository = new SettingsRepository();
                List<Setting> settings = repository.GetAllSettings();

                foreach (Setting setting in settings)
                {
                    switch (setting.SettingName)
                    {
                        case "Phone":
                            PhoneLabel.Text = setting.SettingValue;
                            break;

                        case "Email":
                            EmailLabel.Text = setting.SettingValue;
                            break;

                        case "Location":
                            LocationLabel.Text = setting.SettingValue;
                            break;

                        case "BusinessHours":
                            BusinessHoursLabel.Text = setting.SettingValue;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageLabel.Text =
                    "Unable to load contact information: " + ex.Message;
            }
        }

        protected void SendMessageButton_Click(object sender, EventArgs e)
        {
            MessageLabel.Text =
                "Thank you! Your message has been received.";
        }
    }
}