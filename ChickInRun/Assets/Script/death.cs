using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    public AudioClip bloodSound;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            other.gameObject.SendMessage("Death");
            GetComponent<AudioSource>().PlayOneShot(bloodSound);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
