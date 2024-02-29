using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private GameObject lastHitGameObject = null;

    public float interactionDistance = 5f;
    public List<Item> haveitems = new();

    public CinemachineVirtualCameraBase camera2;
    public PlayableDirector timeline;
    protected bool timelineFinsish = false;

    public Transform ItemPos;
    private Item prevHitItem;

    public Item currentItem;
    public Rig rig;
    public RigBuilder rigBuilder;
    public RigTarget rigTarget;
    public RigHint rigHint;
    public ComeGhost comeGhost;
    public Locker locker;
    private Enemy collisionEnemy;

    private bool isPaused = false;
    public bool isOver = false;
    private Battery battery;

    private void Awake()
    {
        rig.weight = 0f;
        rigBuilder.enabled = false;
        timeline = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (isOver)
        {
            TimelineEndCheck();
            return;
        }
        RayCheck();
        
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

        if (locker != null)
        {
            locker = null;
        }

        if (battery != null)
        {
            battery = null;
        }
    }

 

   private void OnInteraction() //F 상호작용키 
    {
        
        if (prevHitItem != null)
        {
            // 상호작용 가능한 아이템이 있고 상호작용 텍스트가 활성화된 상태일 때
            Holder.Instance.SetItemState(prevHitItem.name, true); // 아이템을 먹히고 BOOL 값을 TRUE로 설정
            haveitems.Add(prevHitItem) ;
            prevHitItem.tag = "Untagged";
            prevHitItem.SetTransform(ItemPos);
            prevHitItem.gameObject.SetActive(false);
            prevHitItem = null;
        }

        if (lastHitGameObject != null && lastHitGameObject.gameObject.GetComponentInParent<Door>() != null)
        {
            Door door = lastHitGameObject.gameObject.GetComponentInParent<Door>();

            if (door.isOpen)
            {
                door.CloseDoor();
            }
            else
            {
                door.OpenDoor();
            }
        }

        if (lastHitGameObject != null && lastHitGameObject.gameObject.GetComponentInParent<Shield>() != null)
        {
            Shield shield = lastHitGameObject.gameObject.GetComponentInParent<Shield>();

            if (shield.isOpen)
            {
                shield.OpenShield();
            }
        }

        if (lastHitGameObject != null && lastHitGameObject.gameObject.GetComponentInParent<Drawer>() != null)
        {
            Drawer drawer = lastHitGameObject.gameObject.GetComponentInParent<Drawer>();
            drawer.DrawerController();
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
        if (battery != null && Holder.Instance.isHaveItems["Flashlight"])
        {
            battery.Use();
            Destroy(battery.gameObject);
            battery = null;
        }
    }

   private void OnFlashlight() //Q누르면
    {
        if (Holder.Instance.isHaveItems.ContainsKey("Flashlight"))
        {
            foreach (Item item in haveitems)
            {
                if (item.GetComponent<Flashlight>() != null)
                {
                    if (!item.gameObject.activeSelf)
                    { 
                        ItemActive(item);

                    }
                    else
                    {
                        ItemDisable(item);



                    }
                    break;
                }

            }
        }
    }

    private void OnCCTV()
    {
        if (Holder.Instance.isHaveItems.ContainsKey("CCTV"))
        {
            foreach (Item item in haveitems)
            {
                if (item.GetComponent<CCTV>() != null)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        ItemActive(item);
                    }
                    else
                    {
                        ItemDisable(item);
                    }
                    break;
                }

            }
        }
    }
    private void OnTab()
    {
        if (currentItem != null)
        {
            ItemDisable(currentItem);
        }

            if (Holder.Instance.isHaveItems.ContainsKey("WalkList"))
            {
                foreach (Item item in haveitems)
                {
                    if (item.GetComponent<CurrentWorkList>() != null)
                    {
                        if (!item.gameObject.activeSelf)
                        {
                            item.gameObject.SetActive(true);

                        }
                        else
                        {
                            ItemDisable(item);

                        }
                    }
                }

            }
        

    }

    // Esc키를 누르면 일시정지 및 옵션창 활성화
    private void OnPause()
    {
        isPaused = !isPaused;

        if (!cameraZoom.isZoomIn)
        {
            cameraController.SetOverlayCamAtive();
            cameraController.SetPointCamActive();
            Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = true;
        }

        GameManager.instance.overlayManager.OptionOverlayController();
    }


    private void ItemActive(Item item)
    {
        item.gameObject.SetActive(true);
        currentItem = item;
        rig.weight = 1;
        rigHint.SetTarget(currentItem.transform);
        rigTarget.SetTarget(currentItem.transform);
        rigBuilder.enabled = true;
    }
    private void ItemDisable(Item item)
    {
        item.gameObject.SetActive(false);
        currentItem = null;
        rigBuilder.enabled = false;
        rig.weight = 0;
    }

    private void PositionAndRotation(Transform _tf)
    {
        camera2.transform.SetPositionAndRotation(_tf.position, _tf.rotation);
    }
    private void OnTimeline()
    {
        PositionAndRotation(collisionEnemy.gameoverCamPos);
        timeline.Play();
    }
    public void CameraPriorityChange(int _num)
    {
        camera2.Priority = _num;
    }
    private void CameraChange(PlayableDirector director)
    {
        timelineFinsish = true;
        CameraPriorityChange(11);
    }

    private void TimelineEndCheck()
    {
        if (!timelineFinsish)
            timeline.stopped += CameraChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            collisionEnemy = other.gameObject.GetComponent<Enemy>();
            
            print("닿았어");
            isOver = true;
            OnTimeline();
            camera2.LookAt = collisionEnemy.enemySpine;
        }
    }

    private void RayCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, new Color(1, 0, 1));

        if (Physics.Raycast(ray, out hit, interactionDistance) && !cameraZoom.isZoomIn)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interaction Object"))
            {
                if (lastHitGameObject != null)
                {
                    lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                }

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
                else if (hit.collider.CompareTag("OutDoor"))
                {
                    if (lastHitGameObject != null)
                    {
                        lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                    }

                    lastHitGameObject = hit.collider.gameObject;
                    lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject.gameObject.GetComponentInParent<Door>().isOut = true;
                }
                else if (hit.collider.CompareTag("InDoor"))
                {
                    if (lastHitGameObject != null)
                    {
                        lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                    }

                    lastHitGameObject = hit.collider.gameObject;
                    lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject.gameObject.GetComponentInParent<Door>().isOut = false;
                }
                else if (hit.collider.CompareTag("Drawer"))
                {
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject = hit.collider.gameObject;
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
                else if (hit.collider.CompareTag("MissionItem"))
                {
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject = hit.collider.gameObject;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject gameObject = hit.collider.gameObject.transform.Find("Target").gameObject;
                        cameraZoom.LookAtZoomIn(gameObject);
                        hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
                        cameraZoom.MissionOverlayControl(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Locker"))
                {
                    locker = hit.collider.gameObject.GetComponent<Locker>();
                    lastHitGameObject = hit.collider.gameObject;
                }
                else if (hit.collider.CompareTag("Battery"))
                {
                    battery = hit.collider.gameObject.GetComponent<Battery>();
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject = hit.collider.gameObject;
                }
                else if (hit.collider.CompareTag("Switch"))
                {
                    hit.collider.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(true);
                    lastHitGameObject = hit.collider.gameObject;
                }
                else if (hit.collider.CompareTag("Ghost"))
                {
                    comeGhost.isSee = true;
                }
                else
                {
                    ResetInteractions();
                }
            }
        }
        else
        {
            ResetInteractions();
        }
    }
}