using System;
using System.ComponentModel;

using TechnicalServices.Persistence.SystemPersistence.Configuration;

namespace UI.PresentationDesign.ConfiguratorUI.Controller
{
    public class CompatibilityDispToSourcePair
    {
        private readonly Func<Mapping> _createMappingFunc;
        protected readonly DisplayType _disp;

        private readonly Func<Mapping, SourceType, bool> _linkPredicate;
        private readonly SourceType Source;
        private bool _Compatible;

        protected internal CompatibilityDispToSourcePair(DisplayType disp, SourceType source,
                                                         Func<Mapping, SourceType, bool> linkPredicate,
                                                         Func<Mapping> createMappingFunc)
        {
            _linkPredicate = linkPredicate;
            _createMappingFunc = createMappingFunc;
            Source = source;
            _disp = disp;
            _Compatible = _disp.MappingList.Exists(m => _linkPredicate(m, Source));
        }

        [DisplayName("Ок")]
        public bool Compatible
        {
            get { return _Compatible; }
            set
            {
                bool needSignal = _Compatible != value;
                _Compatible = value;
                if (needSignal)
                    if (_Compatible)
                        _disp.MappingList.Add(_createMappingFunc());
                    else
                        _disp.MappingList.RemoveAll(m => _linkPredicate(m, Source));
            }
        }

        [DisplayName("Аппаратный источник")]
        public virtual string Name
        {
            get { return Source.Name; }
        }
    }
}