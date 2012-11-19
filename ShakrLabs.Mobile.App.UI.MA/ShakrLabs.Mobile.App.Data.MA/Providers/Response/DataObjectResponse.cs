
using System;

namespace ShakrLabs.Mobile.App.Data.Providers.Response
{
	public enum DataObjectSource
	{
        Unspecified,
		Local,
        Remote
	}

    public class DataObjectResponse<T> : DataObjectResponseBase
    {
        #region Fields

        public T DataObject { get; private set; }
        #endregion Fields


        #region Properties
	
		#endregion Properties


		#region Constructors & Initialization
        #region Constructors
        private DataObjectResponse(T dataObject, DataObjectSource dataObjectSource)
			: base(dataObjectSource)
        {
            DataObject = dataObject;
        }
	
		private DataObjectResponse(DataAccessError error, DataObjectSource dataObjectSource)
			: base(error, dataObjectSource)
        { }

        private DataObjectResponse(DataObjectSource dataObjectSource): base(dataObjectSource)
        {
            
        }
        #endregion


        #region Factory Methods
        public static DataObjectResponse<T> Create(DataObjectSource dataObjectSource)
        {
            return new DataObjectResponse<T>(dataObjectSource);
        }

		public static DataObjectResponse<T> Create(DataAccessError dataAccessError, DataObjectSource dataObjectSource)
        {
			return new DataObjectResponse<T>(dataAccessError, dataObjectSource);
        }

		public static DataObjectResponse<T> Create(T dataObject, DataObjectSource dataObjectSource)
        {
            if (dataObject == null) // will always be false for a value type which is the desired effect. Ignore the Resharper warning.
            {
                Console.WriteLine("Null DataObject sent to DataObjectResponse<T>.Create");
				//return new DataObjectResponse<T>(DataAccessError.CreateNoContentErrorCode(), dataObjectSource);
            }
			return new DataObjectResponse<T>(dataObject, dataObjectSource);
        }

        /// <summary>
        /// Creates a DataObjectResponse from another response. 
        /// </summary>
        /// <param name="dataObjectResponse"></param>
        /// <param name="dataObject"> </param>
        /// <returns></returns>
        public static DataObjectResponse<T> Create(DataObjectResponseBase dataObjectResponse, T dataObject)
        {
            var objectResponse = Create(dataObjectResponse.Error, dataObjectResponse.ObjectSource);
            objectResponse.DataObject = dataObject;
            return objectResponse;
        }
        #endregion
        #endregion Constructors & Initialization


        #region Methods
        //public T GetDataObjectOrHandleError()
        //{
        //    if (HasError)
        //    {
        //        throw DataAccessException.Create(Error);
        //    }
        //    return DataObject;
        //}
        #endregion Methods
    }
}