using System;
using System.Threading.Tasks;
using PaymentGatewayService.AcquiringBank.Commands;

namespace PaymentGatewayService.AcquiringBank
{
    /*
     * This is just a mock of the acquiring bank. Ideally this should be a different
     * micro service and should not be contained here.
     */
    public class AcquiringBankService : IAcquiringBankService
    {
        private readonly AcquiringBankServiceParameters _parameters;

        private static Random _random;
        private static double _bankRemainingBalance;

        public AcquiringBankService(
            AcquiringBankServiceParameters parameters)
        {
            _parameters = parameters;
            _random = new Random();

            _bankRemainingBalance = _parameters.BankTotalBalance;
        }

        public async Task<StartTransactionResponse> StartTransaction(StartTransactionCommand command)
        {
            var totalTransactionTime = _random.Next(
                _parameters.TransactionMinTimeMs,
                _parameters.TransactionMaxTimeMs);

            await Task.Delay(totalTransactionTime);

            if (totalTransactionTime >= _parameters.TransactionTimeoutTimeMs)
            {
                return new StartTransactionResponse()
                {
                    ErrorMessage = "Transaction timed out",
                };
            }

            /* Not taking in consideration the currency at this point */
            if (_bankRemainingBalance < command.Amount)
            {
                return new StartTransactionResponse()
                {
                    ErrorMessage = "Not enough balance for this transaction.",
                };
            }

            _bankRemainingBalance -= command.Amount;

            return new StartTransactionResponse();
        }
    }
}