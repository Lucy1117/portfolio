using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGateLava : MonoBehaviour
{
    public Material openMat;
    public int openNumber;
    private int lavanum;
    private GameObject lGate;

    // Start is called before the first frame update
    void Start()
    {
        lGate = GameObject.FindGameObjectWithTag("LavaGate");
    }

    // Update is called once per frame
    void Update()
    {
        lavanum = lGate.GetComponent<GateOpen>().openNum;
        if(lavanum == openNumber)
        {
            GetComponent<Renderer>().material = openMat;
        }
    }
}
