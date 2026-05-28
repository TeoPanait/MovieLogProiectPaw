using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;
using MovieLog.DTOs;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _context.Reviews
                .Select(r => new ReviewDto(r.Id, r.Text, r.Rating, r.MovieId, r.UserId))
                .ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .Select(r => new ReviewDto(r.Id, r.Text, r.Rating, r.MovieId, r.UserId))
                .ToListAsync();
            return Ok(reviews);
        }
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(CreateReviewDto dto)
        {
            var review = new Review
            {
                Text = dto.Text,
                Rating = dto.Rating,
                MovieId = dto.MovieId,
                UserId = dto.UserId
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviews), new { id = review.Id }, new ReviewDto(review.Id, review.Text, review.Rating, review.MovieId, review.UserId));
        }
    }
}
