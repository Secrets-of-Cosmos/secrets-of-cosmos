using UnityEngine;
using Cinemachine;

public class VcamClamping : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        vcam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = Mathf.Clamp(
            vcam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value,
            -90f,
            90f);        
    }
}
