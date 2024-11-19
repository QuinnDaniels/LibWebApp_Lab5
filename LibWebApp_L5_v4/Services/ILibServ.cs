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
        public List<User> ReadUsersOld(string filepath);
        Task<List<User>> ReadUsersOldAsync();


        public List<Book> ReadBooksOld();
        public List<Book> ReadBooksOld(string filepath);
        Task<List<Book>> ReadBooksOldAsync();



        public List<User> ReadUsers(string filePath);
        public void WriteUsersToCsv(string filePath, List<User> users);
        public void WriteUsersToCsv(List<User> users);


        public User makeUser(string name, string email);
        public User makeUser(string[] headerrow);
        public Book makeBook(string inTitle, string inAuthor, string inISBN, int? inCopies);


        public void AppendNewUser(List<User> user);
        public void AppendNewBook(List<Book> book);
        public void AppendNewBook(Book book);
        public void AppendNewUser(User user);
        public void AppendNewUser(string UserName, string UserEmail);
        public void AppendNewBook(string inTitle, string inAuthor, string inISBN, int? inCopies);





        public void EditBook(Book book);
        public void EditBook(int bookId, string? eTitle, string? eAuthor, string? eisbn, string? eCopiesString);

        Task<string> EditUserAsyncResult(int userId, string? eName, string? eEmail);


        public void EditUser(int userId, string? eName, string? eEmail);
        public void EditUser(User user);




        public void BorrowBook(Book inbook, User inuser);
        public void ListBorrowedBooks();
        public List<User> ReadBorrowUsers();
        public List<Book> ReadBorrowedBooksByUser(User? user);
        Task<List<Book>> AsyncReadBorrowedBooksByUser(User? user);


        public void ReturnBook(User? inuser, Book? inbook);


    }




}

