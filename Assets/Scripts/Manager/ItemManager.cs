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

    public void SetItemState(string itemName, bool isHave) //상태변경
    {
        if (isHaveItems.ContainsKey(itemName))
        {
            isHaveItems[itemName] = isHave;
        }
        else
        {
            print("아이템 이름 오타");
        }
    }
}
