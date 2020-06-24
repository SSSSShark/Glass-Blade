using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//非运行时也触发效果
[ExecuteInEditMode]
//屏幕后处理特效一般都需要绑定在摄像机上
[RequireComponent(typeof(Camera))]

public class BlackAndWhite : MonoBehaviour
{
    //shader 拖过来
    Material material;
    public Shader shader;
	//死亡时的灰度
    [Range(0, 1.0f)]
    public float GrayFactor;
	//死亡状态
    private bool death = false;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("startbaw.");
        material = new Material(shader);
    }

    // Update is called once per frame
    void Update()
    {
        if (GrayFactor < 1)
            GrayFactor += Time.deltaTime * 0.5f;
    }

    public void setDeath()
    {
        death = true;
        Debug.Log("[BlackAndWhite:setDeath()] Player dead.");
    }

    public void setLive()
    {
        death = false;
        Debug.Log("[BlackAndWhite:setLive()] Player alive.");
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material && death)
        {
            //Debug.Log("did something.");
            //设置shader中的_GrayFactor参数
            material.SetFloat("_GrayFactor", GrayFactor);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            //Debug.Log("do nothing.");
            Graphics.Blit(src, dest);
        }
    }
}