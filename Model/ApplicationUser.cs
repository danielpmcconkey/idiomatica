﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Model
{
    /// <summary>
    /// this is purely the user login
    /// </summary>
    // Add profile data for application users by adding properties to the ApplicationUser class
    [Table("AspNetUsers", Schema = "dbo")]
    public class ApplicationUser : IdentityUser
    {
    }
}
