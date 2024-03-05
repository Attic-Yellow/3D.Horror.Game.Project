using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExcelRow : MonoBehaviour
{
    public TMP_InputField inputCount;
    public TextMeshProUGUI objNameText;
    public TextMeshProUGUI explainText; 
    public TextMeshProUGUI priceText;
    private TextMeshProUGUI SumText;
    private string errorMessage = "null#"; //오류메세지
    [SerializeField] private string objTextName;
    [SerializeField] private string explainTextName;

    public int num;
    public bool isTrue;
    private void Awake()
    {
         SumText = GetComponentInChildren<TextMeshProUGUI>();   
         objNameText.text = objTextName;
        if(explainTextName != null)
        explainText.text = explainTextName;

    }
    private void Update()
    {

        if (int.TryParse(inputCount.text, out int count) && int.TryParse(priceText.text, out int itemPrice))
        {
            int sum = count * itemPrice;
            if(count == GameManager.instance.poolingManager.missionObjCount[num])
            {
                isTrue = true;
            }
            else
            {
                isTrue = false;
            }
            SumText.text = AddCommas(sum);

        }
        else
        {
            isTrue = false;
            SumText.text = (errorMessage);
        }
    }

    private string AddCommas(int number)
    {
        return string.Format("{0:#,###}", number);
    }
}
