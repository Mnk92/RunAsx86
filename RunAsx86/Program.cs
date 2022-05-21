using System.Reflection;

namespace RunAsx86
{
    static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var parsed = ParametersParser.Parse(args);
                if (parsed.Error.Length > 0)
                {
                    Console.WriteLine(parsed.Error);
                }
                else
                {
                    AppDomain.CurrentDomain.AssemblyResolve += (_, e) => LoadFromSameFolder(e, parsed.Folder);
                    var assembly = Assembly.LoadFile(parsed.AssemblyPath);

                    if (assembly.EntryPoint == null)
                    {
                        Console.WriteLine("Can't find entry point.");
                    }
                    else
                    {
                        var result = assembly.EntryPoint.Invoke(assembly.EntryPoint, new object[] { args.Skip(1).ToArray() });
                        if (result != null)
                        {
                            return (int)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return -1;
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
