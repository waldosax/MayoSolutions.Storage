using System.IO;

namespace MayoSolutions.Storage.Local
{
    internal static class PathExtensions
    {
        public static string SanitizeSimulatedPath(this string path)
        {
            return path.Replace('\\', '/').Trim('\\', '/');
        }

        public static string SanitizeLocalPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).TrimEnd('\\', '/');
        }
    }
}