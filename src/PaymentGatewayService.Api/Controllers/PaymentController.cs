using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayService.Api.ViewModels;
using PaymentGatewayService.Payments;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
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
                return BadRequest(response.Error);
            }

            // var viewModel = _mapper.Map<(DataSet dataSet, List<ShareableGroup> shareableGroups), DataSetSimpleViewModel>(response.Result);

            return Ok(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
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
                return BadRequest(response.Error);
            }

            // var viewModel = _mapper.Map<(DataSet dataSet, List<ShareableGroup> shareableGroups), DataSetSimpleViewModel>(response.Result);

            return Ok(response.Result);
        }
    }
}