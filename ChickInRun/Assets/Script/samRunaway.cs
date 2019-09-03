using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class samRunaway : MonoBehaviour
{
    private GameObject wayPoint;
    private Vector3 wayPointPos;
    private Vector3 runawayPos;
    private float timer = 1.0f;

    private float speed = 3.0f;
    private float maxDistance = 2.0f;
    private float minDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        wayPoint = GameObject.Find("wayPoint");
    }

    // Update is called once per frame
    void Update()
    {
        wayPointPos = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y, wayPoint.transform.position.z);
        runawayPos = transform.position - wayPoint.transform.position;

        if (Vector3.Distance(transform.position, wayPoint.transform.position) < maxDistance)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }

            transform.position = Vector3.MoveTowards(transform.position, runawayPos * minDistance, speed * Time.deltaTime);
            GetComponent<Animation>().Play("mannyonsam_Walk");
            transform.LookAt(-wayPointPos);
        }
        if(timer <=0)
        {
            StartCoroutine("StopSam");
            timer = 1.0f;
        }
    }

    private IEnumerator StopSam()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
