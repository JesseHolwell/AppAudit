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
        public static RuleCollection AppRules {get;set;}

        static void Main(string[] args)
        {
            //parse the xml file - async?
            AppRules = GetRulesFromXml();
            
            //ask for the project directory - async?
            Console.WriteLine("Enter app location > ");
            var appLocation = Console.ReadLine();

            //search this location for any relevant files
            //identified:
            //web.configs
            var debugconfig = "web.config";
            var stagingconfig = "web.staging.config";
            var releaseconfig = "web.release.config";

            //app_start
            //...

            foreach (var path in Directory.GetFiles(appLocation))
            {
                //TODO: better file identification logic?
                //Console.WriteLine(path); // full path
                //Console.WriteLine(System.IO.Path.GetFileName(path)); // file name
                if (path.ToLower().Contains(debugconfig)) 
                    DebugAnalysis(path, debugconfig);
            }

            //TODO: graceful loop or exit
            Console.ReadLine();

        }

        public static void DebugAnalysis(string path, string file)
        {
            var lines = File.ReadAllLines(path);

            foreach (var rule in AppRules.Rules.Where(x=>x.Location == file))
            {
                var matched = false;

                foreach (var line in lines)
                    if (Regex.IsMatch(line, rule.Regex))
                        matched = true;

                Console.WriteLine(rule.Description + ":\t" + matched);
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
