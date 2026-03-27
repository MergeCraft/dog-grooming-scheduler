using AplicationLogic.Interfaces;
using BusinessLogic.RepositoriesInterfaces;
using BusinessLogic.Results;
using Shared.DTOs.PetGroomerDtos;
using Shared.DTOs.PetGroomerMappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationLogic.Services.PetGroomer
{
    public class PetGroomerService : IPetGroomerService
    {
        private readonly IPetGroomerRepository _groomerRepository;

        public PetGroomerService(IPetGroomerRepository groomerRepository)
        {
            _groomerRepository = groomerRepository;
        }

        public async Task<Result<IEnumerable<PetGroomerDto>>> GetAllGroomersAsync()
        {
            var result = await _groomerRepository.GetAllAsync();

            if (result.IsFailure)
                return Result<IEnumerable<PetGroomerDto>>.Failure(result.Errors);

            var dtos = result.Value.Select(groomer => PetGroomerMapper.ToDto(groomer));

            return Result<IEnumerable<PetGroomerDto>>.Success(dtos);
        }
    }
}
