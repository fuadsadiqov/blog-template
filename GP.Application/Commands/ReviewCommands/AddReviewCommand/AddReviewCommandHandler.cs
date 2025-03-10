using GP.Core.Enums.Enitity;
using GP.Core.Exceptions;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.ReviewRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Common;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.ReviewCommands.AddReviewCommand
{
    public class AddReviewCommandHandler : ICommandHandler<AddReviewCommand, AddReviewResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewCommandHandler(IReviewRepository reviewRepository, IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddReviewResponse> Handle(AddReviewCommand command, CancellationToken cancellationToken)
        {
            var message = command.Request.Message;
            var email = command.Request.Email;
            var blogId = command.Request.BlogId;
             
            var user = await _userRepository.GetUserByEmailAsync(email);
            
            var responseMessage = "";
            bool isSuccedd = true;
            
            if (message.Trim().Length <= 0) {
                responseMessage = "Rəy boşdur";
                isSuccedd = false;
            }

            var reviewAlreadyExist = await _reviewRepository.GetFirstAsync(r => r.UserId == user.Id && r.BlogId == blogId);

            if(reviewAlreadyExist != null)
            {
                responseMessage = "Artıq rəy bildirmisiniz";
                isSuccedd = false;
            }

            if(user == null)
            {
                responseMessage = "İstifadəçi tapılmadı";
                isSuccedd = false;
            }

            if (isSuccedd)
            {
                var review = new Review()
                {
                    Id = Guid.NewGuid(),
                    BlogId = blogId,
                    Message = message,
                    UserId = user.Id,
                    Status = RecordStatusEnum.Active,
                    DateCreated = DateTime.Now
                };
                await _reviewRepository.AddAsync(review);
                responseMessage = "Review added successfully";
                await _unitOfWork.CompleteAsync();   
            }

            return new AddReviewResponse
            {
                IsSuccedd = isSuccedd,
                Message = responseMessage
            };
        }
    }
}
