namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    public interface IRemoteDataProvider
    {
        bool CheckObjectInMemory(ParameterList parameters);
    }
}