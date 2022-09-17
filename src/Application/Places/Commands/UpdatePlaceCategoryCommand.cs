using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    public class UpdatePlaceCategoryCommand : ICommand
    {
        public long PlaceId { get; set; }
        public List<PlaceCategory> Categories { get; set; } = new();
    }

    public class AddPlaceCategoryCommandHandler : CommandHandler<UpdatePlaceCategoryCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPlaceCategoryCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(UpdatePlaceCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var place = await _placeRepository.FindByIdAsync(command.PlaceId) ?? throw new PlaceNotFoundException(new { command.PlaceId });
                place.UpdateCategories(command.Categories);

                await _unitOfWork.CommitAsync();
            }

            catch (Exception)
            {
            }

           
            return Unit.Value;
        }
    }
}
