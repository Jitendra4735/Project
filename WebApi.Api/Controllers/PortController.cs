using Microsoft.AspNetCore.Mvc;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PortController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger;

        private readonly IPortProcessor _portProcessor;

        public PortController(IPortProcessor portProcessor)
        {
            _portProcessor = portProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortVM>>> GetPorts()
        {
            var port = await _portProcessor.GetPorts();
            return port.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PortVM>> GetPort(int id)
        {
            var port = await _portProcessor.GetPort(id);
            return port;
        }

        [HttpPost]
        public async Task<ActionResult<PortVM>> CreatePort(Port port)
        {
            var portVm = await _portProcessor.CreatePort(port);
            return portVm;
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePort(EditPort port)
        {
            bool isSuccess = await _portProcessor.UpdatePort(port);
            return isSuccess ? Ok("Resource updated successfully") : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerminal(int id)
        {
            bool isSuccess = await _portProcessor.DeletePort(id);
            return isSuccess ? Ok("Resource removed successfully") : BadRequest();
        }
    }
}
