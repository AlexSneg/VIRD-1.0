using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Common
{
    public abstract class Command : IDisposable
    {
        protected Command(string commandName)
        {
            _commandName = commandName;
        }

        protected bool IsCanceled = false;
        protected bool IsExecuted = false;
        private readonly string _commandName;

        public string CommandName { get { return _commandName; } }

        public bool Execute()
        {
            bool isSuccess = true;
            if (!IsCanceled && !IsExecuted)
            {
                try
                {
                    isSuccess = OnExecute();
                }
                finally
                {
                    IsExecuted = true;
                }
            }
            return isSuccess;
        }

        public void Commit()
        {
            if (!IsCanceled && IsExecuted)
            {
                try
                {
                    OnCommit();
                }
                finally
                {
                    IsCanceled = true;
                }
            }
        }
        public void RollBack()
        {
            if (!IsCanceled && IsExecuted)
            {
                try
                {
                    OnRollBack();
                }
                finally
                {
                    IsCanceled = true;
                }
            }
        }

        public void Dispose()
        {
            RollBack();
        }

        protected abstract bool OnExecute();
        protected abstract void OnRollBack();
        protected abstract void OnCommit();
    }
}
