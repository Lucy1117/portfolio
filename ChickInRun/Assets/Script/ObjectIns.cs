using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIns : MonoBehaviour
{

    public GameObject weaselParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Chick")
        {
            collision.gameObject.SendMessage("Split");
            Instantiate(weaselParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
