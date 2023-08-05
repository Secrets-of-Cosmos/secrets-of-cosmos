using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerRover : MonoBehaviour
{

    public Transform armVertical;
    public Transform armHorizontal;
    public SphereCollider armCollider;
    LayerMask collectableLayer;
    // Start is called before the first frame update
    void Start()
    {
        collectableLayer = LayerMask.NameToLayer("Collectable");

    }

    // Update is called once per frame
    void Update()
    {
        InputControl();
        CollisionCheck();
    }

    void CollisionCheck()
    {

        Collider[] hitColliders = Physics.OverlapSphere(armCollider.bounds.center, armCollider.radius, 1 << collectableLayer);
        if (hitColliders.Length > 0)
        {
            hitColliders[0].GetComponent<Collectable>().Collect(armCollider.transform);
        }
    }

    void InputControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            armVertical.Rotate(0, 1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            armVertical.Rotate(0, -1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            armHorizontal.Rotate(0, 1, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            armHorizontal.Rotate(0, -1, 0, Space.Self);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropItem();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(armCollider.bounds.center, armCollider.radius);
    }

    private void DropItem()
    {
        Collectable collectable = armCollider.GetComponentInChildren<Collectable>();
        if (collectable != null)
        {
            collectable.Drop();
        }
    }
}
