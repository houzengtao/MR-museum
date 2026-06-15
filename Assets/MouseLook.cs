using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
 /*      public float mouseSensitivity = 10000.0f;
        public float clampAngle = 90.0f;//卡的上下，左右不管，都是360°

        private float rotY = 0.0f; // rotation around the up/y axis
        private float rotX = 0.0f; // rotation around the right/x axis

        void Start()
        {
            Vector3 rot = gameObject.transform.localRotation.eulerAngles;
            rotY = rot.y;
            rotX = rot.x;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            gameObject.transform.rotation = localRotation;
    }*/
    //以上的例子是鼠标移动旋转的，不实用，纪念一下就算了。
    //以下例子是点鼠标拖动的，挺简洁的，记得下面的代码一定挂在maincamera上。
   void Start()
    {
 
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(transform.position,-Vector3.up, Input.GetAxis("Mouse X"));
            transform.RotateAround(transform.position, transform.right, Input.GetAxis("Mouse Y"));
        }
    }
}
