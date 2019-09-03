using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUnion : MonoBehaviour
{
    public GameObject manParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Chick")
        {
            collision.gameObject.SendMessage("Union");
            collision.gameObject.SendMessage("Big");
            collision.gameObject.SendMessage("Origin");
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
