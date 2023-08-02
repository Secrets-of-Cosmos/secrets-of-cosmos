using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField]
    PlanetInformation mars;

    void Start()
    {
        Dictionary<string, object> marsAttributes = new Dictionary<string, object>
        {
            { "name", "Mars" },
            { "elements", new List<string> { "Iron", "Oxygen", "Carbon" } },
            { "firstSpacecraft", "Viking 1" },
            { "surfaceSpacecraft", new List<string> { "Rover 1", "Rover 2", "Rover 3" } },
            { "atmosphereCondition", "Thin Atmosphere" },
            { "proximityToSun", 227.9 },
            { "surfaceTemperature", -63 },
            { "lengthOfDay", 24.7 },
            { "lengthOfYear", 687 },
            { "funFacts", "Second thinnest atmosphere in the Solar System!" }
        };

        mars = new PlanetInformation("mars", marsAttributes);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
