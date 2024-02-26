using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activatable : MonoBehaviour
{
  public List<bool> activatable = new();
  public List<Slider> sliders = new();
   private int listCount = 15;

    private void Awake()
    {
 
        for(int i = 0; i < listCount; i++)
        {
            int radomNum = Random.Range(0, 2);
            bool isTrue;
            if (radomNum == 0)
            {
                isTrue = true;
            }
            else
            {
                isTrue = false;
            }
            sliders[i].value = radomNum;
            activatable.Add(isTrue);
        }
    }

  

}
