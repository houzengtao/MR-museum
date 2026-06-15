using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnControl : MonoBehaviour
{
    public Camera mainCamera;
    bool seethroughflag=true;//根据我的场景，一开始是透视的
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBtnSeethroughclick()
    {
        seethroughflag = !seethroughflag;

        if (seethroughflag)
        { 
            // 切换为 MR 模式（显示透视）
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            // 关键：不要用 Color.black，而是 new 一个 RGBA 全为 0 的颜色
            mainCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
        }
        else
            // 切换为纯 VR 模式（显示天空盒）
            mainCamera.clearFlags = CameraClearFlags.Skybox;
    }
}
