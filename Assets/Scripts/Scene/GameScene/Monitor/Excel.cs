using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Excel : MonoBehaviour
{
    public TMP_InputField inputCount;
    public TextMeshProUGUI objNameText;
    public TextMeshProUGUI explainText; 
    public TextMeshProUGUI priceText;
    private TextMeshProUGUI SumText;
    private string errorMessage = "null#"; //오류메세지
    [SerializeField] private string objTextName;
    [SerializeField] private string explainTextName;
    private void Awake()
    {
         SumText = GetComponentInChildren<TextMeshProUGUI>();   
         objNameText.text = objTextName;
        if(explainTextName != null)
        explainText.text = explainTextName;

    }
    private void Update()
    {
        int count;
        int itemPrice;

        if (int.TryParse(inputCount.text, out count) && int.TryParse(priceText.text, out itemPrice))
        {
            int sum = count * itemPrice;
   
            SumText.text = AddCommas(sum);
            
        }
        else
        {
           SumText.text = (errorMessage);
        }
    }

    private string AddCommas(int number)
    {
        return string.Format("{0:#,###}", number);
    }
}
