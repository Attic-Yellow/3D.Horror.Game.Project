using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
   private Image backgroundImage;
    private DrawLineRenderer drawLineRenderer; // DrawLineRenderer ��ũ��Ʈ ����
   /* private CameraZoom cameraZoom;
    public GameObject target;*/
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        drawLineRenderer = GetComponent<DrawLineRenderer>(); // DrawLineRenderer ��ũ��Ʈ�� ã��
        drawLineRenderer.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
       /* cameraZoom = FindObjectOfType<CameraZoom>();*/
    }

    public void OnMouseUpAsButton()
    {
        /*cameraZoom.LookAtZoomIn(target);
        cameraZoom.MissionOverlayControl(target);*/
        print("��ư");
        drawLineRenderer.enabled = true;
        //TODO : �Ͼ���̹����� ��ũ���� �����ؼ� �׸��� �׸��� �ְ� ��ȭ�� ����. 

    }
    public void ChangeImg(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        backgroundImage.sprite = sprite;
    }
  
}
