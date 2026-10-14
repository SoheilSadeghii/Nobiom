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

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? EmailActiveCode { get; set; }

        public bool IsEmailActive { get; set; }

        public string? Mobile { get; set; }

        public int MobileActiveCode { get; set; }

        public string Password { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public bool IsActive { get; set; }



        #endregion
    }
}
