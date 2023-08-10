using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public List<Mission> missions = new List<Mission>();
    public InformationManager informationManager;
    public int missionPlacementDistance = 50;
    public int[] distances;
    public Image missionMarker;
    public TextMeshProUGUI missionMarkerText;
    public PapermanAC player;
    public WorkerRover workerRover;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PapermanAC>();
        SetPlayerForMissions(player);
        SetManagerForMissions();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            informationManager.AddListenerUpdate();
            player = GameObject.FindObjectOfType<PapermanAC>();
            SetPlayerForMissions(player);
        }
        UpdateMissions();
        UpdateMissionArrow();

    }

    public void SetPlayerForMissions(PapermanAC player)
    {
        this.player = player;
        missions.ForEach(mission => mission.player = player);
    }

    public void SetManagerForMissions()
    {
        missions.ForEach(mission => mission.missionManager = this);
    }

    private void UpdateMissionArrow()
    {
        Mission activeMission = ActiveMission();
        if (activeMission != null)
        {
            missionMarker.gameObject.SetActive(true);
            missionMarkerText.gameObject.SetActive(true);
            Vector3 direction = activeMission.transform.position - player.transform.position;
            direction.y = 0;
            float angle = Vector3.SignedAngle(direction, player.transform.forward, Vector3.up);
            missionMarker.transform.rotation = Quaternion.Euler(0, 0, angle);
            missionMarkerText.text = DistanceToMission(activeMission).ToString() + "m";
        }
        else
        {
            missionMarker.gameObject.SetActive(false);
            missionMarkerText.gameObject.SetActive(false);
        }
    }

    private void UpdateMissions()
    {
        distances = XZDistancesToMissions();
        for (int i = 0; i < missions.Count; i++)
        {
            if (distances[i] < missionPlacementDistance && missions[i].gameObject.activeSelf == false)
            {
                missions[i].gameObject.SetActive(true);
                SetPositionByRaycast(missions[i]);
            }
            else if (distances[i] > missionPlacementDistance && missions[i].gameObject.activeSelf == true)
            {
                missions[i].gameObject.SetActive(false);
                missions[i].transform.position = missions[i].transform.position + Vector3.up * 100;
            }
        }
    }


    private void SetPositionByRaycast(Mission m)
    {
        RaycastHit hit;
        Ray ray = new Ray(m.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit))
        {
            m.transform.position = hit.point + new Vector3(0, 2f, 0);
            m.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private Mission ActiveMission()
    {
        return missions.Find(mission => mission.missionStatus == Mission.MissionStatus.Active);
    }

    int[] XZDistancesToMissions()
    {
        int[] distances = new int[missions.Count];
        for (int i = 0; i < missions.Count; i++)
        {
            distances[i] = DistanceToMission(missions[i]);
        }
        return distances;
    }


    int DistanceToMission(Mission m)
    {
        if (workerRover.controlMode == WorkerRover.ControlMode.Player)
        {
            float xDistance = Mathf.Abs(workerRover.transform.position.x - m.transform.position.x);
            float zDistance = Mathf.Abs(workerRover.transform.position.z - m.transform.position.z);
            return (int)Mathf.Sqrt(xDistance * xDistance + zDistance * zDistance);
        }
        else
        {
            float xDistance = Mathf.Abs(player.transform.position.x - m.transform.position.x);
            float zDistance = Mathf.Abs(player.transform.position.z - m.transform.position.z);
            return (int)Mathf.Sqrt(xDistance * xDistance + zDistance * zDistance);
        }
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
