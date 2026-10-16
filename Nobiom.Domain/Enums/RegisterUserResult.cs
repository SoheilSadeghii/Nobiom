using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Domain.Enums
{
    public enum RegisterUserResult
    {
        Success,
        InvalidInputs,
        EmailDuplicated,
        UserNameDuplicated,
        SendActivationEmail,
        Failed
    }
}
