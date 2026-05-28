using MovieLog.DTOs;
using MovieLog.Models;
using MovieLog.Repositories;

namespace MovieLog.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.ReviewRepository.GetAllAsync(cancellationToken);
        // mapam riview in review dto
        return reviews.Select(r => new ReviewDto(r.Id, r.Text, r.Rating, r.MovieId, r.UserId));
    }

    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var review = new Review
        {
            Text = dto.Text,
            Rating = dto.Rating,
            MovieId = dto.MovieId,
            UserId = dto.UserId
        };

        await _unitOfWork.ReviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewDto(review.Id, review.Text, review.Rating, review.MovieId, review.UserId);
    }
}