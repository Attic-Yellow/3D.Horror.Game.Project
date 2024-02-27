using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
   private Image backgroundImage;
    private DrawLineRenderer drawLineRenderer; // DrawLineRenderer 스크립트 참조
   /* private CameraZoom cameraZoom;
    public GameObject target;*/
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        drawLineRenderer = GetComponent<DrawLineRenderer>(); // DrawLineRenderer 스크립트를 찾음
        drawLineRenderer.enabled = false; // 초기에는 비활성화
       /* cameraZoom = FindObjectOfType<CameraZoom>();*/
    }

    public void OnMouseUpAsButton()
    {
        /*cameraZoom.LookAtZoomIn(target);
        cameraZoom.MissionOverlayControl(target);*/
        print("버튼");
        drawLineRenderer.enabled = true;
        //TODO : 하얀색이미지를 스크린에 생성해서 그림을 그릴수 있게 도화지 역할. 

    }
    public void ChangeImg(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        backgroundImage.sprite = sprite;
    }
  
}
