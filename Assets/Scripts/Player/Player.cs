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

    public Transform ItemPos;
    private Item prevHitItem;

    public Rig rig;
    public RigTarget rigTarget;

    public Door door;
    private Battery battery;
    public bool isOpened; //열었는지

    private Enemy collisionEnemy;
    public ComeGhost comeGhost;
    public Locker locker;

    private bool isPaused = false;
    public bool isOver;

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
                if (item != prevHitItem && hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.activeSelf)
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
                    hit.collider.gameObject.transform.Find("Screen").gameObject.GetComponent<MonitorControl>().OnAndOff();
                    cameraZoom.MonitorOn();
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                }
            }
            else if (hit.collider.CompareTag("Ghost"))
            {
                comeGhost.isSee = true;
            }
            else if (hit.collider.CompareTag("Locker"))
            {
                locker = hit.collider.gameObject.GetComponent<Locker>();
                lastHitGameObject = hit.collider.gameObject;
            }
            else if(hit.collider.CompareTag("Battery") && Holder.Instance.isHaveItems["Flashlight"])
            {
                battery = hit.collider.gameObject.GetComponent<Battery>();
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

        if (locker != null)
        {
            locker = null;
        }
        if(battery != null)
        {
            battery = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            collisionEnemy = other.gameObject.GetComponent<Enemy>();
            print("닿았어");
            isOver = true;
        }
    }

    void OnInteraction() //F 상호작용키 
    {
        if (prevHitItem != null)
        {
            // 상호작용 가능한 아이템이 있고 상호작용 텍스트가 활성화된 상태일 때
            Holder.Instance.SetItemState(prevHitItem.name, true); // 아이템을 먹히고 BOOL 값을 TRUE로 설정
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
        if (locker != null && !locker.timelinePlaying)
        {
            if (locker.isIn)
            {
                locker.OnTimeline();
            }
            else
            {
                locker.ReverseTimeline();
            }
        }
        if(battery !=null)
        {
            battery.Use();
            Destroy(battery.gameObject);
        }
    }

    void OnFlashlight() //Q누르면
    {
        if (Holder.Instance.isHaveItems.ContainsKey("Flashlight"))
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
       
    }

    // Esc키를 누르면 일시정지 및 옵션창 활성화
    void OnPause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        GameManager.instance.overlayManager.OptionOverlayController();
    }
}
