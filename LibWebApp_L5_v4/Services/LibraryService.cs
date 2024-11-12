//using data
//using models
//using Lab5_LibraryApp.Data;
using LibWebApp_L5_v4.Models;
using LibWebApp_L5_v4.Services;

using CsvHelper;
using CsvHelper.Configuration;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Globalization;

namespace LibWebApp_L5_v4.Services
{
    /// <summary>
    /// Service for Library functions. Implements ILibraryService
    /// </summary>
    public class LibraryService : ILibraryService
    {
        public readonly string _bookPath;
        public readonly string _userPath;

        public List<User> Users { get; set; }
        public List<Book> Books { get; set; }
        public Dictionary<User, List<Book>> borrowedBooks { get; set; }

        /// <summary>
        /// allows for instantiating the list of created Users, Books, borrowedBooks in other classes that use AlbumServices
        /// </summary>
        public LibraryService() 
        {
            _bookPath = Path.Combine(".", "Data", "Books.csv");
            _userPath = Path.Combine(".", "Data", "Users.csv");

            Users = new List<User>();
            Books = new List<Book>();
            borrowedBooks = new Dictionary<User, List<Book>>();
        }





        public async Task<List<User>> GetAllUsersAsync()
        {
            /*
            try
            {*/

            using var reader = new StreamReader(_userPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<User>().ToList();
            return await Task.FromResult(records);
            
            /*}
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }*/
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            using var reader = new StreamReader(_bookPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Book>().ToList();
            return await Task.FromResult(records);
        }


        public void AddBook(Book book)
        {
            throw new NotImplementedException();
        }
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }


        /*
        static void ReadUsers()
        {
            try
            {
                foreach (var line in File.ReadLines("./Data/Users.csv"))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        var user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        */

        /*
        public Task DeleteBookAsync(int bookId)
        {
            throw new NotImplementedException();
        }

        Task AddBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        Task UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
        */


        //public void AddBook(Book) { }
        //public void DeleteBook(Book) { }




        //public LibraryService(string name) { }





        /// <summary>
        /// allows for instantiating the list of created Users, Books, borrowedBooks in other classes that use AlbumServices
        /// </summary>

        /*
        public LibraryService()
        {
            Users = new List<User>();
            Books = new List<Book>();
            borrowedBooks = new Dictionary<User, List<Book>>();
        }
        */
    }
}
