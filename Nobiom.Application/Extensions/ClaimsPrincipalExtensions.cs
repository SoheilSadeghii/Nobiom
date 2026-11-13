using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Nobiom.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            return user.FindFirst(claimType)?.Value;
        }

        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.NameIdentifier);
        }

        public static string? GetUserName(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.Name);
        }

        public static string? GetFirstName(this ClaimsPrincipal user)
        {
            return user.GetClaimValue("FirstName");
        }

        public static string? GetLastName(this ClaimsPrincipal user)
        {
            return user.GetClaimValue("LastName");
        }

        public static string? GetFullName(this ClaimsPrincipal user)
        {
            return user.GetClaimValue("FullName");
        }

        public static string? GetAvatar(this ClaimsPrincipal user)
        {
            return user.GetClaimValue("Avatar") ?? "default.png";
        }

        public static string? GetMobile(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.MobilePhone) ?? "شماره تلفن ثبت نشده.";
        }

        public static string? GetEmail(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(ClaimTypes.Email);
        }
    }
}
