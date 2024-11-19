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
using CsvHelper.TypeConversion;
using System.IO;
using System.Diagnostics;
using System;
using Microsoft.VisualBasic.FileIO;
using LibWebApp_L5_v4.Components;
using System.Reflection.Metadata;
//using static System.Net.WebRequestMethods;


namespace LibWebApp_L5_v4.Services
{
    /// <summary>
    /// Service for Library functions. Implements ILibraryService
    /// </summary>
    public class LibServ : ILibServ
    {

        // object of service
        public LibServ() { }

        //        public User CurrentUser { get; set; }


        /// <summary>
        /// Dictionary to store <User, List<Book>>.
        /// NOTE: Check functionality of adding records. 
        /// </summary>
        public Dictionary<User, List<Book>> borrowedBooks { get; set; } = new Dictionary<User, List<Book>>();


     //       public LibServ()
     //       {
     //           _pathToUsers = new Path(_UserPath());
     //       }
     //
     //
     //   private userpath_();


        /// <summary>
        /// Simple method that fetches the path to Users.csv. Seemed easier than setting up an accessible variable.
        /// </summary>
        /// <returns>string of path to Users.csv</returns>
        private static string _UserPath()
        { 
            string path = Path.Combine(".", "Data", "Users.csv");
            return path;
        }
        /// <summary>
        /// Simple method that fetches the path to Books.csv. Seemed easier than setting up an accessible variable.
        /// </summary>
        /// <returns>string of path to Books.csv</returns>
        private static string _BookPath()
        {
            string path = Path.Combine(".", "Data", "Books.csv");
            return path;
        }


        /// <summary>
        /// Simple method to check the number of User records in Users.csv. Uses ReadUsersOld()
        /// </summary>
        /// <returns>int of number of users</returns>
        public int userCount()
        {
            return ReadUsersOld().Count();
        }
        /// <summary>
        /// Simple method to check the number of Book records in Books.csv. Uses ReadBooksOld()
        /// </summary>
        /// <returns>int of number of books</returns>
        public int bookCount()
        {
            return ReadBooksOld().Count();
        }

        /// <summary>
        /// Increments the number of Users + 1. Used for setting new User.Id values
        /// </summary>
        /// <returns>int userCount() + 1</returns>
        public int userInc() { return userCount() + 1; }

        /// <summary>
        /// Increments the number of Books + 1. Used for setting new Book.Id values
        /// </summary>
        /// <returns>int bookCount() + 1</returns>
        public int bookInc() { return bookCount() + 1; }

        /// <summary>
        /// Decrements the number of Users - 1. 
        /// </summary>
        /// <returns>int userCount() - 1</returns>
        public int userDec() { return userCount() + 1; }

        /// <summary>
        /// Decrements the number of Books - 1. 
        /// </summary>
        /// <returns>int bookCount() - 1</returns>
        public int bookDec() { return bookCount() + 1; }



        //private readonly string _bookPath = Path.Combine(".", "Data", "Books.csv");
        //private readonly string _userPath = Path.Combine(".", "Data", "Users.csv");
        
        // Custom mapping for CSV without headers
        public sealed class UserMapNoHeaders : ClassMap<User>
        {
            public UserMapNoHeaders()
            {
                Map(m => m.Id).Index(0);
                Map(m => m.Name).Index(1);
                Map(m => m.Email).Index(2);
            }
        }

