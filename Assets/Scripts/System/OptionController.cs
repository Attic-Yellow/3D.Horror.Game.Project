using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;

    private void Start()
    {
        if (pages != null)
        {
            foreach (GameObject page in pages)
            {
                page.SetActive(false);
            }

            pages[0].SetActive(true);
        }
    }

    public void OnClickBackButton(int pageNum)
    {
        pages[pageNum].SetActive(false);
        pages[0].SetActive(true);
    }

    public void OnClickNextButton(int pageNum)
    {
        pages[0].SetActive(false);
        pages[pageNum].SetActive(true);
    }

    public void OnClickCloseButton()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        pages[0].SetActive(true);
    }
}
