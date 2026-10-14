using Nobiom.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Domain.Models.Users
{
    public class User : BaseEntity
    {
        #region Properties

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int EmailActiveCode { get; set; }

        public bool IsEmailActive { get; set; }

        public string? Mobile { get; set; }

        public int MobileActiveCode { get; set; }

        public string Password { get; set; }

        public string? Avatar { get; set; }

        public bool IsActive { get; set; }



        #endregion
    }
}
