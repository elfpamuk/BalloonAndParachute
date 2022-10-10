using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    [SerializeField] private GameObject balloonPrefab;
    private GameObject balloonObject;
    private float timer;
    
    void Start()
    {
        SpawnBalloon();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 3f)
        {
            SpawnBalloon();
            timer = 0;
        }

        timer += Time.deltaTime;

    }

    private void SpawnBalloon()
    {
        balloonObject = Instantiate(balloonPrefab, transform.position 
                                                   + new Vector3(Random.Range(0f,0.2f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f)), Quaternion.identity);
        balloonObject.transform.parent = transform;
    }
}
