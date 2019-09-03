using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    public int openNum;
    public bool lsoundCheck;
    public AudioClip lSound;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animation>().Play("Stop");
        openNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lsoundCheck)
        {
            GetComponent<AudioSource>().PlayOneShot(lSound);
            lsoundCheck = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Chick")
        {
            if(openNum==12)
            {
                GetComponent<Animation>().Play("Move");
            }
        }
    }
}
