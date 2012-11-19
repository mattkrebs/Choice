using System.Collections.Generic;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public class ParameterList : List<Parameter>
    {
        public bool HasNonEmptyParameter
        {
            get
            {
                foreach (var parameter in this)
                {
                    if (!string.IsNullOrEmpty(parameter.Value))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Add(string key, string value)
        {
            var parameter = new Parameter(key, value);
            Add(parameter);
        }

        public static ParameterList Create(string key, string value)
        {
            var list = new ParameterList();
            list.Add(key, value);
            return list;
        }

        public string GetValue(string name)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Name == name)
                {
                    return this[i].Value;
                }
            }
            return null;
        }

        public void SetValue(string name, string value)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Name != name) continue;
                this[i].Value = value;
                return;
            }
            Add(name, value);
        }
    }
}