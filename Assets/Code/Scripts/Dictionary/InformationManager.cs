using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField]
    public PlanetInformation mars;
    public MissionManager missionManager;
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

        AddListenerUpdate();
        SetTexts(mars);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddListenerUpdate()
    {
        HologramMenuController.instance.tabSelectedEvent.AddListener(MenuTabChanged);
        menuDescriptionController = MenuDescriptionController.Instance;
        menuDescriptionController.buttonClickedEvent.AddListener(CardButtonClicked);
    }

    private void CardButtonClicked(ClickedButtonInfo buttonInfo)
    {
        missionManager.AcceptMission(buttonInfo.cardHeader);

        menuDescriptionController.MiddlePartTexts = missionManager.GetMissionDescriptions();

        menuDescriptionController.Show(true, false);
    }

    private void MenuTabChanged(TabType tabType)
    {
        Debug.Log("New tab type: " + tabType);

        if (tabType == TabType.PLANETS)
        {
            SetTexts(mars);
            menuDescriptionController.Show(true, true);
        }
        else if (tabType == TabType.SPACECRAFTS)
        {
            menuDescriptionController.MiddlePartTexts = new MenuDescription[2] {
                new MenuDescription() {
                    Header = "Spacecraft1",
                    Text = "Info about spacecraft1"
                },
                new MenuDescription() {
                    Header = "Spacecraft2",
                    Text = "Info about spacecraft2"
                }
            };
            menuDescriptionController.Show(true, false);
        }
        else if (tabType == TabType.MISSIONS)
        {
            menuDescriptionController.MiddlePartTexts = missionManager.GetMissionDescriptions();

            menuDescriptionController.Show(true, false);
        }
    }

    public void SetTexts(PlanetInformation mars)
    {
        menuDescriptionController.MiddlePartTexts = new MenuDescription[2] {
            new MenuDescription() {
                Header = "Name",
                Text = mars.Get("name")
            },
            new MenuDescription() {
                Header = "Elements",
                Text = mars.Get("elements")
            }
        };

        menuDescriptionController.LeftPartTexts = new MenuDescription[3] {
            new MenuDescription() {
                Header = "First Spacecraft",
                Text = mars.Get("firstSpacecraft")
            },
            new MenuDescription() {
                Header = "Surface Spacecraft",
                Text = mars.Get("surfaceSpacecraft")
            },
            new MenuDescription() {
                Header = "Fun Facts",
                Text = mars.Get("funFacts")
            }
        };

        menuDescriptionController.RightPartTexts = new MenuDescription[3] {
            new MenuDescription() {
                Header = "Atmosphere Condition",
                Text = mars.Get("atmosphereCondition")
            },
            new MenuDescription() {
                Header = "Proximity to Sun",
                Text = mars.Get("proximityToSun")
            },
            new MenuDescription() {
                Header = "Surface Temperature",
                Text = mars.Get("surfaceTemperature")
            }
        };
    }
}