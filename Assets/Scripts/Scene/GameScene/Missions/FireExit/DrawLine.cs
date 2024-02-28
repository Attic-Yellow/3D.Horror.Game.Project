using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    private Image img; //�ٲ� �̹���
    private DrawLineRenderer drawLineRenderer; // DrawLineRenderer ��ũ��Ʈ ����

    private void Awake()
    {
        img = GetComponent<Image>();
        drawLineRenderer = GetComponent<DrawLineRenderer>();
        drawLineRenderer.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ

    }

    public void OnMouseUpAsButton()
    {
        print("��ư");
        drawLineRenderer.enabled = true;
    }
    public void ChangeImg(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        img.sprite = sprite;
    }
  
}
