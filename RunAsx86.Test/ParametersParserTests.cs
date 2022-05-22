namespace RunAsx86.Test
{
    [TestClass]
    public class ParametersParserTests
    {
        [TestMethod]
        public void ExecuteNoArgs()
        {
            var detail = ParametersParser.Parse(Array.Empty<string>());
            Assert.AreEqual("Please, go to application directory, provide path and arguments.", detail.Error);
            Assert.AreEqual("", detail.Folder);
            Assert.AreEqual("", detail.AssemblyPath);
        }

        [TestMethod]
        public void ExecuteWithExecutable()
        {
            const string executable = "cmd";
            var detail = ParametersParser.Parse(new[] { executable });
            Assert.AreEqual("Can't find executable.", detail.Error);
            Assert.AreEqual("", detail.Folder);
            Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, executable + ".exe"), detail.AssemblyPath);
        }

        [TestMethod]
        public void ExecuteWithExecutableAndFolder()
        {
            const string executable = "cmd";
            const string folder = "c:/test/folder";
            var detail = ParametersParser.Parse(new[] { Path.Combine(folder, executable) });
            Assert.AreEqual("Can't find executable.", detail.Error);
            Assert.AreEqual(Path.GetFullPath(folder), Path.GetFullPath(detail.Folder));
            Assert.AreEqual(Path.Combine(folder, executable + ".exe"), detail.AssemblyPath);
        }

        [TestMethod]
        public void ExecuteNoError()
        {
            const string executable = "RunAsx86";
            var detail = ParametersParser.Parse(new[] { Path.Combine(Environment.CurrentDirectory, executable) });
            Assert.AreEqual("", detail.Error);
            Assert.AreEqual(Environment.CurrentDirectory, Path.GetFullPath(detail.Folder));
            Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, executable + ".exe"), detail.AssemblyPath);
        }
    }
}
