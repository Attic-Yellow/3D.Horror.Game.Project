using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    private Image img; //바꿀 이미지
    [SerializeField] private GameObject signArea;

    private void Awake()
    {
        img = GetComponent<Image>();

    }

    public void OnMouseUpAsButton()
    {
        print("버튼");
        signArea.SetActive(true);
    }
    public void ChangeImg(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        img.sprite = sprite;
    }
  
    public void SetSignArea(bool isBool)
    {
        signArea.SetActive(isBool);
    }
}
