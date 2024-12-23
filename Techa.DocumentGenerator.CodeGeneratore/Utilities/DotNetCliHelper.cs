using System.Diagnostics;

namespace Techa.DocumentGenerator.CodeGeneratore.Utilities
{
    public static class DotNetCliHelper
    {
        public static bool RunCliCommand(string command, string path)
        {
            string outputText = string.Empty;
            string standardError = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(path))
                    return false;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = command,
                        WorkingDirectory = path,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = false
                    };

                    process.Start();
                    
                    outputText = process.StandardOutput.ReadToEnd();
                    standardError = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return string.IsNullOrEmpty(standardError);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
