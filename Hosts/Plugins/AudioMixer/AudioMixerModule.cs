using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.AudioMixer.Configurator;
using Hosts.Plugins.AudioMixer.Designer;
using Hosts.Plugins.AudioMixer.Player;
using Hosts.Plugins.AudioMixer.Server;
using Hosts.Plugins.AudioMixer.SystemModule;
using Hosts.Plugins.AudioMixer.Visualizator;

namespace Hosts.Plugins.AudioMixer
{
    public sealed class AudioMixerModule :
        ModuleGeneric
            <AudioMixerSystemModule, AudioMixerDesignerModule, AudioMixerConfiguratorModule,
            AudioMixerVisualizatorModule, AudioMixerServerModule, AudioMixerPlayerModule>
    {
    }
}