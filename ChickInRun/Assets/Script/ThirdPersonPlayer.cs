using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    public Transform myCam;

    public float walkSpeed = 3.0f;
    public float gravity = 20.0f;
    public float jumpSpeed = 5.0f;

    private Vector3 myVelocity;

    internal Vector3 velocityToChild;
    internal bool jumpCheck = false;
    internal bool walkCheck = false;
    internal bool idleCheck = false;
    internal Transform myTransform;

    public static float JUMP_HEIGHT_MAX = 0.3f;
    public float stepTimer = 0.0f;
    private float timer = 0.0f;

    public bool jumping = false;

    public Vector3 moveDirection = Vector3.zero;
    public Vector3 jumpDirection = Vector3.zero;

    private CharacterController controller;
    private float inputX;
    private float inputZ;

    private Vector3 moveVectorX;
    private Vector3 moveVectorZ;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
        GetComponent<Animation>()["Run"].speed = 2.0f;

        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Vertical");
        inputZ = Input.GetAxis("Horizontal");
        moveVectorX = myCam.forward.normalized * inputX;
        moveVectorZ = myCam.right.normalized * inputZ;
        moveVector = (moveVectorX + moveVectorZ).normalized;


        ///////2go//////

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpCheck = true;
            walkCheck = false;
            idleCheck = false;

            this.stepTimer = 0.5f;

            if(controller.isGrounded)
            {
                jumping = true;
            }
            else
            {
                jumping = false;
            }
            if(jumping)
            {
                GetComponent<Animation>().Play("Jump");
                jumpDirection = moveDirection;

                this.transform.Translate((moveDirection.normalized*jumpSpeed) * Time.deltaTime);
                transform.position += transform.up * jumpSpeed * Time.deltaTime;
                myVelocity.y += Mathf.Sqrt(0.2f * 9.8f * JUMP_HEIGHT_MAX);

                myVelocity = new Vector3(moveVector.x, 0.0f, moveVector.z).normalized;
                myVelocity *= walkSpeed;
            }
        }
        if (this.stepTimer > 0.0f && jumping)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                stepTimer = 0.0f;
                jumping = false;
            }
            else
            {
                stepTimer -= Time.deltaTime;

                transform.position += transform.up * jumpSpeed * Time.deltaTime;
                myVelocity = new Vector3(moveVector.x, 0.0f, moveVector.z).normalized;
                myVelocity *= walkSpeed;
            }
            if (timer <= 0)
            {
                myVelocity.y = 0.0f;
            }
        }
        ////////////////

        if (controller.isGrounded)
        {
            myVelocity = new Vector3(moveVector.x, 0.0f, moveVector.z).normalized;
            myVelocity *= walkSpeed;

            if(myVelocity.magnitude > 0.5f)
            {
                GetComponent<Animation>().CrossFade("Run", 0.1f);
                transform.LookAt(myTransform.position + myVelocity);
                walkCheck = true;
                jumpCheck = false;
                idleCheck = false;
            }
            else
            {
                GetComponent<Animation>().CrossFade("Idle", 0.1f);
                walkCheck = false;
                jumpCheck = false;
                idleCheck = true;
            }
        }
        myVelocity.y -= gravity * Time.deltaTime;
        controller.Move(myVelocity * Time.deltaTime);
        velocityToChild = myVelocity;
       
    }
}
