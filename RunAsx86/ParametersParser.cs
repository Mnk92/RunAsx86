namespace RunAsx86
{
    internal static class ParametersParser
    {
        public record Result(string Folder, string AssemblyPath);

        public static Result Parse(string[] args)
        {
            if (!args.Any())
            {
                throw new ArgumentException("Please, go to application directory, provide path and arguments.");
            }

            var assemblyPath = args.First();
            var folder = Path.GetDirectoryName(assemblyPath);
            if (string.IsNullOrEmpty(folder))
            {
                assemblyPath = Path.Combine(Environment.CurrentDirectory, assemblyPath);
            }

            if (!File.Exists(assemblyPath))
            {
                assemblyPath = $"{assemblyPath}.exe";
                if (!File.Exists(assemblyPath))
                {
                    throw new ArgumentException("Can't find executable.");
                }
            }

            return new Result(folder, assemblyPath);
        }
    }
}
