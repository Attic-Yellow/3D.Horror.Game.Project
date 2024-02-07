using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVTranslator : MonoBehaviour
{
    private string csvFileName;
    private string jsonFileName;
    private AssetBundle myLoadedAssetBundle;

    private void Awake()
    {
        StartCoroutine(ConvertToJsonCoroutine());
    }

    IEnumerator ConvertToJsonCoroutine()
    {
        csvFileName = $"{GameManager.instance.GetCurrentLanguage()}.csv";
        jsonFileName = $"{GameManager.instance.GetCurrentLanguage()}.json";

        // ���� ���� �ε�
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Language"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        TextAsset csvTextAsset = myLoadedAssetBundle.LoadAsset<TextAsset>(csvFileName);
        if (csvTextAsset == null)
        {
            Debug.Log($"Failed to load {csvFileName} from AssetBundle!");
            yield break;
        }

        Dictionary<string, string> uiTexts = new Dictionary<string, string>();

        string[] csvLines = csvTextAsset.text.Split('\n');

        foreach (string line in csvLines)
        {
            string[] splitLine = line.Split(',');
            if (splitLine.Length < 2) continue; // ��ȿ���� ���� ���� �ǳʶݴϴ�.

            string key = splitLine[0].Trim();
            string value = splitLine[1].Trim();

            // UI �ؽ�Ʈ�� ��ųʸ��� �߰�
            uiTexts[key] = value;
        }

        var combinedData = new
        {
            uiTexts
        };

        string json = JsonConvert.SerializeObject(combinedData, Formatting.Indented);

        // ���� ���� ��ε�
        myLoadedAssetBundle.Unload(false);
    }
}
