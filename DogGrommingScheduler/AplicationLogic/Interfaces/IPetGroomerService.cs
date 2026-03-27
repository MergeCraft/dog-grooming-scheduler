using BusinessLogic.Results;
using Shared.DTOs.PetGroomerDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AplicationLogic.Interfaces
{
    public interface IPetGroomerService
    {
        Task<Result<IEnumerable<PetGroomerDto>>> GetAllGroomersAsync();
    }
}
