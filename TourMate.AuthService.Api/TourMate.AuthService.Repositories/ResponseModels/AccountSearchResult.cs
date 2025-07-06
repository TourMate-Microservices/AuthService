﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourMate.AuthService.Repositories.ResponseModels
{
    public class AccountSearchResult
    {
        public int AccountId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
