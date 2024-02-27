using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CustomUtils.ScreenShot))]
public class ScreenShotEditor : Editor 
{
    CustomUtils.ScreenShot screenShot;
	void OnEnable() => screenShot = target as CustomUtils.ScreenShot;

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();
         if (GUILayout.Button("ScreenShot"))
         {
              screenShot.ScreenShotClick();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        } 
	}
}
#endif

namespace CustomUtils
{
    public class ScreenShot : MonoBehaviour
    {
        private int num = 0;
        [SerializeField] string screenShotName;
        public GameObject backQuad;

        public string ScreenShotClick()
        {
            RenderTexture renderTexture = GetComponent<Camera>().targetTexture;   
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            string filePath = $"{Application.dataPath}/{screenShotName}{num}.png ";
            num++;
            File.WriteAllBytes(filePath, texture.EncodeToPNG());
           return filePath;
        }
    }
}