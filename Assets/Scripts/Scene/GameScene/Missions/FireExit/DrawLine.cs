using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    private Image img; //바꿀 이미지
    private DrawLineRenderer drawLineRenderer; // DrawLineRenderer 스크립트 참조

    private void Awake()
    {
        img = GetComponent<Image>();
        drawLineRenderer = GetComponent<DrawLineRenderer>();
        drawLineRenderer.enabled = false; // 초기에는 비활성화

    }

    public void OnMouseUpAsButton()
    {
        print("버튼");
        drawLineRenderer.enabled = true;
    }
    public void ChangeImg(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        img.sprite = sprite;
    }
  
}
