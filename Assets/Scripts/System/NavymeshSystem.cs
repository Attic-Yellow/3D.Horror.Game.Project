using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavymeshSystem : MonoBehaviour
{
   private NavMeshSurface meshSurface;

    private void Awake()
    {
        meshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        BuildNavMesh();
    }

    private void BuildNavMesh()
    {
        // NavMeshSurface�� BuildNavMesh() �޼ҵ带 ȣ���Ͽ� �׺���̼� �޽ø� ����ũ
        if (meshSurface != null)
        {
            meshSurface.BuildNavMesh();
        }
        else
        {
            Debug.Log("�׺�Ž��� ����");
        }
    }
}
