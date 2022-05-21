using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RunAsx86
{
    static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (!args.Any())
                {
                    throw new ArgumentException("Please, go to application directory, provide path and arguments.");
                }
                var path = args.First();
                var folder = Path.GetDirectoryName(path);
                var target = Prepare(
                    string.IsNullOrEmpty(folder) ? Path.Combine(Environment.CurrentDirectory, path) : path);

                AppDomain.CurrentDomain.AssemblyResolve += (_, e) => LoadFromSameFolder(e, folder);
                var assembly = Assembly.LoadFile(target);

                if (assembly.EntryPoint == null)
                {
                    Console.WriteLine("Can't find entry point.");
                    return -1;
                }
                var result = assembly.EntryPoint.Invoke(assembly.EntryPoint, new object[] { args.Skip(1).ToArray() });
                if (result != null)
                {
                    return (int)result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        private static string Prepare(string path)
        {
            if (File.Exists(path)) return path;
            if (File.Exists(path + ".exe")) return $"{path}.exe";
            throw new ArgumentException("Can't find executable.");
        }

        private static Assembly LoadFromSameFolder(ResolveEventArgs args, string folder)
        {
            string assemblyPath;
            if (!string.IsNullOrEmpty(folder))
            {
                assemblyPath = Path.Combine(folder, new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                    return Assembly.LoadFrom(assemblyPath);
            }
            assemblyPath = Path.Combine(Environment.CurrentDirectory, new AssemblyName(args.Name).Name + ".dll");
            return File.Exists(assemblyPath) ?
                Assembly.LoadFrom(assemblyPath) :
                null;
        }
    }
}
