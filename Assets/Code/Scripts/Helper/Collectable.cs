using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Collectable");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(Transform parent = null)
    {
        if (parent != null)
        {
            rb.isKinematic = true;
            transform.SetParent(parent);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            transform.SetParent(null);
        }
    }

    public void Drop()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
        StartCoroutine(SetLayerAfterDelay(0.2f, LayerMask.NameToLayer("Collectable")));
    }

    IEnumerator SetLayerAfterDelay(float delay, int layer)
    {
        yield return new WaitForSeconds(delay);
        gameObject.layer = layer;
    }
}

