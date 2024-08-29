using Microsoft.AspNetCore.Mvc;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TerminalsController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger;

        private readonly ITerminalProcessor _terminalProcessor;

        public TerminalsController(ITerminalProcessor terminalProcessor)
        {
            _terminalProcessor = terminalProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TerminalVM>>> GetTerminals()
        {
            var terminal = await _terminalProcessor.GetTerminals();
            return terminal.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TerminalVM>> GetTerminal(int id)
        {
            var terminal = await _terminalProcessor.GetTerminal(id);
            return terminal;
        }

        [HttpPost]
        public async Task<ActionResult<TerminalVM>> CreateTerminal(Terminal terminal)
        {
            var terminalVM = await _terminalProcessor.CreateTerminal(terminal);
            return CreatedAtAction(nameof(GetTerminal), new { id = terminalVM.Id }, terminalVM); ;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTerminal(TerminalEdit terminal)
        {
            bool isSuccess = await _terminalProcessor.UpdateTerminal(terminal);
            return isSuccess ? Ok("Resource updated successfully") : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerminal(int id)
        {
            bool isSuccess = await _terminalProcessor.DeleteTerminal(id);
            return isSuccess ? Ok("Resource removed successfully") : BadRequest();
        }
    }
}
