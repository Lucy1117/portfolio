using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JM.MyProject.MyGame
{
    public class FirstPersonCamera : MonoBehaviour
    {

        public Transform cameraTransform;
        public float height = 0.0f;
        public float distance = 0.0f;

        // Use this for initialization
        void Start()
        {
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            cameraTransform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z + distance);
            cameraTransform.rotation = transform.rotation;

        }

        // Update is called once per frame
        void Update()
        {
            cameraTransform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z + distance);
            cameraTransform.rotation = transform.rotation;

        }
    }
}