using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathElectric : MonoBehaviour
{
    public AudioClip elecSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            other.gameObject.GetComponent<PlayerInst>().electricVar++;
            other.gameObject.SendMessage("ElecDeath");
            GetComponent<AudioSource>().PlayOneShot(elecSound);
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
