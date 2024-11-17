using LibWebApp_L5_v4.Models;
//using Lab5_LibraryApp.Data;

namespace LibWebApp_L5_v4.Services
{
    /// <summary>
    /// Interface for LibraryService
    /// </summary>
    public interface ILibServ
    {

        //        public static string _UserPath();

        //        public User CurrentUser { get; set; }

        public Dictionary<User, List<Book>> borrowedBooks { get; set; }


        public int userCount();
        public int bookCount();
        public int userInc();
        public int bookInc();
        public int userDec();
        public int bookDec();



        public List<User> ReadUsers();
        Task<List<User>> ReadUsersAsync();
        public List<User> ReadUsersOld();
        Task<List<User>> ReadUsersOldAsync();


        public List<Book> ReadBooksOld();
        Task<List<Book>> ReadBooksOldAsync();



        public List<User> ReadUsers(string filePath);
        public void WriteUsersToCsv(string filePath, List<User> users);
        public void WriteUsersToCsv(List<User> users);


        public void AppendNewUser(List<User> user);
        public void AppendNewBook(List<Book> book);




        public void EditBook(Book book);
        public void EditBook(int bookId, string? eTitle, string? eAuthor, string? eisbn, string? eCopiesString);

        Task<string> EditUserAsyncResult(int userId, string? eName, string? eEmail);


        public void EditUser(int userId, string? eName, string? eEmail);
        public void EditUser(User user);




        public void BorrowBook(Book inbook, User inuser);

    }




}

