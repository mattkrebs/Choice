using System;

namespace ShakrLabs.Mobile.App.Data.Providers.Response
{
    public class DataAccessException : Exception
    {
        private DataAccessException()
        {
        }

        private DataAccessException(string message) : base(message)
        {
        }

        public DataAccessError DataAccessError { get; set; }

        public static DataAccessException Create(DataAccessError dataAccessError)
        {
            DataAccessException dataAccessException;
            if (dataAccessError == null)
            {
                dataAccessException = new DataAccessException();
            }
            else
            {
                dataAccessException = new DataAccessException(dataAccessError.Message);
            }
            dataAccessException.DataAccessError = dataAccessError;
            return dataAccessException;
        }

        //public static DataAccessException Create(DataAccessError dataAccessError, string message)
        //{
        //    dataAccessError.Message = message;
        //    return Create(dataAccessError);
        //}
    }
}