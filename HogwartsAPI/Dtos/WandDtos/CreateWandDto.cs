     using HogwartsAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace HogwartsAPI.Dtos.WandDtos
{
    public class CreateWandDto
    {
        public decimal Price { get; set; }
        public double Length { get; set; }
        public string? WoodType { get; set; }
        public string? Color { get; set; }
        public int CoreId { get; set; }
    }
}
