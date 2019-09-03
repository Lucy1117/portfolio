using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstFollow : MonoBehaviour
{
    private float jumpSpeed = 8.0f;

    private GameObject parentVar;
    private bool parentJump;
    private bool parentWalk;
    private bool parentIdle;
    private Vector3 velocity;
    //private Transform mytransform;

    // Start is called before the first frame update
    void Start()
    {
        //mytransform = this.transform;
        parentVar = GameObject.FindWithTag("Chick");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.parent = parentVar.transform;
        velocity = this.transform.parent.gameObject.GetComponent<ThirdPersonPlayer>().velocityToChild;
        parentJump = this.transform.parent.gameObject.GetComponent<ThirdPersonPlayer>().jumpCheck;
        parentWalk = this.transform.parent.gameObject.GetComponent<ThirdPersonPlayer>().walkCheck;
        parentIdle = this.transform.parent.gameObject.GetComponent<ThirdPersonPlayer>().idleCheck;

        if(parentJump)
        {
            velocity.y = jumpSpeed;
            GetComponent<Animation>().Play("Jump");
        }
        else if(parentWalk)
        {
            GetComponent<Animation>().CrossFade("Run", 0.1f);
        }
        else if(parentIdle)
        {
            GetComponent<Animation>().CrossFade("Idle", 0.1f);
        }
    }
}
