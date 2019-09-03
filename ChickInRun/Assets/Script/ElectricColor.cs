using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricColor : MonoBehaviour
{
    public Material darkMatA;
    public Material darkMatB;
    public Material originMat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /// <summary>
    /// 2019
    /// </summary>
    /// <param name="num"></param>
    void ElecChange(int num)
    {
        if (num == 1)
        {
            GetComponent<Renderer>().material = darkMatA;
        }
        else if (num == 2)
        {
            GetComponent<Renderer>().material = darkMatB;
        }
        else if (num == 0)
        {
            GetComponent<Renderer>().material = originMat;
        }
    }
}
