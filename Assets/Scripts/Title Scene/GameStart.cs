using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Animator mainCamera;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Camera uiCamera;

    public void StartGameFun()
    {
        mainCamera.SetTrigger("isStart");
        doorAnimator.SetTrigger("isOpen");
        uiCamera.gameObject.SetActive(false);
    }

    private IEnumerator StartGame(float delay)
    {
        while (true)
        {
            
            yield return new WaitForSeconds(delay);
            
            yield return new WaitForSeconds(.5f);
        }
    }
}
