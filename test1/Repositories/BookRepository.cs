using System.Data;
using Microsoft.Data.SqlClient;
using test1.Model.Dto;

namespace test1.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;


    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<BookEditionDto>> GetBookEditionsAsync(int bookId)
    {
        var query = @"SELECT 
            Book.ID AS BookID,
            Book.Title AS BookTitle,
            Edition.ID AS EditionID,
            Edition.Title AS EditionTitle,
            Edition.ReleaseDate AS EditionReleaseDate,
            PublishingHouse.ID AS PublishingHouseID,
            PublishingHouse.Name AS PublishingHouseName,
            OwnerFirstName,
            OwnerLastName
        FROM Book
        INNER JOIN BookEdition ON BookEdition.BookId = Book.ID
        INNER JOIN Edition ON Edition.ID = BookEdition.EditionID
        INNER JOIN PublishingHouse ON PublishingHouse.ID = BookEdition.PublishingHouseId
        WHERE Book.ID = @BookID";

        
        
       

        var editions = new List<BookEditionDto>();

        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@BookID", bookId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        var BookId = reader.GetOrdinal("BookId");
        var admissionDateOrdinal = reader.GetOrdinal("EditionDate");
        while (await reader.ReadAsync())
        {
            editions.Add(new BookEditionDto()
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                BookId= new BookDto()
                {
                    BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                    Title= reader.GetString(reader.GetOrdinal("Title"))
                        
                },
                EditionTitle  = reader.GetString(reader.GetOrdinal("EditionTitle")),
                ReleaseDate = reader.GetDateTime(admissionDateOrdinal),
                PublishingHouseId = new PublishingHouseDto()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("BookId")),
                    Name= reader.GetString(reader.GetOrdinal("Name")),
                    OwnerFirstName = reader.GetString(reader.GetOrdinal("OwnerFirstName")),
                    OwnerLastName = reader.GetString(reader.GetOrdinal("OwnerLastName")),  
                }
            });
        }

        return editions; 
        }

       
    
    public async Task<bool> AddBookWithEditionAsync(string bookTitle, string editionTitle, DateTime releaseDate, int publishingHouseId)
    {
        var queryBook = @"INSERT INTO Book (Title) OUTPUT INSERTED.ID VALUES (@Title);";
        var queryEdition = @"INSERT INTO Edition (BookId, Title, ReleaseDate, PublishingHouseId) VALUES (@BookId, @Title, @ReleaseDate, @PublishingHouseId);";

        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();
        await using var commandBook = new SqlCommand(queryBook, connection, transaction);
        commandBook.Parameters.AddWithValue("@Title", bookTitle);

        try
        {
            // Insert book and get the generated ID
            var bookId = (int)await commandBook.ExecuteScalarAsync();

            // Insert edition with the new Book ID
            await using var commandEdition = new SqlCommand(queryEdition, connection, transaction);
            commandEdition.Parameters.AddWithValue("@BookId", bookId);
            commandEdition.Parameters.AddWithValue("@Title", editionTitle);
            commandEdition.Parameters.AddWithValue("@ReleaseDate", releaseDate);
            commandEdition.Parameters.AddWithValue("@PublishingHouseId", publishingHouseId);
            await commandEdition.ExecuteNonQueryAsync();

            // Commit transaction
            transaction.Commit();
            return true;
        }
        catch (Exception)
        {
            // Rollback transaction in case of an error
            transaction.Rollback();
            return false;
        }
    }


}