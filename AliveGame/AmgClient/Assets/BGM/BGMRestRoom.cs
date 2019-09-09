using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class BGMRestRoom : MonoBehaviour
    {
        public float soundVolume;

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
                AudioSource audiosource = GetComponent<AudioSource>();
                audiosource.volume = soundVolume;
                GameObject.Find("BackGroundSound").GetComponent<AudioSource>().volume = GameObject.Find("BackGroundSound").GetComponent<AudioSource>().volume/2;
                audiosource.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                AudioSource audiosource = GetComponent<AudioSource>();
                audiosource.Stop();
                GameObject.Find("BackGroundSound").GetComponent<AudioSource>().volume = GameObject.Find("BackGroundSound").GetComponent<AudioSource>().volume * 2;
            }
        }
    }
}
