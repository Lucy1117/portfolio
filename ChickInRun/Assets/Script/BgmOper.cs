using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmOper : MonoBehaviour
{
    public bool deathCheck;

    private void OnTriggerStay(Collider other)
    {
        if(deathCheck)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!deathCheck)
        {
            if(other.gameObject.tag == "Chick")
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
