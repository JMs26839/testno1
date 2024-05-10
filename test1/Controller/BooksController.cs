using Microsoft.AspNetCore.Mvc;
using test1.Model.Dto;
using test1.Repositories;

namespace test1.Controller;

[Route("api/books/{bookId}/editions")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BooksController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookEditionDto>>> GetBookEditions(int bookId)
    {
        try
        {
            var editions = await _bookRepository.GetBookEditionsAsync(bookId);
            if (editions == null || !editions.Any())
                return NotFound("No editions found for the given book ID.");

            return Ok(editions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving data: " + ex.Message);
        }
    }
    
    


        [HttpPost]
        public async Task<IActionResult> AddBook(string bookTitle, string editionTitle, DateTime releaseDate, int publishingHouseId)
        {
            if (string.IsNullOrEmpty(bookTitle) || string.IsNullOrEmpty(editionTitle))
            {
                return BadRequest("Missing book title or edition title.");
            }

            bool result = await _bookRepository.AddBookWithEditionAsync(bookTitle, editionTitle, releaseDate, publishingHouseId);
            if (result)
            {
                return Ok("Book and its edition added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the book and its edition.");
            }
        }
    }


