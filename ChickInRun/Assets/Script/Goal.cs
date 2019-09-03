using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private int goalNum;
    public GameObject gate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chick")
        {
            if(goalNum==12)
            {
                StartCoroutine("WaitGoal");
                SceneManager.LoadScene("Ending");
            }
        }
    }
    private IEnumerator WaitGoal()
    {
        yield return new WaitForSeconds(1.0f);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goalNum = gate.gameObject.GetComponent<GateOpen>().openNum;
    }
}
