using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endinglava : MonoBehaviour
{
    public Material openMat;
    public Material originMat;
    private int level = 0;
    public int order;
    public int orderA;
    public int orderB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        level++;
        if(level == orderA)
        {
            GetComponent<Renderer>().material = openMat;
        }
        else if(level == orderB)
        {
            GetComponent<Renderer>().material = originMat;
            level = order;
        }
    }
}
