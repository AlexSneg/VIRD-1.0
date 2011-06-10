using System.ComponentModel;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public class SoftwareSourceAggregate
    {
        [Browsable(false)]
        public object Plugin { get; set; }

        [Browsable(false)]
        public object Source { get; set; }

        [DisplayName("Плагин включен")]
        [TypeConverter("TechnicalServices.Common.TypeConverters.YesNoConverter, TechnicalServices.Common")]
        public bool Enabled
        {
            get { return ((PluginAdapter) Plugin).Enabled; }
            set { ((PluginAdapter) Plugin).Enabled = value; }
        }
    }
}