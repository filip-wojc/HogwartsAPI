using FluentValidation;
using HogwartsAPI.Dtos.WandDtos;
using HogwartsAPI.Entities;

namespace HogwartsAPI.Dtos.WandWalidators
{
    public class CreateWandWalidator : AbstractValidator<CreateWandDto>
    {
        private readonly HogwartDbContext _context;
        public CreateWandWalidator(HogwartDbContext context)
        {
            _context = context;

            RuleFor(w => w.Price).NotEmpty();
            RuleFor(w => w.Length).NotEmpty().LessThanOrEqualTo(14).GreaterThanOrEqualTo(9);
            RuleFor(w => w.WoodType).NotEmpty();
            RuleFor(w => w.Color).NotEmpty();
            RuleFor(w => w.CoreId).NotEmpty().Must(
                (core, x) => CoreExists(core.CoreId)
                ).WithMessage($"That id does not exist");
        }

        private bool CoreExists(int coreId)
        {
            return _context.Cores.Any(c => c.Id == coreId);
        }
    }
}
