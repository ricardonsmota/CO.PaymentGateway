using System.Threading.Tasks;
using PaymentGatewayService.AcquiringBank.Commands;

namespace PaymentGatewayService.AcquiringBank
{
    public interface IAcquiringBankService
    {
        Task<StartTransactionResponse> StartTransaction(StartTransactionCommand command);
    }
}