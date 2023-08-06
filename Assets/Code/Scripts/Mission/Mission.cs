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
        Craft,
        Send
    }

    public enum MissionStatus
    {
        Available,
        Active,
        Completed
    }



    [SerializeField]
    protected MissionManager missionManager;

    [SerializeField]
    protected MissionType missionType;
    [SerializeField]
    public string missionName;
    [SerializeField]
    public string missionDescription;
    [SerializeField]
    public MissionStatus missionStatus;
    [SerializeField]
    public string attributeToCheck;

    // Distance to player to start dialogue
    [SerializeField]
    protected float nearDistance = 5.0f;

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


    protected bool IsPlayerNearby()
    {
        return Vector3.Distance(player.transform.position, transform.position) < nearDistance;
    }

    protected void FreezeRigidbody(bool isFrozen)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = isFrozen ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        }
    }


}
