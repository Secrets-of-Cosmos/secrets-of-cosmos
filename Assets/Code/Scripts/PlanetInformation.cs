using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetInformation
{
    private Dictionary<string, object> attributes;
    private Dictionary<string, bool> discovered;
    private string name;

    public PlanetInformation(string name, Dictionary<string, object> initialAttributes)
    {
        this.name = name;
        attributes = new Dictionary<string, object>(initialAttributes);
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
            return attributes[attribute]?.ToString() ?? "Unknown";
        }
        else
        {
            return "****";
        }
    }
}