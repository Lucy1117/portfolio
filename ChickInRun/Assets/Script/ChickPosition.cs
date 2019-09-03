using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickPosition : MonoBehaviour
{
    public GameObject wayPoint;
    private float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            //The position of the waypoint will update to the player's position
            UpdatePosition();

            timer = 0.5f;
        }
    }

    void UpdatePosition()
    {
        wayPoint.transform.position = this.transform.position;
    }
}
