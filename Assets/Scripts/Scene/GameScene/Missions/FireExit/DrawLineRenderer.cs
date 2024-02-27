using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CustomUtils;

public class DrawLineRenderer : MonoBehaviour
{
    public Material defaultMaterial;

    private LineRenderer lr;  
    private int positionCount = 2; 
    private Vector3 PrevPos = Vector3.zero;
    public bool isDraw = false;
    public ScreenShot ScreenShot;
    [SerializeField] List<GameObject> Lines = new();
    public float camDistance = 0.3f;
    void Update()
    {
        DrawMouse();
        if(isDraw && Input.GetKeyDown(KeyCode.S))
        {
            string savedImagePath = ScreenShot.ScreenShotClick();
            Texture2D savedTexture = LoadTexture(savedImagePath);
            DrawLine draw = GetComponent<DrawLine>();
            draw.ChangeImg(savedTexture);
        }
    }

    void DrawMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance));

        if (Input.GetMouseButtonDown(0))
        {
            CreateLine(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            ConnectLine(mousePos);
        }

    }

    void CreateLine(Vector3 mousePos)
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
        Lines.Add(line);
    }

    void ConnectLine(Vector3 mousePos)
    {
        if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
        {
            PrevPos = mousePos;
            positionCount++;
            lr.positionCount = positionCount;
            lr.SetPosition(positionCount - 1, mousePos);
        }

    }
    Texture2D LoadTexture(string filePath) //���Ͽ��� ��ũ����ã��
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D loadedTexture = new Texture2D(2, 2); // �ӽ÷� ũ�⸦ ����
            loadedTexture.LoadImage(fileData);
            return loadedTexture;
        }
        else
        {
            Debug.LogError($"File not found at path: {filePath}");
            return null;
        }
    }
}
