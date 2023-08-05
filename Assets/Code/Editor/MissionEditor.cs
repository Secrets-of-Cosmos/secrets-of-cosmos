using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Mission))]
public class MissionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Mission nesnesini al
        Mission mission = (Mission)target;

        // Özellikleri düzenleyebilmek için başlangıç kodunu çağır
        base.OnInspectorGUI();

        // "Add Mission" butonu
        if (GUILayout.Button("Add Mission"))
        {
            mission.AddMission();
            Debug.Log(mission.GetMissionName() + " added!");
        }

        // "Complete Mission" butonu
        if (GUILayout.Button("Complete Mission"))
        {
            mission.CompleteMission();
            Debug.Log(mission.GetMissionName() + " completed!");
        }
    }
}
