using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGate : MonoBehaviour
{
    private GameObject lGate;
    public GameObject lavaParticle;

    // Start is called before the first frame update
    void Start()
    {
        lGate = GameObject.FindGameObjectWithTag("LavaGate");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(2.0f, new Vector3(0.0f, 0.0f, 1.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            lGate.GetComponent<GateOpen>().openNum++;
            Instantiate(lavaParticle, transform.position, transform.rotation);
            Destroy(gameObject);
            lGate.GetComponent<GateOpen>().lsoundCheck = true;
        }
    }
}
