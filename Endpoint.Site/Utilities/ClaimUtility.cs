﻿using System.Security.Claims;

namespace Endpoint.Site.Utilities
{
    public static class ClaimUtility
    {
        public static long? GetUserId(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                long userId = long.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                return userId;
            }
            catch(Exception)
            {
                return null;
            }
            
        }

        public static string GetUserEmail(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                string email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
                return email;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static List<string> GetRoles(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                List<string> roles = new List<string>();
                foreach(var item in claimsIdentity.Claims.Where(p => p.Type.EndsWith("role")))
                {
                    roles.Add(item.Value);
                }
                return roles;
            }catch (Exception)
            {
                return null;
            }
        }
    }
}
