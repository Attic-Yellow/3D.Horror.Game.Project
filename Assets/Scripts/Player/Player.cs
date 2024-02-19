using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private GameObject lastHitGameObject = null;

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

    private bool isPaused = false;

    private void Awake()
    {
        rig.weight = 0f;
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, new Color(1, 0, 1));

        if (Physics.Raycast(ray, out hit, interactionDistance, LayerMask.GetMask("Interaction Object")) && !cameraZoom.isZoomIn)
        {
            // 충돌한 물체가 상호작용 가능한 물체인지 확인
            if (hit.collider.CompareTag("Item"))
            {
                Item item = hit.collider.gameObject.GetComponent<Item>();
                if (item != prevHitItem)
                {
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                }
                prevHitItem = item;
                hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                lastHitGameObject = hit.collider.gameObject;
            }
            else if (hit.collider.CompareTag("OutDoor") )
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                lastHitGameObject = hit.collider.gameObject;
                door.isOut = true;
            }
            else if(hit.collider.CompareTag("InDoor"))
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                lastHitGameObject = hit.collider.gameObject;
                door.isOut = false;
            }
            else if (hit.collider.CompareTag("Monitor"))
            {
                hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                lastHitGameObject = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject gameObject = hit.collider.gameObject.transform.Find("Target").gameObject;
                    cameraZoom.LookAtZoomIn(gameObject);
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                }
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
        if (lastHitGameObject != null)
        {
            lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
        }

        lastHitGameObject = null;

        if (prevHitItem != null)
        {
            prevHitItem = null;
        }

        if (door != null)
        {
            door = null;
        }
    }

    void OnInteraction() //F 상호작용키 
    {
        if (prevHitItem != null)
        {
            // 상호작용 가능한 아이템이 있고 상호작용 텍스트가 활성화된 상태일 때
            ItemManager.Instance.SetItemState(prevHitItem.name, true); // 아이템을 먹히고 BOOL 값을 TRUE로 설정
            haveitems.Add(prevHitItem); // 아이템을 인벤토리에 추가
            prevHitItem.tag = "Untagged";
            prevHitItem.SetTransform(ItemPos);
            prevHitItem.gameObject.SetActive(false);
            prevHitItem = null;
        }
        if (door != null)
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

    // Esc키를 누르면 일시정지 및 옵션창 활성화
    void OnPause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        GameManager.instance.overlayManager.OptionOverlayController();
        Cursor.visible = isPaused;
    }
}
