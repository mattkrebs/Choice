using System;

namespace ShakrLabs.Mobile.App.Data
{
    public interface IAppConfig
    {
        //string RootFilePath { get; }
        string StoragePath { get; }
        //SerializationFormat ProviderSerializationFormat { get; }
        string PublicUriBase { get; }
        string MemberUriBase { get; }
        void SetAppAssetStringDelegate(Func<string, string> appAssetStringDelegate);
        string GetAsset(string uri);
        //string GetValue(string key);
        void DeleteAllDataFiles();

        string LoginUri { get; }
 
    }
}