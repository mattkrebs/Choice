using System.Net;

namespace ShakrLabs.Mobile.App.Data.Providers.Response
{
    public class DataObjectResponseBase
    {
        #region Fields
        public readonly DataAccessError Error;
		private readonly DataObjectSource _dataObjectSource;
        #endregion Fields


        #region Properties

        public bool HasError
        {
            get
            {
                var errorObjectNotNull = (Error != null);
                return errorObjectNotNull;
            }
        }

		public DataObjectSource ObjectSource
		{
			get
			{
				return _dataObjectSource;
			}
		}

        #endregion Properties


        #region Constructors & Initialization

        protected DataObjectResponseBase(DataObjectSource dataObjectSource)
        {
			_dataObjectSource = dataObjectSource;
        }

		protected DataObjectResponseBase(DataAccessError error, DataObjectSource dataObjectSource)
        {
            Error = error;
			_dataObjectSource = dataObjectSource;
        }

        ///// <summary>
        ///// Creates instance of DataObjectResponseBase from a DataAccessError.
        ///// </summary>
        ///// <returns></returns>
        //public static DataObjectResponseBase CreateBase(DataAccessError dataAccessError, DataObjectSource dataObjectSource)
        //{
        //    return new DataObjectResponseBase(dataAccessError, dataObjectSource);
        //}

        ///// <summary>
        ///// Creates instance of DataObjectResponseBase from a DataObjectResponse<T>.
        ///// </summary>
        ///// <returns></returns>
        //public static DataObjectResponseBase CreateBase<T>(DataObjectResponse<T> sourceDataObjectResponse, DataObjectSource dataObjectSource)
        //{
        //    return new DataObjectResponseBase(sourceDataObjectResponse.Error, dataObjectSource);
        //}

        ///// <summary>
        ///// Creates instance of DataObjectResponseBase from a HttpWebResponse.
        ///// </summary>
        ///// <returns></returns>
        //public static DataObjectResponseBase CreateBase(HttpWebResponse httpWebResponse, DataObjectSource dataObjectSource)
        //{
        //    DataAccessError dataAccessError = DataAccessError.CreateWithStatusCode(httpWebResponse.StatusCode);
        //    return new DataObjectResponseBase(dataAccessError, dataObjectSource);
        //}
		
        ///// <summary>
        ///// Creates instance of DataObjectResponseBase without an error.
        ///// </summary>
        ///// <returns></returns>
        //public static DataObjectResponseBase CreateBase(DataObjectSource dataObjectSource)
        //{
        //    return new DataObjectResponseBase(null, dataObjectSource);
        //}

        #endregion Constructors & Initialization


        #region Methods
        public void OnDataAccessErrorThrowException()
        {
            if (HasError)
            {
                throw DataAccessException.Create(Error);
            }
        }
        #endregion Methods
    }
}