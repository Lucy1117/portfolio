using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mannyonsamBlueSmall : MonoBehaviour
{
    public GameObject manParticle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            other.gameObject.SendMessage("Small");
            Instantiate(manParticle, transform.position, transform.rotation);
            Destroy(gameObject);
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
