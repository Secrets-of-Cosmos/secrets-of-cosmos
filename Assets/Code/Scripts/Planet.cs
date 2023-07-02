using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float orbitSpeed = 1.0f;
    public Transform orbitingObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (orbitingObject != null)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            transform.RotateAround(orbitingObject.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}
