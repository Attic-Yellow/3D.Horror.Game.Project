using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Container : MonoBehaviour
{
    public bool isOpen;

    public Transform inRoomTransform;

    [SerializeField] protected TextMeshProUGUI[] openText;
    [SerializeField] protected TextMeshProUGUI[] closeText;

    protected void Start()
    {
        for (int i = 0; i < openText.Length; i++)
        {
            if (openText != null)
            {
                openText[i].gameObject.SetActive(!isOpen);
            }
            
            if (closeText != null)
            {
                closeText[i].gameObject.SetActive(isOpen);
            }
        }
    }
}
