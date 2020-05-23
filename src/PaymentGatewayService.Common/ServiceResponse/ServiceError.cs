namespace PaymentGatewayService.Common.ServiceResponse
{
    public class ServiceError
    {
        public ServiceError()
        {
        }

        public ServiceError(ServiceErrorCode code)
        {
            Code = code;
        }

        public ServiceErrorCode Code { get; set; }
    }
}