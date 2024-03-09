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
        // NavMeshSurface의 BuildNavMesh() 메소드를 호출하여 네비게이션 메시를 베이크
        if (meshSurface != null)
        {
            meshSurface.BuildNavMesh();
        }
        else
        {
            Debug.Log("네비매쉬가 없음");
        }
    }
}
