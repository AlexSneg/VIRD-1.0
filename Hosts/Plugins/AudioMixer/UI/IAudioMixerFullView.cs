using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hosts.Plugins.AudioMixer.SystemModule.Config;

namespace Hosts.Plugins.AudioMixer.UI
{
    public interface IAudioMixerFullView
    {
        /// <summary>расчетная ширина контрола, чтобы не было скролинга </summary>
        int GetWidthControl(int countInputs, int countFaders);
        /// <summary>обновить матрицу </summary>
        void UpdateMixerTies(bool[][] tiesState);
        /// <summary>обновить значение фейдера </summary>
        void UpdateFaderValue(int instanceId, bool mute, int level);
        /// <summary>инициализация матрицы входов/выходов </summary>
        void InitializeMatrix(bool hasMatrix, int matrixId,
            List<AudioMixerInput> inputs, 
            List<AudioMixerOutput> outputs, 
            Func<AudioMixerInput, AudioMixerOutput, bool> getMatrixUnit);
        /// <summary>инициализация фейдеров </summary>
        void InitializeFaderGroups(List<AudioMixerFaderGroupDesign> groups);
        /// <summary>на контроле пользователь нажал кнопку с командой</summary>
        event Action<string, IConvertible[]> PushCommandButtonEvent;
        /// <summary>возвращает матрицу сформированную по тому же принципу что и в AudioMixerDeviceDesign.Matrix </summary>
        List<AudioMixerMatrixUnit> GetMatrixState { get; }
        /// <summary>возвращает состояние всех фейдеров </summary>
        List<AudioMixerFaderDesign> GetFadersState { get; }
        /// <summary>
        /// Сохраняет настройки фэйдеров.
        /// </summary>
        void SaveFaders(List<AudioMixerFaderGroupDesign> groups);
    }
}
