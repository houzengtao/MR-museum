using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTeleportation : MonoBehaviour
{
    public int rayLength = 10;
 //   public int collisionLayer;
    bool aboutToTeleport = false;
    Vector3 teleportPos = new Vector3();//target
                                        //    public Material tMat;
    Material Mater, lastMaterial, nextMaterial;
    public LineRenderer laser;
    public GameObject pointer;
    public GameObject player;
    public Material sourceMat;//用于代码新建材质的材质资源
    // Start is called before the first frame update
    void Start()
    {
                lastMaterial = GameObject.Find("Sphere").GetComponent<Renderer>().material;
                nextMaterial = Resources.Load<Material>("Materials/location_61");
        //       laser.SetPosition(0, Vector3.zero);
        //       laser.SetPosition(1, Vector3.zero);
       

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.6f)//右控制器上的前侧RIndexTrigger按键
        {
            pointer.SetActive(true);//pointer显现
            Material mat = new Material(sourceMat); //新建材质；
            mat.shader = Shader.Find("Unlit/Color");//设置材质的shader类型
            mat.color = new Color(1.0f, 0f, 0f, 1.0f);//为材质的颜色赋默认的红色值(rgba通道)
            laser.material = mat;//将创建的材质赋值给线渲染组件
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, origin+transform.forward*10);
                       
            if (Physics.Raycast(origin, transform.forward, out hit, rayLength * 10))//上边已经提了，所以是右控制器上的前侧RIndexTrigger键处发出的射线
            {
                
                laser.SetPosition(1, hit.point);
                if (hit.collider.gameObject.tag == "nextmarker")
                {
                    aboutToTeleport = true;
                    teleportPos = hit.point;
                   
                     mat.shader = Shader.Find("Unlit/Color");//设置材质的shader类型
                     mat.color = new Color(0f, 1.0f, 0f, 1.0f);//为材质的颜色赋绿色值(rgba通道)
                     laser.material = mat;//将创建的材质赋值给线渲染组件
                   
                    //                   pointer.transform.position = hit.point;
                }
                else //法线朝外的实体如cube等才起作用，如果没有碰到任何东西则这里的else也不起作用如全景内球体（shader为panoramic）
                {
                    aboutToTeleport = false;
                    
                    mat.shader = Shader.Find("Unlit/Color");//设置材质的shader类型
                    mat.color = new Color(1.0f, 0f, 0f, 1.0f);//为材质的颜色赋红色值(rgba通道)
                    laser.material = mat;//将创建的材质赋值给线渲染组件
                    //                   pointer.transform.position = hit.point;

                    //                   pointer.transform.position = v1;


                }

            }
                        
        }
        else if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.2f) && aboutToTeleport == true)
        {
            pointer.SetActive(false);
            aboutToTeleport = false;
            laser.SetPosition(0, Vector3.zero);
            laser.SetPosition(1, Vector3.zero);
            //           player.transform.position = teleportPos;
               //         lastMaterial = GameObject.Find("Sphere").GetComponent<Renderer>().material;
                        GameObject.Find("Sphere").GetComponent<Renderer>().material = nextMaterial;
            Debug.Log(nextMaterial.name);
                        if(nextMaterial.name.Equals("location_61"))
                            nextMaterial= Resources.Load<Material>("Materials/location_01");
                        else
                            nextMaterial = Resources.Load<Material>("Materials/location_61");


        }
        else if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.2f) && aboutToTeleport == false)
        {
                   pointer.SetActive(false);
                   laser.SetPosition(0, Vector3.zero);
                   laser.SetPosition(1, Vector3.zero);
        }
    }
}
