using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public Dictionary<string, bool> isHaveItems = new();

    private void Awake()
    {
        Instance = this;
        InitItems();
    }

    private void InitItems()
    {
        isHaveItems.Add("Flashlight", false);
        isHaveItems.Add("Document", false);
        isHaveItems.Add("CCTV", false);
        isHaveItems.Add("Key", false);
    }

    public void SetItemState(string itemName, bool isHave) //���º���
    {
        if (isHaveItems.ContainsKey(itemName))
        {
            isHaveItems[itemName] = isHave;
        }
        else
        {
            print("������ �̸� ��Ÿ");
        }
    }
}
