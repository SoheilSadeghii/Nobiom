using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Domain.Enums
{
    public enum EditProfileResult
    {
        Success,
        UserNotFound,
        InvalidInputs,
        DuplicateUserName,
        DuplicateMobile,
        DuplicateEmail,
        DuplicateNationalCode,
        EmailVerificationRequired
    }
}
