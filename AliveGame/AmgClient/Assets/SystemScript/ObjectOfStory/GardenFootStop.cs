using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class GardenFootStop : MonoBehaviour
    {
        private float timer;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("부딪부딪");
                other.GetComponent<AudioSource>().Stop();
                // other.SendMessage("PlayerDaed");
            }
        }
    }
}
