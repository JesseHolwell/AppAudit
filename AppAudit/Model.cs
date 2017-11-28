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

        //File to search
        [System.Xml.Serialization.XmlElement("Location")]
        public string Location { get; set; }

        //Human-readable description
        [System.Xml.Serialization.XmlElement("Description")]
        public string Description { get; set; }

        //Low medium high
        //Priority

        //Human-readable description
        //Description

        //Suggested fix or implementation?

    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("RuleCollection")]
    public class RuleCollection
    {
        [XmlArray("Rules")]
        [XmlArrayItem("Rule", typeof(Rule))]
        public Rule[] Rules { get; set; }
    }
}
