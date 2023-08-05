using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    protected enum MissionType
    {
        Kill,
        Collect,
        Escort,
        Explore,
        Talk,
        Craft
    }

    [SerializeField]
    protected MissionManager missionManager;

    [SerializeField]
    protected MissionType missionType;
    [SerializeField]
    string missionName;
    [SerializeField]
    string missionDescription;
    [SerializeField]
    bool isComplete = false;
    [SerializeField]
    public string attributeToCheck;

    public PapermanAC player;

    public void CompleteMission()
    {
        missionManager.CompleteMission(missionName);
    }

    public void AddMission()
    {
        missionManager.AcceptMission(missionName);
    }

    public string GetMissionName()
    {
        return missionName;
    }


}
