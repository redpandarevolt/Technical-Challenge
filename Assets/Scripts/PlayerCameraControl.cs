using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    [SerializeField] float m_CamVerticalSpeed = 2f;
    [SerializeField] float m_CamHorizontalSpeed = 3f;
    [SerializeField] float m_ZoomSpeed = 1f;
    [SerializeField] float m_CamYMax = 40f;
    [SerializeField] float m_CamYMin = 0f;
    [SerializeField] float m_NeckYMin = 0;
    [SerializeField] float m_NeckYMax = -40f;
    [SerializeField] float m_NeckXRange = 20f;
    [SerializeField] float m_CamZoomMin = 15f;
    [SerializeField] float m_CamZoonMax = 50f;

    public Camera mCam = null;
    public Transform mLookAtPos = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float RotateDeltaX = Input.GetAxis("Mouse X");
        float RotateDeltaY = Input.GetAxis("Mouse Y");
        float Zoom = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(1) == true)
        {

            //Handles camera up/down/left/right           
            float nCamAngleX = transform.localEulerAngles.x;
            nCamAngleX = (nCamAngleX > 180) ? nCamAngleX - 360 : nCamAngleX;
            nCamAngleX += -RotateDeltaY * m_CamVerticalSpeed;
            float nCamAngleY = transform.localEulerAngles.y;
            nCamAngleY = (nCamAngleY > 180) ? nCamAngleY - 360 : nCamAngleY;
            nCamAngleY += RotateDeltaX * m_CamHorizontalSpeed;

            Vector3 nCamRot = transform.localEulerAngles;
            nCamRot.x = Mathf.Clamp(nCamAngleX, m_CamYMin, m_CamYMax);
            nCamRot.y = nCamAngleY;
            transform.localEulerAngles = nCamRot;
        }

        //Handles Camera Zoom
        mCam.fieldOfView -= m_ZoomSpeed * Zoom * 10;
        mCam.fieldOfView = Mathf.Clamp(mCam.fieldOfView, m_CamZoomMin, m_CamZoonMax);
        mCam.transform.LookAt(mLookAtPos.position);

        //If player is facing a direction different from cam while idle
        //The player will start to rotate that direction
        //Implment this after you get standard movement rotation to work

    }
}
