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
    public TextMeshProUGUI interactionText; //��ȣ�ۿ� �ؽ�Ʈ

    public Transform ItemPos;
    Item prevHitItem;

    public Rig rig;
    public RigTarget rigTarget;

    public Door door;
    public bool isOpened; //��������

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

    void OnInteraction() //F ��ȣ�ۿ�Ű 
    {
        if (prevHitItem != null)
        {
            // ��ȣ�ۿ� ������ �������� �ְ� ��ȣ�ۿ� �ؽ�Ʈ�� Ȱ��ȭ�� ������ ��
            ItemManager.Instance.SetItemState(prevHitItem.name, true); // �������� ������ BOOL ���� TRUE�� ����
            haveitems.Add(prevHitItem); // �������� �κ��丮�� �߰�
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

    void OnFlashlight() //Q������
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

    // EscŰ�� ������ �Ͻ����� �� �ɼ�â Ȱ��ȭ
    void OnPause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        GameManager.instance.overlayManager.OptionOverlayController();
        Cursor.visible = isPaused;
    }
}
