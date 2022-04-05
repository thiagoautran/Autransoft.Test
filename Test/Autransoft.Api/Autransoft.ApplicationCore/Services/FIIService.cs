using Autransoft.ApplicationCore.Entities;
using Autransoft.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autransoft.ApplicationCore.Services
{
    public class FIIService : IFIIService
    {
        private readonly IFIIRepository _fiiRepository;

        public FIIService(IFIIRepository fiiRepository) =>
            _fiiRepository = fiiRepository;

        public async Task<IEnumerable<FIIEntity>> ListAsync() =>
            await _fiiRepository.ListAsync();
    }
}