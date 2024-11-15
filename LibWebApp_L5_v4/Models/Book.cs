using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibWebApp_L5_v4.Models{
    /// <summary>
    /// Data Model for a Book item. To be used with Books.csv
    /// </summary>
    public class Book {
        
        /// <summary>
        /// integer ID. Primary Key to identify a Book object. Should auto increment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Title of the book. Code should be able to handle if a Title is formatted as "Title", "The ", or other comma variations
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of a book's Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// a string that should represent a books ISBN
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// QD - an integer to track how many available copies of a book there are. 
        /// </summary>
        public int AvailableCopies { get; set; }


        


        //public Book(int id, string title, string author, string iSBN, int availableCopies)
        //{
        //    Id = id;
        //    Title = title;
        //    Author = author;
        //    ISBN = iSBN;
        //    AvailableCopies = availableCopies;
        //}


    }
}
