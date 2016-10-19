using System.Collections.Generic;

namespace ProiectColectiv.Core.Constants
{
    public class Roles
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string CONTRIBUTOR = "Contributor";
        public const string MANAGER = "Manager";
        public const string CITITOR = "Cititor";
        public static IEnumerable<string> AllRoles = new List<string> { ADMINISTRATOR, CITITOR, CONTRIBUTOR, MANAGER };
    }
}
