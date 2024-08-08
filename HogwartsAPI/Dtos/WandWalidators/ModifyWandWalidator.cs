using FluentValidation;
using HogwartsAPI.Dtos.WandDtos;

namespace HogwartsAPI.Dtos.WandWalidators
{
    public class ModifyWandWalidator : AbstractValidator<ModifyWandDto>
    {
        public ModifyWandWalidator()
        {
            RuleFor(w => w.Price).NotEmpty();
        }
    }
}
