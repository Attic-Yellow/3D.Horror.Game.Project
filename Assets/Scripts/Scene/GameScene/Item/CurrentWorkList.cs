using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurrentWorkList : Item
{



    public void SetPosition(Vector3 vc)
    {
        gameObject.transform.position = vc;
    }

}
