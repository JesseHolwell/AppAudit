using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace AppAudit
{
    class Program
    {
        public static RuleCollection AppRules { get; set; }

        static void Main(string[] args)
        {
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

                //search this location for any relevant files
                //identified:
                //web.configs
                var files = new List<string>()
                {
                    "web.config",
                    "web.staging.config",
                    "web.release.config",

                    "global.asax",
                    "startup.cs",
                    @"Views\Shared\_Layout.cs"
                };

                var locatedFiles = Directory.GetFiles(appLocation);
                //app_start
                //...


                //TODO: better file identification logic?
                Console.WriteLine("Files found:");
                foreach (var path in locatedFiles)
                    Console.WriteLine(path);

                foreach (var path in locatedFiles)
                    foreach (var config in files)
                        if (path.ToLower().Contains(config))
                            AnalyzeFile(path, config);

                //TODO: more graceful loop or exit
                Console.WriteLine("Exit? (X))");
                if (Console.ReadLine() == "X")
                    proceed = false;

            }
        }

        public static void AnalyzeFile(string path, string file)
        {
            var lines = File.ReadAllLines(path);

            var ruleScope = AppRules.Rules.Where(x => x.Location == file);
            if (ruleScope.Any())
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

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public static RuleCollection GetRulesFromXml()
        {
            string path = "BestPractices.xml";

            RuleCollection rules = null;
            XmlSerializer serializer = new XmlSerializer(typeof(RuleCollection));
            StreamReader reader = new StreamReader(path);
            rules = (RuleCollection)serializer.Deserialize(reader);
            reader.Close();
            return rules;
        }
    }


}
