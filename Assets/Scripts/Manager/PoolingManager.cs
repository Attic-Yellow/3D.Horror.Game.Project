using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private int poolSize;

    [SerializeField] private Transform objParent; //������
    [SerializeField] private List<GameObject> onThePlaneObjs; //�ٴ��� ������ ������Ʈ
    [SerializeField] private List<GameObject> inTheObjs; //��� �ȿ� ������ ������Ʈ
    [SerializeField] private List<GameObject> missionObjs; //�̼ǿ� ����� ������Ʈ

    [SerializeField] private List<Transform> planeTf; //�ٴ� ��
    [SerializeField] private List<Transform> inTheTf;// ex) ���� �� , ĳ��� ��
    [SerializeField] private List<Transform> missionTF0; //ù�̼�
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
        if (!objectPools.ContainsKey(objPrefab)) //�������� ������ �����ϰ� ����Ʈ�� �߰�
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
            print("���� ����");
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
                    randomNum   = Random.Range(1, num + 1); // �ִ밪�� ���ϴ� ������ ����
                }
                InitObjOnPosition(gameObject, positionList, randomNum);
            }
        }
    }

    private Vector3 SetObjPos(List<Transform> positions)
    {
        if (positions.Count == 0)
        {
            print("������ ���� ����");
            return Vector3.zero;
        }

        randomIndex = Random.Range(0, positions.Count);
        Vector3 position = positions[randomIndex].position;
        
       isDrawerObj = positions[randomIndex].GetComponentInParent<Drawer>() != null ? true : false;
        
      
        positions.RemoveAt(randomIndex);
        print($"���� ��ġ ī��Ʈ{positions.Count}");
        return position;
    }

    private void InitObjOnPosition(GameObject objPrefab, List<Transform> positionList, int num)
    {
        print($"{num}");
        for (int i = 0; i < num; i++)
        {
            print($"���� ī��Ʈ{positionList.Count}");
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
            print($"i�� �� {i}"); ;
                InitPool(missionObjs[i], poolSize); //11�� ����
                int randomNum = Random.Range(1, poolSize+1);
                InitObjOnPosition(missionObjs[i], missionTFsList[i], randomNum);

                // �̼� ������Ʈ�� ������ missionObjCount ����Ʈ�� �߰�
                missionObjCount.Add(randomNum);
                      }
        }
}
  
