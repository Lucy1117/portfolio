using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollow : MonoBehaviour
{
    private GameObject wayPoint;
    private Vector3 wayPointPos;
    private Quaternion wayPointRot;

    private float speed = 2.5f;
    private float MaxDistance = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        wayPoint = GameObject.Find("wayPoint");
    }

    // Update is called once per frame
    void Update()
    {
        wayPointPos = new Vector3(wayPoint.transform.position.x, transform.position.y, wayPoint.transform.position.z);

        if(Vector3.Distance(transform.position, wayPoint.transform.position) < MaxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
            transform.LookAt(wayPointPos);
            GetComponent<Animation>().Play("weaselRun");
        }
    }
}
