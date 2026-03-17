using BusinessLogic.Entities;
using Shared.DTOs.PetGroomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.PetGroomerMappers
{
    public static class PetGroomerMapper
    {
        public static PetGroomerDto ToDto(PetGroomer entity)
        {
            return new PetGroomerDto
            {
                Id = entity.Id,
                Name = entity.User.Name,
            };
        }
    }
}
