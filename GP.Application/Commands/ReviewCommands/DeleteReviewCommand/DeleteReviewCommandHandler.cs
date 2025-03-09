using System.Security.Claims;
using GP.Application.Queries.UserQueries.GetAuthUser;
using GP.Core.Exceptions;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.ReviewRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Commands.ReviewCommands.DeleteReviewCommand;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, DeleteReviewResponse>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly AuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteReviewCommandHandler(AuthService authService, IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteReviewResponse> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var id = command.Request.Id;
        var review = await _reviewRepository.GetFirstAsync(r => r.Id == id);
        var userId = _authService.GetAuthorizedUserId();
        
        if (userId != null && userId == review.UserId)
        {
            _reviewRepository.Delete(review);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        else
        {
            throw new WrongRequestException();
        }
        return new DeleteReviewResponse { };
    }
}