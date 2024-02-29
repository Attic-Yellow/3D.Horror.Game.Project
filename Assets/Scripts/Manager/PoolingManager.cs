using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private int poolSize;
    [SerializeField] private int fireExitCount;
    [SerializeField] private int battertCount;
    [SerializeField] private Transform objParent; //부무로 담을 곳
    [SerializeField] private GameObject fireExitPrefab;
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private List<Transform> fireExitTf; //소화기 넣을만한 곳
    [SerializeField] private List<Transform> inTheObjTf;// ex) 서랍 안 , 캐비넷 안
    public Dictionary<GameObject, List<GameObject>> objectPools = new();
  
    private void Start()
    {
        GameManager.instance.poolingManager = this;
        InitPool(fireExitPrefab, poolSize);
        InitPool(batteryPrefab, poolSize);
        for (int i = 0;  i < fireExitCount; i++)
        {
            GameObject initObj = GetPool(fireExitPrefab);
           initObj.transform.position =  SetObjPos(fireExitTf);
        }
       for(int i = 0; i <battertCount; i++)
        {
            GameObject initObj = GetPool(batteryPrefab);
            initObj.transform.position = SetObjPos(inTheObjTf);
        }
       

     
    }

    public void InitPool(GameObject objPrefab, int poolSize)
    {
        if (!objectPools.ContainsKey(objPrefab)) //생성한적 없으면 생성하고 리스트에 추가
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(objPrefab);
                obj.transform.parent = objParent;
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            objectPools.Add(objPrefab, objectPool);
        }
    }

    public GameObject GetPool(GameObject objPrefab)
    {
        if (objectPools.ContainsKey(objPrefab))
        {
            List<GameObject> objectPool = objectPools[objPrefab];
            foreach (GameObject obj in objectPool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
            GameObject newObj = Instantiate(objPrefab);
            objectPool.Add(newObj);
            return newObj;
        }
        else
        {
            print("생성한 풀이 없음");
            return null;
        }
    }

    private Vector3 SetObjPos(List<Transform> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogError("생성할 곳이 없음");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, positions.Count);
        Vector3 position = positions[randomIndex].position;
        positions.RemoveAt(randomIndex); 
        return position;
    }

}
