using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    public List<Mission> missions = new List<Mission>();
    public InformationManager informationManager;
    // Start is called before the first frame update
    void Start()
    {
        MenuDescription[] missionDescriptions = GetMissionDescriptions();
        Debug.Log("Mission descriptions:");
        Debug.Log(missionDescriptions.Length);
        foreach (MenuDescription missionDescription in missionDescriptions)
        {
            Debug.Log(missionDescription.Header);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AcceptMission(string missionName)
    {
        int index = missions.FindIndex(mission => mission.GetMissionName().Equals(missionName));
        if (index != -1)
        {
            if (missions[index].missionStatus == Mission.MissionStatus.Available)
            {
                missions.ForEach(mission => mission.missionStatus = mission.missionStatus == Mission.MissionStatus.Active ? Mission.MissionStatus.Available : mission.missionStatus);
                missions[index].missionStatus = Mission.MissionStatus.Active;
            }
            else if (missions[index].missionStatus == Mission.MissionStatus.Active)
            {
                missions[index].missionStatus = Mission.MissionStatus.Available;
            }
        }
    }

    public void CompleteMission(string missionName)
    {
        int index = missions.FindIndex(mission => mission.GetMissionName().Equals(missionName));

        if (index != -1)
        {
            Mission mission = missions[index];
            informationManager.mars.Discover(mission.attributeToCheck);
            mission.missionStatus = Mission.MissionStatus.Completed;
            informationManager.SetTexts(informationManager.mars);
        }
    }

    public MenuDescription[] GetMissionDescriptions()
    {
        List<MenuDescription> missionDescriptions = new List<MenuDescription>();
        foreach (Mission mission in missions)
        {

            missionDescriptions.Add(new MenuDescription()
            {
                Header = mission.missionName,
                Text = mission.missionDescription,
                showButton = true,
                ButtonText = mission.missionStatus.ToString(),
            });

        }
        return missionDescriptions.ToArray();
    }
}
