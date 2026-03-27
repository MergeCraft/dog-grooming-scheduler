namespace Shared.DTOs.PetGroomerDtos
{
    public class PetGroomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Podrías agregar especialidad o foto si lo deseas a futuro
    }
}
