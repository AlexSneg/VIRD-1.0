using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechnicalServices.Persistence.SystemPersistence.Presentation;
using TechnicalServices.Interfaces.ConfigModule.Player;
using TechnicalServices.Persistence.SystemPersistence.Configuration;
using TechnicalServices.Interfaces;
using TechnicalServices.Exceptions;
using TechnicalServices.Entity;
using Timer=System.Timers.Timer;

namespace DomainServices.EnvironmentConfiguration.ConfigModule.Player
{
    /// <summary>Общий контроллер для плагинов, умеет отправлять команды, 
    /// и может переодически обнолять view</summary>
    public abstract class PlayerPlaginsController<TDevice, TView>
        : IPlayerPlaginsController where TDevice : Device where TView : IPlayerPlagingBaseView
    {
        protected TView View;
        protected readonly object _timerSync = new object();
        protected IEventLogging Logging;
        protected IPlayerCommand PlayerCommand;
        protected TDevice Device;
        protected System.Timers.Timer _timer = null;

        public PlayerPlaginsController(TDevice device, IPlayerCommand playerCommand,
            IEventLogging logging, TView view)
        {
            View = view;
            PlayerCommand = playerCommand;
            Device = device;
            Logging = logging;
            //View.ControlPlayerTimerTickEvent += ViewControlTimerTickEvent;
            View.PushCommandButtonEvent += ViewPushCommandButtonEvent;
            _timer = new Timer(1000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
        }

     
        /// <summary>метод которые вызывается, когда работает таймер и контроллер должен переодически обновлять контрол </summary>
        protected abstract void UpdateView();
        
        public virtual void Dispose()
        {
            if (_timer != null)
            {
                _timer.Close();
                _timer = null;
            }
            //View.ControlPlayerTimerTickEvent -= ViewControlTimerTickEvent;
            View.PushCommandButtonEvent -= ViewPushCommandButtonEvent;
        }

        public bool? IsOnLine 
        {
            get { return PlayerCommand.IsOnLine(Device.Type); }
        }
        public bool IsShow 
        {
            get { return PlayerCommand.IsShow; }
        }

        public void SetControlPlayerTimerEnable(bool enable, int? millisec)
        {
            if (millisec.HasValue)
                _timer.Interval = millisec.Value;
            _timer.Enabled = enable;
        }

        /// <summary>реакция на отправку команды с контрола </summary>
        protected virtual void ViewPushCommandButtonEvent(string command, params IConvertible[] parameters)
        {
            bool isSuccess;
            ExecuteCommand(command, out isSuccess, parameters);
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ViewControlTimerTickEvent();
        }

        /// <summary>метод обработчик события от таймера, старайтесь не использовать в дочерних классах </summary>
        private void ViewControlTimerTickEvent()
        {
            if (Monitor.TryEnter(_timerSync))
            {
                try
                {
                    UpdateView();
                }
                finally
                {
                    Monitor.Exit(_timerSync);
                }
            }
        }
        /// <summary>при получении ответа на команду кристрона, ответ надо распарсить, ответ может различаться для разных типов оборудования </summary>
        protected virtual string ParseCommandAnswerParameter(string parameter, EquipmentCommand cmd)
        {
            if (parameter.IndexOf('(') == -1) return parameter;
            int startIndex = parameter.IndexOf('(') + 1;
            int length = parameter.IndexOf(')');
            length = length == -1 ? parameter.Length - 1 : length;
            length -= (parameter.IndexOf('(') + 1);
            string res = parameter.Substring(startIndex, length);
            return res;
        }

        /// <summary>выполняет указанную команду, без параметров и возвращает результат работы команды</summary>
        protected string ExecuteCommand(string commandName, out bool isSuccess, params IConvertible[] parameters)
        {
            try
            {
                if (Device == null)
                    throw new Exception("Object device is null");
                if (Device.Type == null)
                    throw new Exception("Object Device.Type is null");
                EquipmentCommand cmd = Device.Type.CommandList.Find(delegate(EquipmentCommand item) { return item.Name.Equals(commandName); });
                if (cmd == null)
                    throw new WrongHardwareCommand(string.Format("Wrong command name {0} for device {1}", commandName, Device.Type.UID));
                string result = PlayerCommand.DoEquipmentCommand(new CommandDescriptor(Device.Type.UID, cmd.Command, parameters));
                //если прописан ответ в конфиге проверим его
                if ((string.IsNullOrEmpty(result)) || (!result.StartsWith(cmd.Answer)))
                    throw new WrongHardwareCommandAnswer(string.Format("wrong answer for command:{0}, actual:{1}, expected:{2}", cmd.Command, result, cmd.Answer));
                result = ParseCommandAnswerParameter(result, cmd);
                isSuccess = true;
                return result;
            }
            catch(Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
                isSuccess = false;
                return string.Empty;
            }
        }

        /// <summary> выполняет команду которая должна вернуть int </summary>
        protected int ExecuteCommandInt32(string command, out bool isSuccess, params IConvertible[] parameters)
        {
            try
            {
                string result = ExecuteCommand(command, out isSuccess, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
                isSuccess = false;
                return -1;
            }
        }
        /// <summary> выполняет команду которая должна коллекцию строк </summary>
        protected string[] ExecuteCommandArrayString(string command, out bool isSuccess, params IConvertible[] parameters)
        {
            try
            {
                string answer = ExecuteCommand(command, out isSuccess, parameters);
                string[] items = answer.Split(',');
                return items.Select(item => item.Trim('"')).ToArray();
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
                isSuccess = false;
                return new string[] { };
            }
        }

        /// <summary> выполняет команду которая должна вернуть int </summary>
        protected int[] ExecuteCommandArrayInt32(string command, out bool isSuccess, params IConvertible[] parameters)
        {
            try
            {
                string answer = ExecuteCommand(command, out isSuccess, parameters);
                string[] items = answer.Split(',');
                int[] results = new Int32[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    results[i] = Convert.ToInt32(items[i]);
                }
                return results;
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
                isSuccess = false;
                return new Int32[] { };
            }
        }
        /// <summary> выполняет команду которая должна вернуть 0 или 1 </summary>
        protected bool ExecuteCommandBool(string command, out bool isSuccess, params IConvertible[] parameters)
        {
            try
            {
                string result = ExecuteCommand(command, out isSuccess, parameters);
                return Convert.ToBoolean(Convert.ToInt32(result));
            }
            catch (Exception ex)
            {
                Logging.WriteError(ex.Message + ". Class - " + this.ToString());
                isSuccess = false;
                return false;
            }
        }
    }
}
