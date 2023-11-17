using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Develop.Extensions
{
    public static class DirectoryExtensions
    {
        public static void CreateDirectoryRecursively(string path)
        {
            // Split the path into individual directory names
            string[] directories = path.Split(Path.DirectorySeparatorChar);

            // Build the full path progressively, creating each directory along the way
            for (int i = 0; i < directories.Length; i++)
            {
                string currentPath = string.Join(Path.DirectorySeparatorChar.ToString(), directories, 0, i + 1);
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }
    }
}
