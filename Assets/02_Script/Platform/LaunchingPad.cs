using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingPad : MonoBehaviour
{
    public GameObject[] launchingPad;
    public GameObject cannon;

    float launchTime = 0f;
    float cannonInterval;
    int firingPosition;
    int oldFiringPosition= -1;

    void Start()
    {
        RandomCannonInterval();
    }

    // Update is called once per frame
    void Update()
    {
        launchTime += Time.deltaTime;
        if(launchTime >= cannonInterval)
        {
            RandomFiringPotition();
            if (firingPosition == oldFiringPosition)
            {
                RandomFiringPotition();
            }
            else
            {
                Instantiate(cannon, launchingPad[firingPosition].transform.position, Quaternion.identity);
                launchTime = 0f;
                oldFiringPosition = firingPosition;
                RandomCannonInterval();
            }
        }
    }

    void RandomFiringPotition()
    {
        firingPosition = Random.Range(0, 5);
    }

    void RandomCannonInterval()
    {
        cannonInterval = Random.Range(1, 10);
    }
}
