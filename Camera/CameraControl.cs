using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 mouseCameraToWorldPosition;
    Vector3 lastMousePos;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        { 
            Vector3 mousePos = Input.mousePosition;
            mouseCameraToWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
           
            lastMousePos = mouseCameraToWorldPosition;
        }

        CameraMovment();
        RotateCamera();
    }


    void CameraMovment()
    {
        //movment
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }

    void RotateCamera()
    {
        //rotation
        float rotateDir = 0f;
        
        
        if (Input.GetKey(KeyCode.Q)) rotateDir = -1f;
        else if (Input.GetKey(KeyCode.E)) rotateDir = +1f;

        float rotateSpeed = 100f;
        if(Input.GetKey(KeyCode.LeftShift)) transform.eulerAngles += new Vector3(rotateDir * rotateSpeed * Time.deltaTime, 0f, 0);
        else transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }


}
