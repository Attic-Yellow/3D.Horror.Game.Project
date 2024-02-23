using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private Enemy collisionEnemy;
    public ComeGhost comeGhost;
    public Locker locker;

    private bool isPaused = false;
    public bool isOver;
    private Battery battery;

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
            if (lastHitGameObject != null)
            {
                lastHitGameObject.gameObject.transform.Find("CanvasRoot").gameObject.SetActive(false);
            }

            // �浹�� ��ü�� ��ȣ�ۿ� ������ ��ü���� Ȯ��
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

        if (locker != null)
        {
            locker = null;
        }

        if (battery != null)
        {
            battery = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            collisionEnemy = other.gameObject.GetComponent<Enemy>();
            print("��Ҿ�");
            isOver = true;
        }
    }

    void OnInteraction() //F ��ȣ�ۿ�Ű 
    {
        if (prevHitItem != null)
        {
            // ��ȣ�ۿ� ������ �������� �ְ� ��ȣ�ۿ� �ؽ�Ʈ�� Ȱ��ȭ�� ������ ��
            Holder.Instance.SetItemState(prevHitItem.name, true); // �������� ������ BOOL ���� TRUE�� ����
            haveitems.Add(prevHitItem); // �������� �κ��丮�� �߰�
            prevHitItem.tag = "Untagged";
            prevHitItem.SetTransform(ItemPos);
            prevHitItem.gameObject.SetActive(false);
            prevHitItem = null;
        }
        if (locker != null && !locker.timelinePlaying)
        {
            if (!locker.isIn)
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
        if (lastHitGameObject == null)
        {
            return;
        }
        if (lastHitGameObject.gameObject.GetComponentInParent<Door>() != null)
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

        if (lastHitGameObject.gameObject.GetComponentInParent<Drawer>() != null)
        {
            Drawer drawer = lastHitGameObject.gameObject.GetComponentInParent<Drawer>();
            drawer.DrawerController();
        }




    }


    void OnFlashlight() //Q������
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
        if (Holder.Instance.isHaveItems.ContainsKey("CCTV"))
        {
            foreach (Item item in haveitems)
            {
                if (item.GetComponent<CCTV>() != null)
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

    // EscŰ�� ������ �Ͻ����� �� �ɼ�â Ȱ��ȭ
    void OnPause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        GameManager.instance.overlayManager.OptionOverlayController();
    }
}