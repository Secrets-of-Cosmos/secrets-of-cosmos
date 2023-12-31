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
    [SerializeField] TextMeshProUGUI distanceToLockedPlanet;
    [SerializeField] SpaceShipController ssc;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pitchf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).x;
        var yawf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).y;
        var rollf = UnityEditor.TransformUtils.GetInspectorRotation(spaceShipTransform).z;

        pitch.text = pitchf.ToString("F2") + "�";
        yaw.text = yawf.ToString("F2") + "�";
        roll.text = rollf.ToString("F2") + "�";

        Rigidbody spaceRb = spaceShipTransform.gameObject.GetComponent<Rigidbody>();
        var locVel = transform.InverseTransformDirection(spaceRb.velocity);

        xyz[0].text = locVel.x.ToString("F2") + " m/s";
        xyz[1].text = locVel.y.ToString("F2") + " m/s";
        xyz[2].text = locVel.z.ToString("F2") + " m/s";

        distanceToLockedPlanet.text = ssc.lockPlanet ? "Distance\n" + ssc.distanceToLockedPlanet.ToString("F2") + " m" : "";
    }
}
