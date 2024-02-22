using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Container : MonoBehaviour
{
    public bool isOpen;

    [SerializeField] protected float rotationSpeed = 4f;
    [SerializeField] protected Quaternion startRotation;
    public Transform inRoomTransform;

    [SerializeField] protected TextMeshProUGUI[] openText;
    [SerializeField] protected TextMeshProUGUI[] closeText;

    protected void Awake()
    {
        startRotation = transform.rotation;
    }

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
