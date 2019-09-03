using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform TargetLookAt;

    public float mydist = 7.0f;
    public float distanceMin = 1.0f;
    public float distanceMax = 13.0f;

    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private float startingDistance = 0.0f;
    private float desiredDistance = 0.0f;

    public float xMouseSensitivity = 5.0f;
    public float yMouseSensitivity = 5.0f;
    public float mouseWheelSensitivity = 5.0f;
    public float yMinLimit = -40.0f;
    public float yMaxLimit = 80.0f;

    public float distanceSmooth = 0.05f;
    private float velocityDistance = 0.0f;
    private Vector3 desiredPosition = Vector3.zero;

    public float xSmooth = 0.05f;
    public float ySmooth = 0.1f;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;
    private float velocityZ = 0.0f;
    private Vector3 mypos = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        mydist = Mathf.Clamp(mydist, distanceMin, distanceMax);
        startingDistance = mydist;
        Reset();
    }

    private void LateUpdate()
    {
        if(TargetLookAt == null)
        {
            return;
        }

        HandPlayerInput();
        CalculateDesiredPosition();
        UpdatePosition();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandPlayerInput()
    {
        float deadZone = 0.01f;

        mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * yMouseSensitivity;

        mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);

        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(mydist - (Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity), distanceMin, distanceMax);
        }

    }

    private void CalculateDesiredPosition()
    {
        mydist = Mathf.SmoothDamp(mydist, desiredDistance, ref velocityDistance, distanceSmooth);
        desiredPosition = CalculatePosition(mouseY, mouseX, mydist);
    }

    private Vector3 CalculatePosition(float rotationX, float rotationY, float dist)
    {
        Vector3 mydirection = new Vector3(0.0f, 0.0f, -mydist);
        Quaternion myrotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        return TargetLookAt.position + (myrotation * mydirection);
    }

    private void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(mypos.x, desiredPosition.x, ref velocityX, xSmooth);
        float posY = Mathf.SmoothDamp(mypos.y, desiredPosition.y, ref velocityY, ySmooth);
        float posZ = Mathf.SmoothDamp(mypos.z, desiredPosition.z, ref velocityZ, xSmooth);
        mypos = new Vector3(posX, posY, posZ);
        transform.position = mypos;
        transform.LookAt(TargetLookAt);
    }

    private void Reset()
    {
        mouseX = -90.0f;
        mouseY = 40.0f;
        mydist = startingDistance;
        desiredDistance = mydist;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        while(angle < -360 || angle > 360)
        {
            if(angle < -360)
            {
                angle += 360;
            }
            if(angle > 360)
            {
                angle -= 360;
            }
        }

        return Mathf.Clamp(angle, min, max);
    }
}
