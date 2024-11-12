using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibWebApp_L5_v4.Models{
    /// <summary>
    /// Data Model for a User item. To be used with Users.csv
    /// </summary>
    public class User {

        /// <summary>
        /// integer ID. Primary Key to identify a User object. Should auto increment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A User's Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A User's Email Address
        /// </summary>
        public string Email { get; set; }




        //public User(int id, string name, string email)
        //{
        //    Id = id;
        //    Name = name;
        //    Email = email;
        //}

    }
}
