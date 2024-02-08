using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    public float interactionDistance = 5f;
    public List<Item> haveitems = new();
    public TextMeshProUGUI interactionText; //상호작용 텍스트

    public Transform ItemPos;
    Item prevHitItem;

    public Rig rig;
    public RigTarget rigTarget;

    public Door door;
    public bool isOpened; //열었는지

    public Transform canvas;
    GameObject panel;

    private void Awake()
    {
        rig.weight = 0f;
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, new Color(1, 0, 1));

        if (Physics.Raycast(ray, out hit, interactionDistance, LayerMask.GetMask("Interaction Object")))
        {
            // 충돌한 물체가 상호작용 가능한 물체인지 확인
            if (hit.collider.CompareTag("Item"))
            {
                Item item = hit.collider.gameObject.GetComponent<Item>();
                if (item != prevHitItem)
                {
                    interactionText.gameObject.SetActive(false);
                }
                prevHitItem = item;
                interactionText.text = prevHitItem.text;
                interactionText.gameObject.SetActive(true);
            }
            else if (hit.collider.CompareTag("OutDoor") )
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                door.isOut = true;
            }
            else if(hit.collider.CompareTag("InDoor"))
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                door.isOut = false;
            }
            else
            {
                ResetInteractions();
            }
        }
        else
        {
            ResetInteractions();
        }
    }

    private void ResetInteractions()
    {
        if (prevHitItem != null)
        {
            // interactionText.gameObject.SetActive(false);
            prevHitItem = null;
        }

        if (door != null)
        {
            door = null;
        }
    }

    void OnInteraction() //F 상호작용키 
    {
        if (prevHitItem != null && interactionText.gameObject.activeSelf)
        {
            // 상호작용 가능한 아이템이 있고 상호작용 텍스트가 활성화된 상태일 때
            ItemManager.Instance.SetItemState(prevHitItem.name, true); // 아이템을 먹히고 BOOL 값을 TRUE로 설정
            haveitems.Add(prevHitItem); // 아이템을 인벤토리에 추가
            prevHitItem.tag = "Untagged";
            prevHitItem.SetTransform(ItemPos);
            prevHitItem.gameObject.SetActive(false);
            prevHitItem = null;
        }
        if (door != null /*&& interactionText.gameObject.activeSelf*/)
        {
            if (door.isOpen)
            {
                door.CloseDoor();
            }
            else
            {
                door.OpenDoor();
            }
            door = null;
        }
        // interactionText.gameObject.SetActive(false); // 상호작용 텍스트 비활성화
    }

    void OnFlashlight() //Q누르면
    {
        if (ItemManager.Instance.isHaveItems.ContainsKey("Flashlight"))
        {
            foreach (Item item in haveitems)
            {
                if (item.GetComponent<Flashlight>() != null)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        rigTarget.SetTransform(item.handTargetPos);
                        item.gameObject.SetActive(true);
                        rig.weight = 1;
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                        rig.weight = 0;
                    }
                    break;
                }

            }
        }
    }

    void OnCCTV()
    {
        if (ItemManager.Instance.isHaveItems.ContainsKey("CCTV"))
        {
            foreach (Item item in haveitems)
            {
                if (item.GetComponent<CCTV>() != null)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        rigTarget.SetTransform(item.handTargetPos);
                        item.gameObject.SetActive(true);
                        if (panel == null)
                            panel = Instantiate(item.GetComponent<CCTV>().phonePanel);
                        else
                        {
                            panel.SetActive(true);
                        }
                        panel.transform.SetParent(canvas);
                        panel.transform.localPosition = Vector3.zero;

                        rig.weight = 1;
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                        rig.weight = 0;
                        panel.SetActive(false);
                    }
                    break;
                }

            }
        }
    }
}
