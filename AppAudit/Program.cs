using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AppAudit
{
    class Program
    {
        public static FileCollection AppRules { get; set; }

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============== Do you follow best practices? ==============");
            Console.ResetColor();

            //parse the xml file - async?
            AppRules = GetRulesFromXml();

            var proceed = true;
            while (proceed)
            {

                //ask for the project directory - async?
                Console.WriteLine("Enter app location > ");
                var appLocation = Console.ReadLine();

                //HACK: 
                appLocation = @"C:\Users\holweje\Source\Repos\Divestment\Divestment";

                var locatedFiles = Directory.GetFiles(appLocation);

                //TODO: better file identification logic?
                Console.WriteLine("=== ANALYZING PROJECT: "
                    + Path.GetDirectoryName(appLocation) + " ===");
                Console.WriteLine("Files found:");
                foreach (var path in locatedFiles)
                    Console.WriteLine("\t- " + Path.GetFileName(path));

                foreach (var path in locatedFiles)
                    foreach (var config in AppRules.Files)
                        if (path.ToLower().Contains(config.Name))
                            AnalyzeFile(path, config.Name);

                //TODO: more graceful loop or exit
                Console.WriteLine("\nExit? (X))");
                if (Console.ReadLine() == "X")
                    proceed = false;

            }
        }

        public static void AnalyzeFile(string path, string filename)
        {
            var lines = File.ReadAllLines(path);

            //var ruleScope = AppRules.Rules.Where(x => x.Location == file);
            foreach (var file in AppRules.Files)
            {
                if (file.Name == filename)
                {
                    var ruleScope = file.Rules;
                    if (ruleScope.Length > 0)
                    {
                        Console.WriteLine("\nAnalyzing: " + path);
                        foreach (var rule in ruleScope)
                        {
                            var matched = false;

                            foreach (var line in lines)
                                if (Regex.IsMatch(line, rule.Regex))
                                    matched = true;

                            Console.BackgroundColor = (matched) ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                            Console.ForegroundColor = (matched) ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.WriteLine(rule.Description + ":\t" + matched);
                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        public static FileCollection GetRulesFromXml()
        {
            string path = "BestPractices.xml";

            FileCollection rules = null;
            XmlSerializer serializer = new XmlSerializer(typeof(FileCollection));
            StreamReader reader = new StreamReader(path);
            rules = (FileCollection)serializer.Deserialize(reader);
            reader.Close();
            return rules;
        }
    }
}
