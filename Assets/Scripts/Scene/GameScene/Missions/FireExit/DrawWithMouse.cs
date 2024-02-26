using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FireExit : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 previousPos;

    [SerializeField]
    private float minDistance = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
     lr = GetComponent<LineRenderer>();
        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetMouseButtonDown(0))
        {
           Vector3 currentPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            currentPos.z = 0f;

            if (Vector3.Distance(currentPos,previousPos) >minDistance)
            {
                lr.positionCount++;
                lr.SetPosition(lr.positionCount -1, currentPos);
                previousPos = currentPos;
            }
        }   
     
    }
}
