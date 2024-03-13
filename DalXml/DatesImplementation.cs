using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

internal class DatesImplementation : IDates
{
    private readonly string _date_xml = "data-config";
    public DateTime? getStartProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(_date_xml).Element("StartProject")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);
    }

    public DateTime? setStartProject(DateTime? startProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_date_xml);
        root.Element("StartProject")!.Value = startProject.ToString();
        XMLTools.SaveListToXMLElement(root, _date_xml);
        return startProject;
    }

    public DateTime? getEndProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(_date_xml).Element("EndProject")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);
    }

    public DateTime? setEndProject(DateTime? endProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(_date_xml);
        root.Element("EndProject")!.Value = endProject.ToString();
        XMLTools.SaveListToXMLElement(root, _date_xml);
        return endProject;
    }
}
