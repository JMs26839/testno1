using test1.Model.Dto;

namespace test1.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookEditionDto>> GetBookEditionsAsync(int bookId);

    Task<bool> AddBookWithEditionAsync(string bookTitle, string editionTitle, DateTime releaseDate, int publishingHouseId);
}