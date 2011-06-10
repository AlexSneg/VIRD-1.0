using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Common
{
    public class CommandInvoker : IDisposable
    {
        protected readonly List<Command> _commandList = new List<Command>();

        public void AddCommand(Command command)
        {
            _commandList.Add(command);
        }

        public static bool Execute(CommandInvoker invoker)
        {
            try
            {
                bool result = invoker.Execute();
                invoker.Commit();
                return result;
            }
            finally
            {
                invoker.Dispose();
            }
        }

        public void Commit()
        {
            _commandList.ForEach(com=>com.Commit());
        }

        public void RollBack()
        {
            _commandList.ForEach(com=>com.RollBack());
        }

        public bool Execute()
        {
            bool result = true;
            _commandList.ForEach(com=> result = result & com.Execute());
            return result;
        }


        public void Dispose()
        {
            foreach (Command command in _commandList)
            {
                try
                {
                    command.Dispose();
                }
                catch{}
            }
            _commandList.Clear();
        }
    }
}
