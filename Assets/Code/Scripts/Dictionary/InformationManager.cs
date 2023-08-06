using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField]
    public PlanetInformation mars;
    private MenuDescriptionController menuDescriptionController;
    
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

        HologramMenuController.Instance.tabSelectedEvent.AddListener(MenuTabChanged);
        menuDescriptionController = MenuDescriptionController.Instance;
        menuDescriptionController.buttonClickedEvent.AddListener(CardButtonClicked);
        SetTexts(mars);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CardButtonClicked(ClickedButtonInfo buttonInfo) {
        Debug.Log("Button clicked: " + buttonInfo.buttonName);
        // You can also get current selected tab with:
        // HologramMenuController.Instance.CurrentTab
    }

    private void MenuTabChanged(TabType tabType) {
        Debug.Log("New tab type: " + tabType);
    }

    public void SetTexts(PlanetInformation mars)
    {
        menuDescriptionController.MiddlePartTexts[0].Header = "Name";
        menuDescriptionController.MiddlePartTexts[0].Text = mars.Get("name");

        menuDescriptionController.MiddlePartTexts[1].Header = "Elements";
        menuDescriptionController.MiddlePartTexts[1].Text = mars.Get("elements");

        menuDescriptionController.LeftPartTexts[0].Header = "First Spacecraft";
        menuDescriptionController.LeftPartTexts[0].Text = mars.Get("firstSpacecraft");

        menuDescriptionController.LeftPartTexts[1].Header = "Surface Spacecraft";
        menuDescriptionController.LeftPartTexts[1].Text = mars.Get("surfaceSpacecraft");

        menuDescriptionController.LeftPartTexts[2].Header = "Fun Facts";
        menuDescriptionController.LeftPartTexts[2].Text = mars.Get("funFacts");

        menuDescriptionController.RightPartTexts[0].Header = "Atmosphere Condition";
        menuDescriptionController.RightPartTexts[0].Text = mars.Get("atmosphereCondition");

        menuDescriptionController.RightPartTexts[1].Header = "Proximity to Sun";
        menuDescriptionController.RightPartTexts[1].Text = mars.Get("proximityToSun");

        menuDescriptionController.RightPartTexts[2].Header = "Surface Temperature";
        menuDescriptionController.RightPartTexts[2].Text = mars.Get("surfaceTemperature");

        menuDescriptionController.UpdateTexts();
    }
}
