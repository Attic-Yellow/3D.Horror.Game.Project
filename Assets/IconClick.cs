using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconClick : MonoBehaviour
{
    public GameObject overlayUI;

    public void ClickToIcon()
    {
        print("��ư�� �����µ�");
       overlayUI.SetActive(true);
    }

    public void ClickToDelete()
    {
        overlayUI.SetActive(false);
    }
}
