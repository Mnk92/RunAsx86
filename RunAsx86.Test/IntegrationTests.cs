using System.Diagnostics;

namespace RunAsx86.Test
{
    [TestClass]
    public class IntegrationTests
    {
        private static Tuple<int, string> Execute(string arguments)
        {
            var workingDirectory = Environment.CurrentDirectory;
            using var process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = Path.Combine(workingDirectory, "RunAsx86.exe");
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            Assert.IsTrue(File.Exists(process.StartInfo.FileName));
            var output = "";
            while (!process.HasExited)
            {
                Assert.AreEqual("", process.StandardError.ReadToEnd());
                var line = process.StandardOutput.ReadToEnd().TrimEnd('\r', '\n');
                if (line.Length > 0)
                {
                    output += line;
                }
            }
            return new Tuple<int, string>(process.ExitCode, output);
        }

        [TestMethod]
        [Ignore]
        public void ExecuteItself()
        {
            var (exitCode, output) = Execute("RunAsx86");
            Assert.AreEqual("Please, go to application directory, provide path and arguments.", output);
            Assert.AreEqual(-1, exitCode);
        }
    }
}