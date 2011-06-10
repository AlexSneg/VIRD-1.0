using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Hosts.Plugins.IEDocument.Common
{
    public class Param
    {
        private string postKey;
        private string postValue;

        public Param() { }

        public Param(string _postKey, string _postValue)
        {
            this.postKey = _postKey;
            this.postValue = _postValue;
        }

        public string PostKey
        {
            get { return postKey; }

            set
            {
                postKey = value;
            }
        }
        public string PostValue
        {
            get { return postValue; }

            set
            {
                postValue = value;
            }
        }
    }

    public class ParsePOSTParams : IEnumerable
    {
        private IList<Param> postParams;
        private int position = -1;
        private string incorrectParams = "";

        public ParsePOSTParams()
        {
            postParams = new List<Param>();
        }

        public string Verification(string str)
        {
            string[] split = null;
            split = str.Split(new Char[] { ';' });

            for (int i = 0; i < split.Length; i++)
            {
                ParseParam(split[i]);
            }
            //if (incorrectParams != "") 
            //{
            //    throw new NullReferenceException(String.Format("Некорректно введённые параметры: {0}.", incorrectParams));
            //}
            return incorrectParams;

        }

        private void ParseParam(string _param)
        {
            string keyPattern = "";
            string valuePattern = "";
            string paramPattern = @"^\s*(?<keyPattern>[^\s\;][0-9a-zA-ZА-Яа-я]*)\s*\=\s*(?<valuePattern>[^\s\;][0-9a-zA-ZА-Яа-я]*)\s*$";

            Regex reg1 = new Regex(paramPattern);
            Match match1 = reg1.Match(_param);
            keyPattern = match1.Groups["keyPattern"].Value;
            if (keyPattern != "")
            {
                Regex reg2 = new Regex(paramPattern);
                Match match2 = reg2.Match(_param);
                valuePattern = match2.Groups["valuePattern"].Value;
                if (valuePattern != "")
                {
                    postParams.Add(new Param(keyPattern.Trim(), valuePattern.Trim()));
                }
                else
                {
                    incorrectParams = incorrectParams + _param + " ";
                }
            }
            else
            {
                //int incorrectPosition = position++;
                incorrectParams = incorrectParams + _param + "; ";
            }
        }

        public IEnumerator GetEnumerator()
        {
            return postParams.GetEnumerator();
        }

        public void Add(Param param)
        {
            postParams.Add(param);
            this.position++;
        }

        public void RemoveAt(int index)
        {
            postParams.RemoveAt(index);
        }

        public void Clear()
        {
            position = -1;
            postParams.Clear();
        }

        #region Properties

        public int Count
        {
            get
            {
                return postParams.Count;
            }
        }

        public Param this[int i]
        {
            get
            {
                if (i < 0 || i > Count)
                {
                    throw new IndexOutOfRangeException("Out of range in ParsePOSTParams.");
                }
                else
                    return postParams[i];
            }
        }

        #endregion
    }
}
