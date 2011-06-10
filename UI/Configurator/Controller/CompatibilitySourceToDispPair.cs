using System;
using System.ComponentModel;

using Hosts.Plugins.Jupiter.SystemModule.Config;

using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public class CompatibilitySourceToDispPair : CompatibilityDispToSourcePair
    {
        private readonly JupiterInOutConfig _jupiterIn;

        protected internal CompatibilitySourceToDispPair(DisplayType disp, JupiterInOutConfig jupiterIn,
                                                         SourceType source,
                                                         Func<Mapping, SourceType, bool> linkPredicate,
                                                         Func<Mapping> createMappingFunc)
            : base(disp, source, linkPredicate, createMappingFunc)
        {
            _jupiterIn = jupiterIn;
        }

        protected internal CompatibilitySourceToDispPair(DisplayType disp, SourceType source,
                                                         Func<Mapping, SourceType, bool> linkPredicate,
                                                         Func<Mapping> createMappingFunc)
            : base(disp, source, linkPredicate, createMappingFunc)
        {
            _jupiterIn = null;
        }

        [DisplayName("Пассивный дисплей/Вход видеостены")]
        public override string Name
        {
            get
            {
                return null != _jupiterIn
                           ? string.Format("{0}:Вход#{1}", _disp.Name, _jupiterIn.VideoIn)
                           : _disp.Name;
            }
        }
    }
}