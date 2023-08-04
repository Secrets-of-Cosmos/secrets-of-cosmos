using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceShipDockUIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform spaceShipTransform;
    [SerializeField] TextMeshProUGUI roll;
    [SerializeField] TextMeshProUGUI pitch;
    [SerializeField] TextMeshProUGUI yaw;
    [SerializeField] TextMeshProUGUI[] xyz;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pitchf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).x;
        var yawf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).y;
        var rollf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).z;

        pitch.text = pitchf.ToString("F2") + "°";
        yaw.text = yawf.ToString("F2") + "°";
        roll.text = rollf.ToString("F2") + "°";

        //xyz[0].text = spaceShipTransform.position.x.ToString("F2") + " m";
        //xyz[1].text = spaceShipTransform.position.y.ToString("F2") + " m";
        //xyz[2].text = spaceShipTransform.position.z.ToString("F2") + " m";
    }
}
