namespace PaymentGatewayService.Common.ServiceResponse
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(ServiceError error)
        {
            Error = error;
        }

        public ServiceResponse(ServiceErrorCode errorCode)
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

    public class ServiceResponse<T> : ServiceResponse
    {
        public ServiceResponse(ServiceError error)
            : base(error)
        {
        }

        public ServiceResponse()
        {
        }

        public ServiceResponse(ServiceErrorCode errorCode)
            : base(errorCode)
        {
        }

        public ServiceResponse(T result)
        {
            Result = result;
        }

        public T Result { get; set; }
    }
}