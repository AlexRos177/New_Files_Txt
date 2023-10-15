using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

class Program
{
    static async Task Main(string[] args)
    {
        string directoryPath = @"C:\tools\ConsAppMyNewFileTxt";
        string[] files = Directory.GetFiles(directoryPath, "*.txt");


        for (int i = 1; i <= 10; i++)
        {
            string filename = Path.Combine(directoryPath, $"file{i}.txt");
            string content = $"Tas ir file Nr. {i}. Hello, visiem tas ir jauns txt files!";
            File.WriteAllText(filename, content);
        }

        Dictionary<string, string> fileContents = new Dictionary<string, string>();
        List<Task> tasks = new List<Task>();

        foreach (var file in files)
        {
            tasks.Add(Task.Run(() =>
            {
                string content = File.ReadAllText(file);
                string filename = Path.GetFileName(file);
                lock (fileContents)
                {

                    fileContents.Add(filename, content);
                }
            }));
        }

        await Task.WhenAll(tasks);


        foreach (var kvp in fileContents)
        {
            Console.WriteLine($"File: {kvp.Key}, Content: {kvp.Value}");
        }
    }
}

