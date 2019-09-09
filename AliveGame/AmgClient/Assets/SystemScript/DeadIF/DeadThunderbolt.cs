using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class DeadThunderbolt : MonoBehaviour
    {
        private GameObject initObj;

        public GameObject thunderParticle;
        public GameObject lightningEffect;

        // Use this for initialization
        void Start()
        {
            initObj = GameObject.FindGameObjectWithTag("Initiate");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                 GameObject.Instantiate(thunderParticle,
                    new Vector3(other.gameObject.transform.position.x,
                                other.gameObject.transform.position.y + 10.0f,
                                other.gameObject.transform.position.z)
                    , Quaternion.identity);
                GameObject.Instantiate(lightningEffect);
                other.SendMessage("PlayerDead");
                initObj.SendMessage("CreateDeadGUI");
            }
        }
    }
}