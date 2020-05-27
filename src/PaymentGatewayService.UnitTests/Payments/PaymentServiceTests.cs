using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGatewayService.AcquiringBank;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.UnitTests.Payments
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private IPaymentService _paymentService;

        private Mock<IPaymentRepository> _repositoryMock;
        private Mock<IValidatorFactory> _validatorFactoryMock;
        private Mock<IAcquiringBankService> _acquiringBankServiceMock;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger<PaymentService>>();

            _repositoryMock = new Mock<IPaymentRepository>();
            _validatorFactoryMock = new Mock<IValidatorFactory>();

            _acquiringBankServiceMock = new Mock<IAcquiringBankService>();

            _paymentService = new PaymentService(
                loggerMock.Object,
                _validatorFactoryMock.Object,
                _repositoryMock.Object,
                _acquiringBankServiceMock.Object);
        }

        [Test]
        public async Task CreatePayment_ValidRequest_CreatesPayment()
        {
            // Arrange
            IsValidationSuccess(true);

            var command = new CreatePaymentCommand()
            {
                Amount = 100,
                CardNumber = "1111 0000 0000 0000",
                Currency = "EUR",
                Cvv = "123",
                ExpirationMonth = 10,
                ExpirationYear = 20
            };

            // Act
            var response = await _paymentService.Create(command);

            // Assert
            Assert.IsFalse(response.IsError);
            Assert.AreEqual(command.Amount, response.Result.Amount);
            Assert.AreEqual(command.CardNumber, response.Result.CardNumber);
            Assert.AreEqual(command.Currency, response.Result.Currency);
            Assert.AreEqual(command.Cvv, response.Result.Cvv);
            Assert.AreEqual(command.ExpirationMonth, response.Result.ExpirationMonth);
            Assert.AreEqual(command.ExpirationYear, response.Result.ExpirationYear);
        }

        [Test]
        public async Task CreatePayment_ValidationError_PaymentNotCreated()
        {
            // Arrange
            IsValidationSuccess(false);

            var command = new CreatePaymentCommand()
            {
                Amount = 100,
                CardNumber = "1111 0000 0000 0000",
                Currency = "EUR",
                Cvv = "123",
                ExpirationMonth = 10,
                ExpirationYear = 20
            };

            // Act
            var response = await _paymentService.Create(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.ValidationError);
        }

        [Test]
        public async Task GetPayment_ValidRequest_RetrievesPayment()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var payment = new Payment()
            {
                Id = paymentId,
                Amount = 100,
                CardNumber = "1111 0000 0000 0000",
                Currency = "EUR",
                Cvv = "123",
                ExpirationMonth = 10,
                ExpirationYear = 20
            };

            var command = new GetPaymentCommand()
            {
                Id = paymentId
            };

            _repositoryMock.Setup(
                    x => x.Get(paymentId))
                .ReturnsAsync(payment);

            // Act
            var response = await _paymentService.Get(command);

            // Assert
            Assert.IsFalse(response.IsError);
            Assert.AreEqual(response.Result, payment);
        }

        [Test]
        public async Task GetPayment_ValidationError_FailToFetchPayment()
        {
            // Arrange
            IsValidationSuccess(false);

            var paymentId = "TestId";

            var command = new GetPaymentCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.Get(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.ValidationError);
        }

        [Test]
        public async Task GetPayment_NotFound_FailToFetchPayment()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var command = new GetPaymentCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.Get(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.NotFound);
        }

        [Test]
        public async Task SetPaymentStatusAccepted_ValidRequest_SetsPaymentStatusAsAccepted()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var payment = new Payment()
            {
                Id = paymentId,
                Amount = 100,
                CardNumber = "1111 0000 0000 0000",
                Currency = "EUR",
                Cvv = "123",
                ExpirationMonth = 10,
                ExpirationYear = 20,
                Status = new PaymentStatus()
                {
                    StatusCode = PaymentStatusCode.Processing
                }
            };

            var command = new SetPaymentStatusAcceptedCommand()
            {
                Id = paymentId
            };

            _repositoryMock.Setup(
                    x => x.Get(paymentId))
                .ReturnsAsync(payment);

            // Act
            var response = await _paymentService.SetStatusAccepted(command);

            // Assert
            Assert.IsFalse(response.IsError);
            Assert.AreEqual(payment.Status.StatusCode, PaymentStatusCode.Accepted);
        }

        [Test]
        public async Task SetPaymentStatusAccepted_ValidationError_FailToSetPaymentAsAccepted()
        {
            // Arrange
            IsValidationSuccess(false);

            var paymentId = "TestId";

            var command = new SetPaymentStatusAcceptedCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.SetStatusAccepted(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.ValidationError);
        }

        [Test]
        public async Task SetPaymentStatusAccepted_NotFound_FailToSetPaymentAsSuccess()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var command = new SetPaymentStatusAcceptedCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.SetStatusAccepted(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.NotFound);
        }

        [Test]
        public async Task SetPaymentStatusRejected_ValidRequest_SetsPaymentStatusAsRejected()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var payment = new Payment()
            {
                Id = paymentId,
                Amount = 100,
                CardNumber = "1111 0000 0000 0000",
                Currency = "EUR",
                Cvv = "123",
                ExpirationMonth = 10,
                ExpirationYear = 20,
                Status = new PaymentStatus()
                {
                    StatusCode = PaymentStatusCode.Processing
                }
            };

            var command = new SetPaymentStatusRejectedCommand()
            {
                Id = paymentId
            };

            _repositoryMock.Setup(
                    x => x.Get(paymentId))
                .ReturnsAsync(payment);

            // Act
            var response = await _paymentService.SetStatusRejected(command);

            // Assert
            Assert.IsFalse(response.IsError);
            Assert.AreEqual(payment.Status.StatusCode, PaymentStatusCode.Rejected);
        }

        [Test]
        public async Task SetPaymentStatusRejected_ValidationError_FailToSetPaymentAsRejected()
        {
            // Arrange
            IsValidationSuccess(false);

            var paymentId = "TestId";

            var command = new SetPaymentStatusRejectedCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.SetStatusRejected(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.ValidationError);
        }

        [Test]
        public async Task SetPaymentStatusRejected_NotFound_FailToSetPaymentAsRejected()
        {
            // Arrange
            IsValidationSuccess(true);

            var paymentId = "TestId";

            var command = new SetPaymentStatusRejectedCommand()
            {
                Id = paymentId
            };

            // Act
            var response = await _paymentService.SetStatusRejected(command);

            // Assert
            Assert.IsTrue(response.IsError);
            Assert.AreEqual(response.Error.Code, ServiceErrorCode.NotFound);
        }

        private void IsValidationSuccess(bool isValid)
        {
            var validationResult = new Mock<ValidationResult>();

            validationResult.Setup(
                x => x.IsValid).Returns(isValid);

            var validatorMock = new Mock<IValidator<object>>();

            validatorMock.Setup(x => x.ValidateAsync(
                    It.IsAny<object>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult.Object);

            _validatorFactoryMock.Setup(
                    x => x.GetValidator<object>())
                .Returns(validatorMock.Object);
        }
    }
}