﻿using Microsoft.AspNetCore.Identity;

namespace Bel_Souvenirs.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
