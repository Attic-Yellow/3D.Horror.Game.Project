using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconClick : MonoBehaviour
{
    public GameObject overlayUI;

    public void ClickToIcon()
    {
        print("버튼을 눌렀는데");
       overlayUI.SetActive(true);
    }

    public void ClickToDelete()
    {
        overlayUI.SetActive(false);
    }
}
