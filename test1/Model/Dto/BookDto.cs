namespace test1.Model.Dto;

public class AuthorDto
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}


public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
}


public class BooksAuthorsDto
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
}


public class BookEditionDto
{
    public int Id { get; set; }
    public BookDto BookId { get; set; }= null!;
    public string EditionTitle { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public PublishingHouseDto PublishingHouseId { get; set; }
    
}


public class BooksGenresDto
{
    public int BookId { get; set; }
    public int GenreId { get; set; }
}


public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}


public class PublishingHouseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OwnerFirstName { get; set; } = string.Empty;
    public string OwnerLastName { get; set; } = string.Empty;
}

public class CreateBookRequest
{
    public string Title { get; set; }
    public List<int> AuthorIds { get; set; }
}
public class NewBookWithAuthors
{
    public string Title { get; set; }
    public List<int> AuthorIds { get; set; }
}
