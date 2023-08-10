using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlanetInformation
{
    [SerializeField]
    private Dictionary<string, object> attributes;
    [SerializeField]
    private Dictionary<string, bool> discovered;
    [SerializeField]
    private string name;
    [SerializeField]
    private string[] attributeNames;

    public PlanetInformation(string name, Dictionary<string, object> initialAttributes)
    {
        this.name = name;
        attributes = new Dictionary<string, object>(initialAttributes);
        attributeNames = initialAttributes.Keys.ToArray();
        discovered = new Dictionary<string, bool>();
        foreach (var key in initialAttributes.Keys)
        {
            discovered[key] = false;
        }
    }

    public void Discover(string attribute)
    {
        if (discovered.ContainsKey(attribute))
        {
            discovered[attribute] = true;
        }
    }

    public string Get(string attribute)
    {
        if (discovered.ContainsKey(attribute) && discovered[attribute])
        {
            if (attributes[attribute] is List<string>)
            {
                string result = "";
                foreach (var item in (List<string>)attributes[attribute])
                {
                    result += item + "\n";
                }

                return result;
            }
            else if (attributes[attribute] is double)
            {
                return attributes[attribute].ToString();
            }
            else
            {
                return attributes[attribute].ToString();
            }
        }
        else
        {
            if (attributes[attribute] is List<string>)
            {
                string result = "";
                foreach (var item in (List<string>)attributes[attribute])
                {
                    result += "****\n";
                }

                return result;
            }
            else if (attributes[attribute] is double)
            {
                return "****";
            }
            else
            {
                return "****";
            }
        }
    }
}