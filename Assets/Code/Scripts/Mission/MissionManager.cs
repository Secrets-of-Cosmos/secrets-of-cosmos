using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    public List<Mission> availableMissions = new List<Mission>();
    public List<Mission> activeMissions = new List<Mission>();
    public List<Mission> completedMissions = new List<Mission>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AcceptMission(string missionName)
    {
        int index = availableMissions.FindIndex(mission => mission.GetMissionName().Equals(missionName));
        if (index != -1)
        {
            activeMissions.Add(availableMissions[index]);
            availableMissions.RemoveAt(index);
        }
    }

    public void CompleteMission(string missionName)
    {
        int index = activeMissions.FindIndex(mission => mission.GetMissionName().Equals(missionName));

        if (index != -1)
        {
            completedMissions.Add(activeMissions[index]);
            activeMissions.RemoveAt(index);
        }
    }
}
