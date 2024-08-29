using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;
using WebApi.Data.Models;
using WebApi.Infrastructure.Repository.Interface;
using WebApi.Utilities;

namespace WebApi.Business.Business
{
    public class PortProcessor : IPortProcessor
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public PortProcessor(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PortVM>> GetPorts()
        {
            var portDto = await _applicationDbContext.Ports.Include(p => p.Terminals).ToListAsync();
            var port = _mapper.Map<IEnumerable<PortVM>>(portDto);
            return port.ToList();
        }

        public async Task<PortVM> GetPort(int id)
        {
            var portDto = await _applicationDbContext.Ports.Include(p => p.Terminals).FirstOrDefaultAsync(p => p.Id == id);

            if (portDto == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found.");
            }
            var port = _mapper.Map<PortVM>(portDto);
            return port;
        }

        public async Task<PortVM> CreatePort(Port port)
        {
            if (_applicationDbContext.Ports.Any(p => p.Code == port.Code))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Port code must be unique.");
            }

            var portDto = _mapper.Map<PortDto>(port);
            _applicationDbContext.Ports.Add(portDto);
            await _applicationDbContext.SaveChangesAsync();

            var portVm = _mapper.Map<PortVM>(portDto);
            portVm.Id = portDto.Id;
            return portVm;
        }

        public async Task<bool> UpdatePort(EditPort port)
        {

            var existingPort = await _applicationDbContext.Ports.FindAsync(port.Id);
            if (existingPort == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Request resource not found to update");
            }

            var portDto = _mapper.Map<PortDto>(existingPort);

            portDto.Name = port.Name;
            portDto.Code = port.Code;
            portDto.LastEditedDate = DateTime.UtcNow;

            _applicationDbContext.Ports.Update(portDto);

            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePort(int id)
        {
            var port = await _applicationDbContext.Ports.FindAsync(id);
            if (port == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found to delete");
            }

            _applicationDbContext.Ports.Remove(port);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
