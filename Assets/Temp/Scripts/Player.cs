using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    public float interactionDistance = 5f;
    public List<Item> haveitems = new();
    public TextMeshProUGUI interactionText; //��ȣ�ۿ� �ؽ�Ʈ

    public Transform ItemPos;
    Item prevHitItem;

    public Rig rig;
    public RigTarget rigTarget;

    public Door door;
    public bool isOpened; //��������

    Transform canvas;
    GameObject panel;
    private void Awake()
    {
        rig.weight = 0f;
        canvas = FindObjectOfType<Canvas>().transform;
    }
    private void Update()
    {


        Ray ray = Camera.main.ScreenPointToRay(
      new Vector3(Screen.width / 2, Screen.height / 2, 0)); //ȭ�� ����� ���̸� ��
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // �浹�� ��ü�� ��ȣ�ۿ� ������ ��ü���� Ȯ��
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
            else if (hit.collider.CompareTag("OutDoor"))
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                door.isOut = true;
                DoorCheck();


            }
            else if (hit.collider.CompareTag("InDoor"))
            {
                door = hit.collider.gameObject.GetComponentInParent<Door>();
                door.isOut = false;
                DoorCheck();
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }

        }
    }


    void OnInteraction() //F ��ȣ�ۿ�Ű 
    {
        if (prevHitItem != null && interactionText.gameObject.activeSelf)
        {
            // ��ȣ�ۿ� ������ �������� �ְ� ��ȣ�ۿ� �ؽ�Ʈ�� Ȱ��ȭ�� ������ ��
            ItemManager.Instance.SetItemState(prevHitItem.name, true); // �������� ������ BOOL ���� TRUE�� ����
            haveitems.Add(prevHitItem); // �������� �κ��丮�� �߰�
            prevHitItem.tag = "Untagged";
            prevHitItem.SetTransform(ItemPos);
            prevHitItem.gameObject.SetActive(false);
            prevHitItem = null;
        }
        if (door != null && interactionText.gameObject.activeSelf)
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
        interactionText.gameObject.SetActive(false); // ��ȣ�ۿ� �ؽ�Ʈ ��Ȱ��ȭ
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
    void DoorCheck()
    {
        if (door == null)
            return;
        if (door.isOpen)
        {
            interactionText.text = door.closeText;
        }
        else
        {
            interactionText.text = door.openText;
        }
        interactionText.gameObject.SetActive(true);
    }

}
