using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Container : MonoBehaviour
{
    public bool isOpen;
    public Transform inRoomTransform;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip openClip;
    [SerializeField] protected AudioClip closeClip;
    [SerializeField] protected TextMeshProUGUI[] openText;
    [SerializeField] protected TextMeshProUGUI[] closeText;

    protected void Awake()
    {
        source = GetComponent<AudioSource>();
        /*if (source != null && GameManager.instance.settingsManager != null)
        {
            GameManager.instance.settingsManager.AddSfxSourceList(source);
        }*/
    }

    protected void Start()
    {
        for (int i = 0; i < openText.Length; i++)
        {
            if (openText != null)
            {
                openText[i].gameObject.SetActive(!isOpen);
            }
            
            if (closeText.Length != 0)
            {
                closeText[i].gameObject.SetActive(isOpen);
            }
        }
    }
}