        /// <summary>
        /// intended to do the same as Read methods but without the need to pass a variable
        /// </summary>
        /// <returns>List of Users </returns>
        public List<User> ReadUsers()
        {
            try
            {
   //             CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
  //              {
 //                   HasHeaderRecord = true
  //              };
                using (var reader = new StreamReader(_UserPath()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    //csv.Configuration.HasHeaderRecord = false;
                csv.Context.RegisterClassMap<UserMapNoHeaders>(); // Use a custom mapping for no-header CSV
                    //csv.HeaderRecord = false;
                    List<User> records = new List<User>();
                    
                    
                    
                    records.Add(makeUser(csv.HeaderRecord));

                    records.AddRange(csv.GetRecords<User>().ToList());
                    return records;
                }
            }
            catch (HeaderValidationException ex)
            {
                // Specific exception for header issues
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                // Specific exception for type conversion issues
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                // General exception for other issues
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }








        /// <summary>
        /// async version of the same thing
        /// </summary>
        /// <returns>Task of List of Users</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<User>> ReadUsersAsync()
        {
            try
            {
                CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    //HasHeaderRecord = true

                };
                using (var reader = new StreamReader(_UserPath()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<UserMapNoHeaders>(); // Use a custom mapping for no-header CSV

                    List<User> records = new List<User>();



                    records.Add(makeUser(csv.HeaderRecord));

                    records.AddRange(csv.GetRecords<User>().ToList());
                    //return records;
                    return await Task.FromResult(records);
                }
            }
            catch (HeaderValidationException ex)
            {
                // Specific exception for header issues
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                // Specific exception for type conversion issues
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                // General exception for other issues
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }

        /// <summary>
        /// Uses original method of reading csv results
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public List<User> ReadUsersOld() 
        {
            List<User> results = new List<User>();
             try
            {
                foreach (var line in File.ReadLines("./Data/Users.csv"))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        User user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        results.Add(user);

                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Uses original method of reading csv results
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public List<User> ReadUsersOld(string filepath)
        {
            List<User> results = new List<User>();
            try
            {
                foreach (var line in File.ReadLines(filepath))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        User user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        results.Add(user);

                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }



        /// <summary>
        /// Async Version of the same thing
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<User>> ReadUsersOldAsync()
        {
            List<User> results = new List<User>();
            try
            {
                foreach (var line in File.ReadLines("./Data/Users.csv"))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        User user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };

                        results.Add(user);

                    }
                }
                return await Task.FromResult(results);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }




        /// <summary>
        /// Read Users From a CSV given a filePath || 
        /// 
        /// https://madhawapolkotuwa.medium.com/how-to-read-and-write-csv-files-in-c-with-csvhelper-47bd0c5fd1b2
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<User> ReadUsers(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<User>().ToList();
                return records;
            }
        }





        /*-------------------     READING - BOOKS      --------------------*/





        public List<Book> ReadBooksOld()
        {
            List<Book> results = new List<Book>();
            try
            {
                foreach (var line in File.ReadLines("./Data/Books.csv"))
                {
                    var fields = line.Split(',');



                    // added because of book number 117
                    if (fields.Length >= 7)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()}, {fields[2].Trim()}, {fields[3].Trim()}",
                            Author = fields[4].Trim(),
                            ISBN = fields[5].Trim(),
                            AvailableCopies = int.Parse(fields[6].Trim())
                        };
                        results.Add(book);
                    }
                    // added due to books with "title"," The "
                    else if (fields.Length >= 6)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()}, {fields[2].Trim()}",
                            Author = fields[3].Trim(),
                            ISBN = fields[4].Trim(),
                            AvailableCopies = int.Parse(fields[5].Trim())
                        };
                        results.Add(book);
                    }
                    else if (fields.Length >= 5)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim(),
                            AvailableCopies = int.Parse(fields[4].Trim())
                        };
                        results.Add(book);
                    }


                }
                return results;


            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }


        public List<Book> ReadBooksOld(string filepath)
        {
            List<Book> results = new List<Book>();
            try
            {
                foreach (var line in File.ReadLines(filepath))
                {
                    var fields = line.Split(',');



                    // added because of book number 117
                    if (fields.Length >= 7)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()}, {fields[2].Trim()}, {fields[3].Trim()}",
                            Author = fields[4].Trim(),
                            ISBN = fields[5].Trim(),
                            AvailableCopies = int.Parse(fields[6].Trim())
                        };
                        results.Add(book);
                    }
                    // added due to books with "title"," The "
                    else if (fields.Length >= 6)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()}, {fields[2].Trim()}",
                            Author = fields[3].Trim(),
                            ISBN = fields[4].Trim(),
                            AvailableCopies = int.Parse(fields[5].Trim())
                        };
                        results.Add(book);
                    }
                    else if (fields.Length >= 5)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim(),
                            AvailableCopies = int.Parse(fields[4].Trim())
                        };
                        results.Add(book);
                    }


                }
                return results;


            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }





        public async Task<List<Book>> ReadBooksOldAsync()
        {
            List<Book> results = new List<Book>();
            try
            {
                foreach (var line in File.ReadLines("./Data/Books.csv"))
                {
                    var fields = line.Split(',');



                    // added because of book number 117
                    if (fields.Length >= 7)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()},{fields[2].Trim()},{fields[3].Trim()}",
                            Author = fields[4].Trim(),
                            ISBN = fields[5].Trim(),
                            AvailableCopies = int.Parse(fields[6].Trim())
                        };
                        results.Add(book);
                    }
                    // added due to books with "title"," The "
                    else if (fields.Length >= 6)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = $"{fields[1].Trim()},{fields[2].Trim()}",
                            Author = fields[3].Trim(),
                            ISBN = fields[4].Trim(),
                            AvailableCopies = int.Parse(fields[5].Trim())
                        };
                        results.Add(book);
                    }
                    else if (fields.Length >= 5)
                    {
                        var book = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim(),
                            AvailableCopies = int.Parse(fields[4].Trim())
                        };
                        results.Add(book);
                    }


                }
                return await Task.FromResult(results);


            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}", ex);
            }
        }











        /*-------------     WRITING - USERS    -----------------*/



        /// <summary>
        /// Write Users to a CSV given a filePath || 
        /// 
        /// https://madhawapolkotuwa.medium.com/how-to-read-and-write-csv-files-in-c-with-csvhelper-47bd0c5fd1b2
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="users"></param>
        public void WriteUsersToCsv(string filePath, List<User> users)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(users);
            }
        }

        /// <summary>
        /// intended to do the same as Write methods but without the need to pass a variable. 
        /// </summary>
        /// <param name="users"></param>
        public void WriteUsersToCsv(List<User> users)
        {
            using (var writer = new StreamWriter(_UserPath()))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(users);
            }
        }


        /*
        public void WriteUsersToCsv(User user)
        {
            using (var writer = new StreamWriter(_UserPath()) )
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(user);
            }
        }
        */


        public User makeUser(string name, string email)
        { 
            User output = new User 
            { 
                Id = userInc(),
                Name = name,
                Email = email
            };
            return output;
        }


        public User makeUser(string[] headerrow)
        {
            //fields = headerrow.Split(',');
            var fields = headerrow;

            //if (fields.Length >= 3) // Ensure there are enough fields
            //{
                User user = new User
                {
                    Id = int.Parse(fields[0].Trim()),
                    Name = fields[1].Trim(),
                    Email = fields[2].Trim()
                };

                return user;
            //}
        }

        public Book makeBook(string inTitle, string inAuthor, string inISBN, int? inCopies)
        {
            int copies;
            if (inCopies == null || inCopies <= 0)  { copies = 1; }
            else { copies = (int)inCopies; }
            Book output = new Book
            {
               
                Id = bookInc(),
                Title = inTitle,
                Author = inAuthor,
                ISBN = inISBN,
                AvailableCopies = copies
            };
            return output;
        }


        public void AppendNewUser(List<User> user)
        { 
            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };
            using (var stream = File.Open(_UserPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(user);
            }
        
        
        }


        public void AppendNewUser(User user)
        {
            List<User> adder = new List<User>();
            adder.Add(user);


            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(_UserPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(adder);
            }


        }



        public void AppendNewUser(string UserName, string UserEmail)
        {
            List<User> adder = new List<User>
            {
                //make new User
                new User
                    {
                        Id = userInc(),
                        Name = UserName,
                        Email = UserEmail
                    },
            };

            //List<User> adder = new List<User>();
            //adder.Add(user);


            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(_UserPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(adder);
            }


        }







        public void AppendNewBook(List<Book> book)
        {
            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(_BookPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(book);
            }


        }


        public void AppendNewBook(Book book)
        {
            List<Book> adder = new List<Book>();
            adder.Add(book);

            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(_BookPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(adder);
            }


        }

        public void AppendNewBook(string inTitle, string inAuthor, string inISBN, int? inCopies)
        {
            int copies;
            if (inCopies == null || inCopies <= 0)
            {
                copies = 1;
            }
            else { copies = (int)inCopies; }

            List<Book> adder = new List<Book>
            {
                //make new Book
                new Book
                    {
                        Id = bookInc(),
                        Title = inTitle,
                        Author = inAuthor,
                        ISBN = inISBN,
                        AvailableCopies = copies
                    },
            };

            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(_BookPath(), FileMode.Append))           // ("path\\to\\file.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(adder);
            }


        }





        /*
        public void AddQuestion(Question toAdd)
        {
            //In reality this should be a write to a file/database
            //or a POST request to an API
            AllQuestions.Add(toAdd);
        }
        */











        public void EditUser(int userId, string? eName, string? eEmail)
        {

            List<User> uList = ReadUsersOld();
            //Console.Write("\nEnter User ID to Edit: ");

            //if (int.TryParse(Console.ReadLine(), out int userId))
            //{
            try { 
                User? user = uList.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    Console.Write("Enter new Name (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(eName)) user.Name = eName;

                    Console.Write("Enter new Email (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(eEmail)) user.Email = eEmail;


                    string tID = userId.ToString();

                    string tPath = _UserPath();
                    List<String> lines = new List<String>();
                    if (File.Exists(tPath)) ;
                    {
                        using (StreamReader reader = new StreamReader(tPath))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[0].Contains(tID))
                                    {
                                        split[1] = user.Name;
                                        split[2] = user.Email;
                                        line = String.Join(",", split);
                                    }
                                }

                                lines.Add(line);
                            }
                        }

                        using (StreamWriter writer = new StreamWriter(tPath, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }

                        Console.WriteLine("User updated successfully!\n");
                    }
                }
            }
                /*    else
                  {
                        Console.WriteLine("User not found!\n");
                }*/
                catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}. User not found", ex);
            }
        }





        public void EditUser(User inuser)
        {

            List<User> uList = ReadUsersOld();
            //Console.Write("\nEnter User ID to Edit: ");

            //if (int.TryParse(Console.ReadLine(), out int userId))
            //{
            try
            {
                User? user = uList.FirstOrDefault(u => u.Id == inuser.Id);

                if (user != null)
                {
                    //Console.Write("Enter new Name (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(inuser.Name)) user.Name = inuser.Name;

                    //Console.Write("Enter new Email (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(inuser.Email)) user.Email = inuser.Email;


                    string tID = inuser.Id.ToString();

                    string tPath = _UserPath();
                    List<String> lines = new List<String>();
                    if (File.Exists(tPath)) 
                    {
                        using (StreamReader reader = new StreamReader(tPath))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[0].Contains(tID))
                                    {
                                        split[1] = user.Name;
                                        split[2] = user.Email;
                                        line = String.Join(",", split);
                                    }
                                }

                                lines.Add(line);
                            }
                        }

                        using (StreamWriter writer = new StreamWriter(tPath, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }

                        Console.WriteLine("User updated successfully!\n");
                    }
                }
            }
            /*    else
              {
                    Console.WriteLine("User not found!\n");
            }*/
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}. User not found", ex);
            }
        }







        public async Task<string> EditUserAsyncResult(int userId, string? eName, string? eEmail)
        {

            List<User> uList = ReadUsersOld();

            try
            {
                User? user = uList.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    //Console.Write("Enter new Name (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(eName)) user.Name = eName;

                    //Console.Write("Enter new Email (leave blank to keep current): ");
                    if (!string.IsNullOrEmpty(eEmail)) user.Email = eEmail;


                    string tID = userId.ToString();

                    string tPath = _UserPath();
                    List<String> lines = new List<String>();
                    if (File.Exists(tPath)) ;
                    {
                        using (StreamReader reader = new StreamReader(tPath))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    if (split[0].Contains(tID))
                                    {
                                        split[1] = user.Name;
                                        split[2] = user.Email;
                                        line = String.Join(",", split);
                                    }
                                }

                                lines.Add(line);
                            }
                        }

                        using (StreamWriter writer = new StreamWriter(tPath, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }

                        return await Task.FromResult("User updated successfully!");
                    } 
                }
                return await Task.FromResult("User not found!");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}.", ex);
            }
        }








        public void EditBook(Book inbook)
        {

            //bool eCS = int.TryParse(eCopiesString, out int eCopies);


            List<Book> bList = ReadBooksOld();
            //Console.Write("\nEnter User ID to Edit: ");

            try
            {
                Book? book = bList.FirstOrDefault(b => b.Id == inbook.Id);

                if (book != null)
                {
                    //Console.Write("Enter new Name (leave blank to keep current): ");

                    if (!string.IsNullOrEmpty(inbook.Title )) book.Title = inbook.Title ;       //book+inbook==
                    if (!string.IsNullOrEmpty(inbook.Author )) book.Author = inbook.Author ;
                    if (!string.IsNullOrEmpty(inbook.ISBN )) book.ISBN = inbook.ISBN ;
                    if (inbook.AvailableCopies > -1) book.AvailableCopies = inbook.AvailableCopies; //inbook2, book1 --> book2
                    
                    //if (eCS) book.AvailableCopies = eCopies;


                    int tID = inbook.Id;

                    string tPath = _BookPath();
                    List<String> lines = new List<String>();


                    if (File.Exists(tPath))                     // move into prev conditional check
                    {                                               // book and inbook dissappear??

                        String[] tSplit = book.Title.Split(',');



                        using (TextFieldParser csvParser = new TextFieldParser(tPath))
                        {
                            //csvParser.CommentTokens = new string[] { "#" };
                            csvParser.SetDelimiters(new string[] { "," });
                            csvParser.HasFieldsEnclosedInQuotes = false; //true;

                            // Skip the row with the column names
                            //csvParser.ReadLine();
                            string line ="";
                            //bool matchFound = false;
                            while (!csvParser.EndOfData ) //&& !matchFound)
                            {
                                // Read current line fields, pointer moves to the next line.
                                string[] fields = csvParser.ReadFields();
                                List<string> strings = new List<string>();

                                if (fields[0].Trim() == tID.ToString())
                                {
                                    int i = 1;
                                    string tits = String.Join(",", tSplit);
                                    /*
                                    //if (tSplit.Length > 1)
                                    //{
                                    //                                        foreach (string s in tSplit)
                                    //                                       {
                                    //                                          fields[i] = s.Trim();
                                    //tits = tits + "," + s.Trim();
                                    //                                      }

                                    //}
                                    //else { tits = book.Title; }

                                    //                                    fields[tSplit.Length + 1] = tits; //book.Title;
                                    //                                    fields[tSplit.Length + 2] = book.Author;
                                    //                                    fields[tSplit.Length + 3] = book.ISBN;
                                    //                                    fields[tSplit.Length + 4] = $"{book.AvailableCopies}"; //.ToString(); // caught here

                                    //line = String.Join(",", fields);
                                    //matchFound = true;
                                    */
                                    
                                    
                                    line = $"{fields[0].Trim()},{tits},{book.Author},{book.ISBN},{book.AvailableCopies}"; // I AM SO UPSET THAT THIS WORKS

                                }

                                else { line = String.Join(",", fields); }
                                lines.Add(line);




                            }
                        }




                        /*

                            using (StreamReader reader = new StreamReader(tPath))
                            {

                                foreach (string liner in reader.ReadLine())
                                { 
                                String line = liner.ToString();

     //                           }
     //
     //                           while ((line = reader.ReadLine()) != null) //only gets line 1
     //                           {
                                    if (line.Contains(","))
                                    {
                                        String[] split = line.Split(',');



                                        if (split[0].Trim() == tID.ToString())
                                        {
                                            int i = 0;
                                            foreach (string s in tSplit)
                                            {
                                                split[i++] = s.Trim();
                                            }
                                            split[tSplit.Length + 1] = book.Title;
                                            split[tSplit.Length + 2] = book.Author;
                                            split[tSplit.Length + 3] = book.ISBN;
                                            split[tSplit.Length + 4] = book.AvailableCopies.ToString();
                                            line = String.Join(",", split);
                                        }
                                    }
                                    lines.Add(line);
                                }
                            }
                        */


                        using (StreamWriter writer = new StreamWriter(tPath, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }

                        Console.WriteLine("Book updated successfully!\n");


                    }








                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}. Book not found", ex);
            }
        }











        public void EditBook(int bookId, string? eTitle, string? eAuthor, string? eisbn, string? eCopiesString)
        {

            bool eCS = int.TryParse(eCopiesString, out int eCopies);
            

            List<Book> bList = ReadBooksOld();
            //Console.Write("\nEnter User ID to Edit: ");

            //if (int.TryParse(Console.ReadLine(), out int userId))
            //{
            try
            {
                Book? book = bList.FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    Console.Write("Enter new Name (leave blank to keep current): ");
                    
                    if (!string.IsNullOrEmpty(eTitle)) book.Title = eTitle;
                    if (!string.IsNullOrEmpty(eAuthor)) book.Author = eAuthor;
                    if (!string.IsNullOrEmpty(eisbn)) book.ISBN = eisbn;
                    if (eCS) book.AvailableCopies = eCopies;


                    string tID = bookId.ToString();

                    string tPath = _BookPath();
                    List<String> lines = new List<String>();
                    if (File.Exists(tPath)) ;
                    {
                        using (StreamReader reader = new StreamReader(tPath))
                        {
                            String line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains(","))
                                {
                                    String[] split = line.Split(',');

                                    String[] tSplit = book.Title.Split(',');


                                    if (split[0].Contains(tID))
                                    {
                                        int i = 0;
                                        foreach (string s in tSplit)
                                        {
                                            split[i++] = s.Trim();
                                        }
                                        split[ tSplit.Length + 1 ] = book.Title;
                                        split[ tSplit.Length + 2 ] = book.Author;
                                        split[ tSplit.Length + 3 ] = book.ISBN;
                                        split[ tSplit.Length + 4 ] = book.AvailableCopies.ToString();
                                        line = String.Join(",", split);
                                    }
                                }
                                lines.Add(line);
                            }
                        }

                        using (StreamWriter writer = new StreamWriter(tPath, false))
                        {
                            foreach (String line in lines)
                                writer.WriteLine(line);
                        }

                        Console.WriteLine("Book updated successfully!\n");
                    }
                }
            }
            /*    else
              {
                    Console.WriteLine("Book not found!\n");
            }*/
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}. Book not found", ex);
            }
        }








        /*------------------------------------------------------------------------*/


        public void BorrowBook(Book inbook, User inuser)
        {

            //ListBooks();
            List<Book> bList = ReadBooksOld();
            List<User> uList = ReadUsersOld();

            //Console.Write("\nEnter Book ID to Borrow: ");

            //if (int.TryParse(Console.ReadLine(), out int bookId))
            //{
            try 
            { 
                Book? book = bList.FirstOrDefault(b => b.Id == inbook.Id);

                if (book != null && book.AvailableCopies > 0 && bList.Count(b => b.Id == inbook.Id) > 0)
                {

                    //ListUsers();
                    Console.Write("\nEnter User ID who is borrowing the book: ");
                    

                    try
                    {
                        User? user = uList.FirstOrDefault(u => u.Id == inuser.Id);
                        //if (int.TryParse(Console.ReadLine(), out int userId))
                        //{

                            //User user = users.FirstOrDefault(u => u.Id == userId);

                            if (user != null && uList.Count(u => u.Id == inuser.Id) > 0) //)
                            {
                                if (!borrowedBooks.ContainsKey(user))
                                {
                                    borrowedBooks[user] = new List<Book>();
                                }
                                borrowedBooks[user].Add(book);

                                book.AvailableCopies = book.AvailableCopies - 1;


                            /*----------*
                             *
                             /*
                            // only remove book if no more copies are available
                            if (book.AvailableCopies <= 0)
                            {
                                books.Remove(book);
                            }
                             */
                            /*-----------*/

                            EditBook(book); // update the AvailableCopies



                                Console.WriteLine("Book borrowed successfully!\n");
                            //}
                            //else
                            //{
                            //    Console.WriteLine("User not found!\n");
                            //}
                        }
                        else
                        {
                            Console.WriteLine("Invalid input!\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"An error occurred: {ex.Message}. User not found", ex);
                    }

                }
                else
                {
                    Console.WriteLine("Book not found or no available copies!\n");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}. Book not found", ex);
            }
            //}
           //else
            //{
            //    Console.WriteLine("Invalid input!\n");
            //}
        }


        /*
        static void ReturnBook()
        {

            ListBorrowedBooks();
            Console.Write("\nEnter User ID to return a book for: ");

            if (int.TryParse(Console.ReadLine(), out int userId))
            {

                User user = users.FirstOrDefault(u => u.Id == userId);

                if (user != null && borrowedBooks.ContainsKey(user) && borrowedBooks[user].Count > 0)
                {

                    Console.WriteLine("Borrowed Books:");

                    for (int i = 0; i < borrowedBooks[user].Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {borrowedBooks[user][i].Id}. {borrowedBooks[user][i].Title} by {borrowedBooks[user][i].Author} (ISBN: {borrowedBooks[user][i].ISBN})");
                    }

                    Console.Write("\nEnter the number of the book to return: ");

                    if (int.TryParse(Console.ReadLine(), out int bookNumber) && bookNumber >= 1 && bookNumber <= borrowedBooks[user].Count)
                    {

                        Book bookToReturn = borrowedBooks[user][bookNumber - 1];

                        borrowedBooks[user].RemoveAt(bookNumber - 1);

                        bool bookFound = false;
                        foreach (Book b in books)
                        {
                            // see if there exists a matching book
                            if (b.Title == bookToReturn.Title && b.Author == bookToReturn.Author && b.ISBN == bookToReturn.ISBN)
                            {
                                b.AvailableCopies += 1;
                                bookFound = true;
                            }
                        }
                        if (bookFound == false)
                        {
                            foreach (Book b in books)
                            {
                                if (b.Id == bookToReturn.Id)
                                {
                                    bookToReturn.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
                                    Console.WriteLine("bookId conflict! updated returning bookID");
                                }
                            }

                            bookToReturn.AvailableCopies = 1;
                            books.Add(bookToReturn);
                        }

                        Console.WriteLine("Book returned successfully!\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!\n");
                    }
                }
                else
                {
                    Console.WriteLine("User not found or no borrowed books!\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input!\n");
            }
        }
        */



        
        public void ReturnBook(User? inuser, Book? inbook)
        {

            //ListBorrowedBooks();
            //Console.Write("\nEnter User ID to return a book for: ");

            List<Book> bList = ReadBooksOld();
            List<User> uList = ReadUsersOld();
            List<Book> bbLister = ReadBorrowedBooksByUser(inuser);

            //if (int.TryParse(Console.ReadLine(), out int userId))
            //{
            try
            { 

                //User user = uList.FirstOrDefault(u => u.Id == inuser.Id);

                //List<User> ushortlist = uList.FindAll(u => u.Id == inuser.Id);


                //foreach ()


                if (inuser != null && borrowedBooks.ContainsKey(inuser))// && borrowedBooks[user].Count > 0)
                {

                    Console.WriteLine("Borrowed Books:");
                    int i = 0;
                    foreach(Book booker in bbLister)//borrowedBooks[inuser]) //for (int i = 0; i < borrowedBooks[user].Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {booker.Id}. {booker.Title} by {booker.Author} (ISBN: {booker.ISBN})");
                        //                        Console.WriteLine($"{i + 1}. {borrowedBooks[user][i].Id}. {borrowedBooks[user][i].Title} by {borrowedBooks[user][i].Author} (ISBN: {borrowedBooks[user][i].ISBN})");
                        i = i + 1;
                    }

                    Console.Write("\nEnter the number of the book to return: ");




                    try 
                    {
                        Book? book = bbLister.FirstOrDefault(b => b.Id == inbook.Id);
                        if (book != null)
                        {
                            Book bookToReturn = book;

                            //bool loop1 = false;
                            //bool loop2 = false;
                            bool condition = false;
                            try
                            {
                                User ucut = new User();
                                Book bcut = new Book();
                                foreach (User uloop in borrowedBooks.Keys)
                                {
                                    foreach (Book bloop in borrowedBooks[uloop])
                                    {
                                        if (bloop == book)
                                        {
                                            Console.WriteLine("book match found");
                                            //borrowedBooks[uloop].Remove(bloop);
                                            ucut = uloop;
                                            bcut = bloop;
                                            condition = true;
                                        }
                                    }
                                    Console.WriteLine("bookloop finished");
                                }
                                Console.WriteLine("userloop finished");
                                
                                borrowedBooks[ucut].Remove(bcut);

                                if (borrowedBooks[ucut].Count == 0)
                                { 
                                    borrowedBooks.Remove(ucut);
                                    Console.WriteLine($"Removed {ucut} from list of borrowing users");
                                }

                                i = 0;
                                List<Book> bbListly = ReadBorrowedBooksByUser(inuser);
                                foreach (Book booker in bbListly)
                                {
                                    Console.WriteLine($"{i + 1}. {booker.Id}. {booker.Title} by {booker.Author} (ISBN: {booker.ISBN})");
                                    i = i + 1;
                                }
                        
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException($"userbook loop error. An error occurred: {ex.Message}.", ex);
                            }

                            //borrowedBooks[inuser].Remove(bookToReturn);
                            bool bookFound = false;


                            foreach (Book b in bbLister)
                            {
                                // see if there exists a matching book
                                if (b.Title == bookToReturn.Title && b.Author == bookToReturn.Author && b.ISBN == bookToReturn.ISBN)
                                {
                                    b.AvailableCopies += 1;
                                    bookFound = true;

                                    EditBook(b); // update the AvailableCopies
                                }
                            }

                            if (bookFound == false)
                            {
                                foreach (Book b in bbLister)
                                {
                                    if (b.Id == bookToReturn.Id)
                                    {
                                        bookToReturn.Id = bbLister.Any() ? bbLister.Max(b => b.Id) + 1 : 1;
                                        Console.WriteLine("bookId conflict! updated returning bookID");


                                    }
                                }

                                bookToReturn.AvailableCopies = 1;
                                //List<Book> adder = new List<Book>();
                                //adder.Add(bookToReturn);
                                AppendNewBook(bookToReturn);
                            }



                            

                            Console.WriteLine("Book updated successfully!\n");


                        }
                        else
                        {
                            Console.WriteLine("Invalid book input!\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"An error occurred: {ex.Message}. Book not found", ex);
                    }
                }
                else
                {
                    Console.WriteLine("User not found or no borrowed books!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid input!\n");
                throw new ApplicationException($"An error occurred: {ex.Message}. User not found", ex);
            }
        }




        


        public void ListBorrowedBooks()
        {

            Console.WriteLine("\nBorrowed Books:");

            foreach (var entry in borrowedBooks)
            {
                Console.WriteLine($"User: {entry.Key.Name}");

                foreach (var book in entry.Value)
                {
                    Console.WriteLine($"{book.Title} by {book.Author} (ISBN: {book.ISBN})");
                }

                Console.WriteLine();
            }
        }

        public List<User> ReadBorrowUsers()
        {
            List<User> outputs = new List<User>(borrowedBooks.Keys);
            foreach (var key in borrowedBooks.Keys)
            {
            
                //if (!outputs.Contains(key))
                //{
                    outputs.Add(key);
                //}
            }

            List<User> putouts = new List<User>();
            putouts = outputs.DistinctBy(u => u.Id).ToList();           //https://josipmisko.com/posts/c-sharp-unique-list

            //List<User> outs = outputs.Distinct().ToList();
            return putouts;//.Distinct;
        }

        public List<Book> ReadBorrowedBooksByUser(User? user)
        {
            //if (user != null)
            //{ 
            //}
            try 
            {
                List<Book> outputs = new List<Book>();
                if (borrowedBooks.ContainsKey(user)) 
                {

                    foreach (User userer in borrowedBooks.Keys)
                    {
                        if (userer.Id == user.Id)
                        {
                            foreach (var booker in borrowedBooks[userer])
                            {
                                outputs.Add(booker);
                            }
                        }
                    }

                    //outputs = borrowedBooks[user];
                }
                return outputs;
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}.", ex);
            }
        }




        public async Task<List<Book>> AsyncReadBorrowedBooksByUser(User? user)
        {
            //if (user != null)
            //{ 
            //}
            try
            {
                List<Book> outputs = new List<Book>();
                if (borrowedBooks.ContainsKey(user))
                {
                    foreach (User userer in borrowedBooks.Keys)
                    {
                        if (userer.Id == user.Id)
                        {
                            foreach (var booker in borrowedBooks[userer])
                            {
                                outputs.Add(booker);
                            }
                        }
                    }
                }
                return await Task.FromResult(outputs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred: {ex.Message}.", ex);
            }
        }








    }
}
