using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
    void Start()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Application.streamingAssetsPath);
        sb.Append("/Language");

        AssetBundle assetBundle = AssetBundle.LoadFromFile(sb.ToString());

        Instantiate(assetBundle);

    }
}
