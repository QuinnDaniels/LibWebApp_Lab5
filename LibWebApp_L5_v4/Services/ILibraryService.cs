using LibWebApp_L5_v4.Models;
//using Lab5_LibraryApp.Data;

namespace LibWebApp_L5_v4.Services
{
    /// <summary>
    /// Interface for LibraryService
    /// </summary>
    public interface ILibraryService
    {
        public List<User> Users { get; set; }
        public List<Book> Books { get; set; }
        public Dictionary<User, List<Book>> borrowedBooks { get; set; }
        
        
        Task<List<Book>> GetAllBooksAsync();
        Task<List<User>> GetAllUsersAsync();

        public void AddBook(Book book);
        public void AddUser(User user);

        //public void DeleteBook(Book book);



        /*
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int bookId);
        */



    }




}

