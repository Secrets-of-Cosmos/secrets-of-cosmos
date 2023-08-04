using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    public Transform spaceCraft;
    public float forceMultiplier;
    public SpaceShipController ssc;
    public bool isCrashed;
    // Start is called before the first frame update
    void Start()
    {
        isCrashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        isCrashed = true;
        ssc.ShakeShip();
        gameObject.SetActive(false);
    }

    public IEnumerator MoveAsteroid(Vector3 startPosition, Transform targetTransform, float time)
    {

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, targetTransform.position, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetTransform.position;
    }

}
