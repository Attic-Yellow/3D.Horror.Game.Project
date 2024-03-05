using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private int poolSize;

    [SerializeField] private Transform objParent; //담을곳
    [SerializeField] private List<GameObject> onThePlaneObjs; //바닥위 생성할 오브젝트
    [SerializeField] private List<GameObject> inTheObjs; //어디 안에 생성할 오브젝트
    [SerializeField] private List<GameObject> missionObjs; //미션에 사용할 오브젝트

    [SerializeField] private List<Transform> planeTf; //바닥 위
    [SerializeField] private List<Transform> inTheTf;// ex) 서랍 안 , 캐비넷 안
    [SerializeField] private List<Transform> missionTf; //미션에 사용될 오브젝트 생성할 위치

    public List<int> missionObjCount = new();
    public Dictionary<GameObject, List<GameObject>> objectPools = new();
  
    private void Start()
    {
        GameManager.instance.poolingManager = this;
        InitObjects(onThePlaneObjs, planeTf,poolSize);
        InitObjects(inTheObjs, inTheTf, poolSize);
        InitMissionObjects();

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
            print("직접 생성");
            GameObject newObj = Instantiate(objPrefab);
            newObj.transform.parent = objParent;
            objectPools[objPrefab].Add(newObj);
            return newObj;
            
        }
    }

    private Vector3 SetObjPos(List<Transform> positions)
    {
        if (positions.Count == 0)
        {
            print("생성할 곳이 없음");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, positions.Count);
        if (positions[randomIndex].gameObject.GetComponentInParent<Drawer>() != null)
        {
            return Vector3.zero;
        }
        Vector3 position = positions[randomIndex].position;
        positions.RemoveAt(randomIndex);

        return position;
    }

    private void InitObjects(List<GameObject> objectList, List<Transform> positionList, int num)
    {
        foreach (var gameObject in objectList)
        {
            if (gameObject != null)
            {
                InitPool(gameObject, num);

                int randomNum = Random.Range(1, num + 1); // 최대값을 원하는 개수로 지정
                InitObjOnPosition(gameObject, positionList, randomNum);
            }
        }
    }
    private void InitObjOnPosition(GameObject objPrefab, List<Transform> positionList, int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject obj = GetPool(objPrefab);
            if (obj != null)
            {
                
                Vector3 position = SetObjPos(positionList);
                obj.transform.position = position;
                if(position ==  Vector3.zero)
                {
                    obj.transform.parent = positionList[i].transform;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;  
                }

            }
        }
    }
    private void InitMissionObjects()
    {
        foreach (var missionObj in missionObjs)
        {
            if (missionObj != null)
            {
                InitPool(missionObj, poolSize);

                int randomNum = Random.Range(1, poolSize + 1); // 최대값을 원하는 개수로 지정
                InitObjOnPosition(missionObj, missionTf, randomNum);

                // 미션 오브젝트의 개수를 missionObjCount 리스트에 추가
                missionObjCount.Add(randomNum);
            }
        }
    }
}
