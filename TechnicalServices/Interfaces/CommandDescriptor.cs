using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TechnicalServices.Interfaces
{
    [DebuggerDisplay("EquipmentId = {EquipmentId}, command = {CommandName}")]
    [Serializable]
    public class CommandDescriptor : IEquatable<CommandDescriptor>
    {
        private readonly List<IConvertible> _list = new List<IConvertible>();

        public CommandDescriptor()
        {
        }

        public CommandDescriptor(int Id) : this()
        {
            EquipmentId = Id;
        }

        public CommandDescriptor(int Id, string command) : this(Id)
        {
            CommandName = command;
        }

        public CommandDescriptor(int Id, string command, IConvertible param) :
            this(Id, command)
        {
            Parameters.Add(param);
        }

        public CommandDescriptor(int Id, string command, IConvertible param1, IConvertible param2) :
            this(Id, command)
        {
            Parameters.Add(param1);
            Parameters.Add(param2);
        }

        public CommandDescriptor(int Id, string command, params IConvertible[] parameters) :
            this(Id, command)
        {
            if (parameters != null)
                Parameters.AddRange(parameters);
        }

        public int EquipmentId { get; private set; }
        public string CommandName { get; set; }

        public List<IConvertible> Parameters
        {
            get { return _list; }
        }

        #region IEquatable<CommandDescriptor> Members

        public bool Equals(CommandDescriptor other)
        {
            bool isParametersEquals = true;
            if (Parameters.Count != other.Parameters.Count) isParametersEquals = false;
            else
            {
                foreach (IConvertible parameter in Parameters)
                {
                    if (
                        !other.Parameters.Exists(
                             par =>
                             par.ToString().Equals(parameter.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        isParametersEquals = false;
                        break;
                    }
                }
            }
            return isParametersEquals && EquipmentId == other.EquipmentId && CommandName == other.CommandName;
        }

        #endregion

        public static string Encode(string value)
        {
            return value. Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\x00", "");
        }
    }
}