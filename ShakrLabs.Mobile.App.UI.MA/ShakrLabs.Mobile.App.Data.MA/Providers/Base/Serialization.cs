using Newtonsoft.Json;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public static class Serialization
    {
        public static T DeserializeObject<T>(string serializedObjectString)
        {
            var dataObject = JsonConvert.DeserializeObject<T>(serializedObjectString);
            return dataObject;
        }
    }
}