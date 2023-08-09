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
        inside.transform.position = outside.transform.position;
        inside.transform.rotation = outside.transform.rotation;
        inside.SetActive(true);
    }

    public void GoOutside()
    {
        outside.SetActive(true);
        outside.transform.position = inside.transform.position;
        outside.transform.rotation = inside.transform.rotation;
        inside.SetActive(false);
    }

    public bool IsInside()
    {
        return inside.activeSelf;
    }

    public bool IsOutside()
    {
        return outside.activeSelf;
    }

    public Vector3 GetInsidePosition()
    {
        return inside.transform.position;
    }

    public Vector3 GetOutsidePosition()
    {
        Debug.Log(outside.transform.position);
        return outside.transform.position;
    }
}
