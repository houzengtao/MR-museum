using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinput : MonoBehaviour
{
    Material Mater,lastMaterial;
    float ry = 0;
    float d = 2;//每次移动距离
    bool noleftflag = false;
    bool norightflag = false;
    bool nofrontflag = false;
    bool nobackflag = false;
    // Start is called before the first frame update
    void Start()
    {
        lastMaterial = gameObject.GetComponent<Renderer>().material;
        Mater = Resources.Load<Material>("Materials/location_61");
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {         
            gameObject.GetComponent<Renderer>().material= Mater;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            gameObject.GetComponent<Renderer>().material = lastMaterial;
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger)>0.5f)
        {
            // GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.position = new Vector3(0, 0, 0);//设置OVRCameraRig不行，它不动，始终是（0，0，0）
            gameObject.transform.position = new Vector3(0, 0, 0);
        }
        // returns true if the primary thumbstick has been moved upwards more than halfway.
        // (Up/Down/Left/Right - Interpret the thumbstick as a D-pad).
        // OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft);
        // returns a Vector2 of the primary (typically the Left) thumbstick’s current state.
        // (X/Y range of -1.0f to 1.0f)
        /****************************************************************************************/
        //以下是场景球往左侧旋转，每拨动一次旋转一定角度。
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > -0.5f)//stick处于中间位置
        {
            noleftflag = true;
        }
        //       if(OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < -0.5f)//stick左侧拨动到一半以上的位置有效，与OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft);意思相同但判断更好
        {
            
            if (noleftflag)//如果从非左侧状态转过来的
            {
                // GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.position = new Vector3(0, 0, 0);//设置OVRCameraRig不行，它不动，始终是（0，0，0）
                gameObject.transform.position = new Vector3(0, 0, 0);
                ry = ry - 10;
                gameObject.transform.rotation = Quaternion.Euler(0, ry, 0);
                noleftflag = false;
            }
        }
        /****************************************************************************************/
        //以下是场景球往右侧旋转，每拨动一次旋转一定角度。
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < 0.5f)//stick处于中间位置
        {
            norightflag = true;
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > 0.5f)//stick右侧拨动到一半以上的位置有效
        {

            if (norightflag)//如果从非右侧状态转过来的
            {
                //   GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.position = new Vector3(0, 0, 0);//设置OVRCameraRig不行，它不动，始终是（0，0，0）
                gameObject.transform.position = new Vector3(0,0,0);
                   ry = ry + 10;
                gameObject.transform.rotation = Quaternion.Euler(0, ry, 0);
                norightflag = false;
            }
        }
        /****************************************************************************************/
        //以下是镜头往前移动，每拨动一次移动一定距离。
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y < 0.5f)//stick处于中间位置
        {
            nofrontflag = true;
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y > 0.5f)//stick右侧拨动到一半以上的位置有效
        {

            if (nofrontflag)
            {
                float x, y, z;
                x = gameObject.transform.position.x;
                y = gameObject.transform.position.y;
                z = gameObject.transform.position.z;
                x = x - d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.right);
                y = y - d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.up);
                z = z - d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.forward);
                gameObject.transform.position = new Vector3(x, y, z);
                nofrontflag = false;
            }
        }
        /****************************************************************************************/
        //以下是镜头往后移动，每拨动一次移动一定距离。
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y >- 0.5f)//stick处于中间位置
        {
            nobackflag = true;
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y <- 0.5f)//stick右侧拨动到一半以上的位置有效
        {

            if (nobackflag)
            {
                float x, y, z;
                x = gameObject.transform.position.x;
                y = gameObject.transform.position.y;
                z = gameObject.transform.position.z;
                x = x + d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.right);
                y = y + d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.up);
                z = z + d * Vector3.Dot(GameObject.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform.forward, GameObject.Find("OVRCameraRig").transform.forward);
                gameObject.transform.position = new Vector3(x, y, z);
                nobackflag = false;
            }
        }
    }
}
