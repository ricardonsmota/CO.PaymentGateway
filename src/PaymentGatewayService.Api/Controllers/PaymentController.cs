using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayService.Api.ViewModels;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPaymentService _service;

        public PaymentController(
            IMapper mapper,
            IPaymentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _service.Get(new GetPaymentCommand()
            {
                Id = id
            });

            if (response.IsError)
            {
                switch (response.Error?.Code)
                {
                    case ServiceErrorCode.ValidationError:
                        return BadRequest();
                    case ServiceErrorCode.NotFound:
                        return NotFound();
                }
            }

            var viewModel = _mapper.Map<Payment,PaymentViewModel>(response.Result);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentRequest model)
        {
            var response = await _service.Create(new CreatePaymentCommand()
            {
                Amount = model.Amount,
                Currency = model.Currency,
                Cvv = model.Cvv,
                CardNumber = model.CardNumber,
                ExpirationMonth = model.ExpirationMonth,
                ExpirationYear = model.ExpirationYear
            });

            if (response.IsError)
            {
                switch (response.Error?.Code)
                {
                    case ServiceErrorCode.ValidationError:
                        return BadRequest();
                    case ServiceErrorCode.NotFound:
                        return NotFound();
                }
            }

            var viewModel = _mapper.Map<Payment,PaymentViewModel>(response.Result);

            return Ok(viewModel);
        }
    }
}