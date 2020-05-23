namespace PaymentGatewayService.Common.ServiceResponse
{
    /**
     * Ideally this logic would be put into a separate project (like a domain base project) that could be
     * packed into a Nuget and reused on other services.
     */
    public class ServiceResult
    {
        public ServiceResult()
        {
        }

        public ServiceResult(ServiceError error)
        {
            Error = error;
        }

        public ServiceResult(ServiceErrorCode errorCode)
        {
            Error = new ServiceError
            {
                Code = errorCode
            };
        }

        public ServiceError Error { get; set; }

        public bool IsError => Error != null;

        public bool IsSuccess => !IsError;
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult(ServiceError error)
            : base(error)
        {
        }

        public ServiceResult()
        {
        }

        public ServiceResult(ServiceErrorCode errorCode)
            : base(errorCode)
        {
        }

        public ServiceResult(T result)
        {
            Result = result;
        }

        public T Result { get; set; }
    }
}