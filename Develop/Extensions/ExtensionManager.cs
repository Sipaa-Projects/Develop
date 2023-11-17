using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Develop.Extensions
{
    public class ExtensionManager
    {
        /// <summary>
        /// Loaded extensions
        /// </summary>
        public static List<Assembly> LoadedExtensions { get; set; } = new List<Assembly>();

        /// <summary>
        /// A const defining the extensions directory
        /// </summary>
        public static string ExtensionsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".develop", "Extensions");

        /// <summary>
        /// Event invoked when an extension has been added
        /// </summary>
        public static event EventHandler ExtensionAdded;

        /// <summary>
        /// Load extensions found in the 'Extensions' directory
        /// </summary>
        public static void Initialize()
        {
            DirectoryInfo d = new(ExtensionsDirectory);
            if (d.Exists)
            {
                foreach (FileInfo f in d.GetFiles())
                {
                    try
                    {
                        LoadLibrary(f.FullName);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Copy a DLL into the Extensions folder, then load it
        /// </summary>
        public static bool LoadLibrary(string path)
        {
            try
            {
                DirectoryExtensions.CreateDirectoryRecursively(ExtensionsDirectory);

                Debug.WriteLine($"ExtLoad: Starting loading of assembly '{path}'.");
                if (!path.Contains(":\\") && !path.Contains("/"))
                {
                    Debug.WriteLine("ExtLoad: Path isn't absolute, making it absolute.");
                    path = AppDomain.CurrentDomain.BaseDirectory + path;
                    Debug.WriteLine($"ExtLoad: Path became '{path}'.");
                }

                FileInfo f = new(path);
                Assembly a = Assembly.LoadFile(path);
                bool IsSLLib = false;
                Type slMeta = null;

                foreach (Type type in a.GetTypes())
                {
                    Debug.WriteLine($"Analyzing type '{type.FullName}'");
                    foreach (MethodInfo i in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
                    {
                        Debug.WriteLine($"Analyzing method '{i.Name}'");
                        if (i.GetCustomAttribute(typeof(Develop.EDK.Attributes.ExtMainAttribute)) != null) 
                        { 
                            Debug.WriteLine($"Found entry point '{i.Name}' in assembly '{a.FullName}'");
                            object instance = null;
                            if (!i.IsStatic)
                                instance = Activator.CreateInstance(slMeta);
                            i.Invoke(instance, new object[] { });

                            LoadedExtensions.Add(a);

                            if (ExtensionAdded != null)
                                ExtensionAdded(null, new());

                            File.Copy(path, Path.Combine(ExtensionsDirectory, f.Name), true);

                            Debug.WriteLine(
                                $"ExtLoad: Loaded '{a.GetName().Name}', version {a.GetName().Version.ToString()}"
                            );
                            
                            return true;
                        }
                    }
                }
            
                return false;
            }
            catch (Exception e)
            {
                throw new($"Failed assembly loading: {e.GetType().Name}: {e.Message}");
            }
        }
    }
}
