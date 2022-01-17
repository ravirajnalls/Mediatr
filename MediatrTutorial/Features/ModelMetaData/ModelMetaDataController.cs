using MediatR;
using MediatrTutorial.Dto;
using MediatrTutorial.Features.Customer.Commands.CreateCustomer;
using MediatrTutorial.Features.Customer.Queries.GetCustomerById;
using MediatrTutorial.Features.ModelMetaData.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static MediatrTutorial.Features.ModelMetaData.Commands.Create;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace MediatrTutorial.Features.Customer
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelMetaDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModelMetaDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ModelMetaDataCommand createCommand)
        {
            string projectId = await _mediator.Send(createCommand);
            return Ok(projectId);
        }

        //List
        [HttpGet]
        public async Task<ActionResult<ListAll.Result>> ListAll([FromQuery]string name)
        {
            var query = new ListAll.Query();
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
