using System;
using System.Xml.Serialization;

namespace AppAudit
{
    [Serializable()]
    public class Rule
    {
        //String to match
        [System.Xml.Serialization.XmlElement("Regex")]
        public string Regex { get; set; }

        //Human-readable description
        [System.Xml.Serialization.XmlAttribute("Description")]
        public string Description { get; set; }

        //Low medium high priority
        //Suggested fix or implementation?
        //A link to the fix
    }

    [Serializable()]
    public class AuditFile
    {
        //File name
        [System.Xml.Serialization.XmlAttribute("Name")]
        public string Name { get; set; }

        //List of rules for file
        [XmlArray("Rules")]
        [XmlArrayItem("Rule", typeof(Rule))]
        public Rule[] Rules { get; set; }
    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("FileCollection")]
    public class FileCollection
    {
        //List of files to analyze
        [XmlArray("Files")]
        [XmlArrayItem("AuditFile", typeof(AuditFile))]
        public AuditFile[] Files { get; set; }
    }
}
