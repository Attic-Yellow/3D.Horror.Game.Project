using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CustomUtils;
using UnityEditor;

public class DrawLineRenderer : MonoBehaviour
{
   [SerializeField] private Material defaultMaterial;
    private LineRenderer lr;  
    private int positionCount = 2; 
    private Vector3 PrevPos = Vector3.zero;
    private ScreenShot ScreenShot;
    private bool isDraw = false;
    public int ZCount = 0;

    public List<GameObject> lines = new();
    public float camDistance = 0.1f;
    private void Awake()
    {
        ScreenShot = FindObjectOfType<ScreenShot>();
    }
   private void Update()
    {
        DrawMouse();
    
    }

   private void DrawMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,  + Input.mousePosition.y,  camDistance));

        if (Input.GetMouseButtonDown(0))
        {
            CreateLine(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            ConnectLine(mousePos);
        }

    }

   private void CreateLine(Vector3 mousePos)
    {
        isDraw = true;
        positionCount = 2;
        GameObject line = new GameObject("Line");
        line.layer = LayerMask.NameToLayer("UI");
        LineRenderer lr = line.AddComponent<LineRenderer>();

        line.transform.parent = Camera.main.transform;
        line.transform.position = mousePos;

        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.numCornerVertices = 5;
        lr.numCapVertices = 5;
        lr.material = defaultMaterial;
        lr.SetPosition(0, mousePos);
        lr.SetPosition(1, mousePos);

        this.lr = lr;
        lines.Add(line);
    }

   private  void ConnectLine(Vector3 mousePos)
    {
        if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
        {
            PrevPos = mousePos;
            positionCount++;
            lr.positionCount = positionCount;
            lr.SetPosition(positionCount - 1, mousePos);
        }

    }
   private  Texture2D LoadTexture(string filePath) //파일에서 스크린샷찾기
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D loadedTexture = new Texture2D(2, 2); // 임시로 크기를 설정
            loadedTexture.LoadImage(fileData);
            Debug.Log($"파일을 찾음{filePath}");
            return loadedTexture;

        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없음: {filePath}");
            return null;
        }
    }

    public void DeleteLines()
    {
        if (isDraw)
        {
            string savedImagePath = ScreenShot.ScreenShotClick();
            EditorApplication.ExecuteMenuItem("Assets/Refresh"); //새로고침
            Texture2D savedTexture = LoadTexture(savedImagePath);
            DrawLine draw = GetComponent<DrawLine>();
            draw.ChangeImg(savedTexture);
            foreach (GameObject line in lines)
            {
                Destroy(line);
            }
            lines.Clear();
            isDraw = false;
            ZCount++;
        }
  


    }

 
}
