using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CheckDraggableObjects : MonoBehaviour
{
    public List<DragAndDrop> draggableObjs = new();
    [SerializeField]private List<bool> isOn = new();
    [SerializeField] private  bool isCheck = false;
    [SerializeField] private bool isSuccess = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        animator.SetBool("IsCheck",isCheck);
      
    }

    private void OnMouseUpAsButton()
    {
        CheckActivatable();
        if (isCheck)
        {
            print("들어왔어");
            animator.SetBool("Success", isSuccess);

        }
    }

    public void CheckActivatable() 
    {
        Activatable activatable = FindObjectOfType<Activatable>();

        foreach (var obj in draggableObjs)
        {
            if(obj.transform.localPosition.x == obj.minX)
            {
                isOn.Add(true);
            }
            if(obj.transform.localPosition.x == obj.maxX)
            {
                isOn.Add(false);
            }
           
        }
        if(activatable.activatable.Count != isOn.Count) //올바르게 되어 있지 않다는 것
        {
            isSuccess = false;
            isCheck = true;
            return;
        }
        else
        {
            for (int i = 0; i < activatable.activatable.Count; i++)
            {
                if (activatable.activatable[i] != isOn[i])
                {
                    isSuccess = false;
                    isCheck = true;
                    return;
                }
            }
            isSuccess = true;
            isCheck = true;
        }
    }
    public void IsCheckFalse() //애니메이션 끝날때 호출
    {
        isCheck = false;
        isOn.Clear();
        if(isSuccess)
        {
            ElectricalShield electricalShield = FindObjectOfType<ElectricalShield>();
            electricalShield.MissionCompleted();
            //성공시 처리할 작업
        }
    }
   
}
