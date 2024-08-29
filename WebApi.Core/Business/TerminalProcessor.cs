using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;
using WebApi.Data.Models;
using WebApi.Infrastructure.Repository.Interface;
using WebApi.Utilities;

namespace WebApi.Business.Business
{
    public class TerminalProcessor : ITerminalProcessor
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public TerminalProcessor(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TerminalVM>> GetTerminals()
        {
            var terminalDto = await _applicationDbContext.Terminals.Include(t => t.Port).ToListAsync();
            var terminal = _mapper.Map<IEnumerable<TerminalVM>>(terminalDto);
            return terminal.ToList();
        }


        public async Task<TerminalVM> GetTerminal(int id)
        {
            var terminalDto = await _applicationDbContext.Terminals.Include(t => t.Port).FirstOrDefaultAsync(t => t.Id == id);

            if (terminalDto == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found.");
            }
            var terminal = _mapper.Map<TerminalVM>(terminalDto);
            return terminal;
        }

        public async Task<TerminalVM> CreateTerminal(Terminal terminal)
        {
            if (_applicationDbContext.Terminals.Any(t => t.Name == terminal.Name && t.PortId == terminal.PortId))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Terminal name must be unique for the port.");
            }
            var terminalDto = _mapper.Map<TerminalDto>(terminal);
            _applicationDbContext.Terminals.Add(terminalDto);
            await _applicationDbContext.SaveChangesAsync();

            //201 created needs to add
            var terminalVM = await GetTerminal(terminalDto.Id);
            return terminalVM;
        }

        public async Task<bool> UpdateTerminal(TerminalEdit terminal)
        {
            var existingTerminal = await _applicationDbContext.Terminals.FindAsync(terminal.Id);
            if (existingTerminal == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Request resource not found to update");
            }

            var terminalDto = _mapper.Map<TerminalDto>(existingTerminal);

            terminalDto.Name = terminal.Name;
            terminalDto.Latitude = terminal.Latitude;
            terminalDto.Longitude = terminal.Longitude;
            terminalDto.IsActive = terminal.IsActive;
            terminalDto.LastEditedDate = DateTime.UtcNow;

            _applicationDbContext.Terminals.Update(terminalDto);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTerminal(int id)
        {
            var terminal = await _applicationDbContext.Terminals.FindAsync(id);
            if (terminal == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found to delete");
            }
            _applicationDbContext.Terminals.Remove(terminal);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
