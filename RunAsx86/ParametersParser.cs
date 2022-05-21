namespace RunAsx86
{
    internal static class ParametersParser
    {
        public record Result(string Folder, string AssemblyPath, string Error);

        public static Result Parse(string[] args)
        {
            var error = "";
            var folder = "";
            var assemblyPath = "";
            if (!args.Any())
            {
                error = "Please, go to application directory, provide path and arguments.";
            }
            else
            {
                assemblyPath = args.First();
                folder = Path.GetDirectoryName(assemblyPath);
                if (string.IsNullOrEmpty(folder))
                {
                    assemblyPath = Path.Combine(Environment.CurrentDirectory, assemblyPath);
                }

                if (!File.Exists(assemblyPath))
                {
                    assemblyPath = $"{assemblyPath}.exe";
                    if (!File.Exists(assemblyPath))
                    {
                        error = "Can't find executable.";
                    }
                }
            }

            return new Result(folder, assemblyPath, error);
        }
    }
}
