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
            { "elements", new List<string> {
                    "Silicon Dioxide (SiO₂): Comprises approximately 45% of Mars' soil.",
                    "Iron(III) Oxide (Fe₂O₃): Comprises approximately 18% of Mars' soil.",
                    "Aluminum Oxide (Al₂O₃): Comprises approximately 7% of Mars' soil."
                }
            },
            { "firstSpacecraft", "Viking 1" },
            { "surfaceSpacecraft", new List<string> { "Ingenuity", "Perseverance", "Curiosity" } },
            { "atmosphereCondition", new List<string> {
                    "95% carbon dioxide",
                    "3% nitrogen",
                    "1.6% argon",
                    "and it has traces of oxygen, carbon monoxide",
                }
            },
            { "proximityToSun", new List<string> {
                    "From an average distance of 228 million kilometers",
                    "It takes sunlight 13 minutes to travel from the Sun to Mars.",
                }
            },
            { "surfaceTemperature", new List<string> {
                    "The temperature on Mars can be as high as 20 degrees Celsius",
                    "or as low as about -153 degrees Celsius."
                }
            },
            { "lengthOfDay", 24.7 },
            { "lengthOfYear", 687 },
            { "funFacts", "Mars was named after the Roman god of war." }
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