using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideOutsideController : MonoBehaviour
{
    public GameObject outside;
    public GameObject inside;

    public void GoInside()
    {
        outside.SetActive(false);
        inside.SetActive(true);
    }

    public void GoOutside()
    {
        outside.SetActive(true);
        inside.SetActive(false);
    }
}
