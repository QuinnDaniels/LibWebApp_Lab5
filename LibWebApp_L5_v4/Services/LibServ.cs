﻿//using data
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



namespace LibWebApp_L5_v4.Services
{
    /// <summary>
    /// Service for Library functions. Implements ILibraryService
    /// </summary>
    public class LibServ : ILibServ
    {

        //        public User CurrentUser { get; set; }



        public Dictionary<User, List<Book>> borrowedBooks { get; set; } = new Dictionary<User, List<Book>>();



        private static string _UserPath()
        { 
            string path = Path.Combine(".", "Data", "Users.csv");
            return path;
        }
        private static string _BookPath()
        {
            string path = Path.Combine(".", "Data", "Books.csv");
            return path;
        }



        public int userCount()
        {
            return ReadUsersOld().Count();
        }

        public int bookCount()
        {
            return ReadBooksOld().Count();
        }


        public int userInc() { return userCount() + 1; }
        public int bookInc() { return bookCount() + 1; }
        public int userDec() { return userCount() + 1; }
        public int bookDec() { return bookCount() + 1; }



        //private readonly string _bookPath = Path.Combine(".", "Data", "Books.csv");
        //private readonly string _userPath = Path.Combine(".", "Data", "Users.csv");


        /// <summary>
        /// intended to do the same as Read methods but without the need to pass a variable
        /// </summary>
        /// <returns></returns>
        public List<User> ReadUsers()
        {
            try
            { 
                using (var reader = new StreamReader(_UserPath()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<User>().ToList();
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
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<User>> ReadUsersAsync()
        {
            try
            {
                using (var reader = new StreamReader(_UserPath()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<User>().ToList();
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
            using (var writer = new StreamWriter(_BookPath()))
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



        /*
        public void AddQuestion(Question toAdd)
        {
            //In reality this should be a write to a file/database
            //or a POST request to an API
            AllQuestions.Add(toAdd);
        }
        */


    }
}
