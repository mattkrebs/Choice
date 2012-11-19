namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public class Parameter
    {
        public string Name;
        public string Value;

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Parameter()
        {
        }

        public static ParameterList CreateList(string key, string value)
        {
            var list = new ParameterList();
            list.Add(key, value);
            return list;
        }
    }
}