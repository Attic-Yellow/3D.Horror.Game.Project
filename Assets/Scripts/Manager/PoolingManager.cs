using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private int poolSize;

    [SerializeField] private Transform objParent; //담을곳
    [SerializeField] private List<GameObject> onThePlaneObjs; //바닥위 생성할 오브젝트
    [SerializeField] private List<GameObject> inTheObjs; //어디 안에 생성할 오브젝트
    [SerializeField] private List<GameObject> missionObjs; //미션에 사용할 오브젝트

    [SerializeField] private List<Transform> planeTf; //바닥 위
    [SerializeField] private List<Transform> inTheTf;// ex) 서랍 안 , 캐비넷 안
    [SerializeField] private List<Transform> missionTF0; //첫미션
    [SerializeField] private List<Transform> missionTF1;
    [SerializeField]private List<Transform> missionTF2;
    [SerializeField]private List <Transform> missionTF3;
    [SerializeField] private List<Transform> missionTF4;

    private List<List<Transform>> missionTFsList = new();
    public List<int> missionObjCount = new();
    private Dictionary<GameObject, List<GameObject>> objectPools = new();

    private int randomIndex;
    private bool isDrawerObj = false; 
    
    private void Start()
    {
        GameManager.instance.poolingManager = this;
        AddTFLists();
        //InitObjects(onThePlaneObjs, planeTf,poolSize);
        InitObjects(inTheObjs, inTheTf, 1);
        InitMissionObjects();
    }

    private void AddTFLists()
    {
        missionTFsList.Add(missionTF0);
        missionTFsList.Add(missionTF1);
        missionTFsList.Add(missionTF2);
        missionTFsList.Add(missionTF3);
        missionTFsList.Add(missionTF4);
    }

    public void InitPool(GameObject objPrefab, int poolSize)
    {
        if (!objectPools.ContainsKey(objPrefab)) //생성한적 없으면 생성하고 리스트에 추가
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(objPrefab);
                obj.name = objPrefab.name;
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
            newObj.transform.parent = objParent;
            objectPools[objPrefab].Add(newObj);
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

    private void InitObjects(List<GameObject> objectList, List<Transform> positionList, int num)
    {
        foreach (var gameObject in objectList)
        {
            if (gameObject != null)
            {
                int randomNum;
                InitPool(gameObject, num);
                if (num == 1)
                {
                    randomNum = 1;
                }
                else
                {
                    randomNum   = Random.Range(1, num + 1); // 최대값을 원하는 개수로 지정
                }
                InitObjOnPosition(gameObject, positionList, randomNum);
            }
        }
    }

    private Vector3 SetObjPos(List<Transform> positions)
    {
        if (positions.Count == 0)
        {
            print("생성할 곳이 없음");
            return Vector3.zero;
        }

        randomIndex = Random.Range(0, positions.Count);
        Vector3 position = positions[randomIndex].position;
        
       isDrawerObj = positions[randomIndex].GetComponentInParent<Drawer>() != null ? true : false;
        
      
        positions.RemoveAt(randomIndex);
        print($"남은 위치 카운트{positions.Count}");
        return position;
    }

    private void InitObjOnPosition(GameObject objPrefab, List<Transform> positionList, int num)
    {
        print($"{num}");
        for (int i = 0; i < num; i++)
        {
            print($"현재 카운트{positionList.Count}");
            GameObject obj = GetPool(objPrefab);
            if (obj != null)
            {
                Vector3 position = SetObjPos(positionList);
                obj.transform.position = position;
              
                if(isDrawerObj)
                {
                    obj.transform.SetParent(positionList[randomIndex]);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    isDrawerObj = false;
                }
                
              
            }
         
        }
    }
    private void InitMissionObjects()
    {
       for(int i = 0; i < missionObjs.Count; i++)
        {
            print($"i의 값 {i}"); ;
                InitPool(missionObjs[i], poolSize); //11개 생성
                int randomNum = Random.Range(1, poolSize+1);
                InitObjOnPosition(missionObjs[i], missionTFsList[i], randomNum);

                // 미션 오브젝트의 개수를 missionObjCount 리스트에 추가
                missionObjCount.Add(randomNum);
                      }
        }
}
  
