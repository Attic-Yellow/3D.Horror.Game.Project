using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private int poolSize;

    [SerializeField] private Transform objParent; //������
    [SerializeField] private List<GameObject> onThePlaneObjs; //�ٴ��� ������ ������Ʈ
    [SerializeField] private List<GameObject> inTheObjs; //��� �ȿ� ������ ������Ʈ
    [SerializeField] private List<GameObject> missionObjs; //�̼ǿ� ����� ������Ʈ

    [SerializeField] private List<Transform> planeTf; //�ٴ� ��
    [SerializeField] private List<Transform> inTheTf;// ex) ���� �� , ĳ��� ��
    [SerializeField] private List<Transform> missionTf; //�̼ǿ� ���� ������Ʈ ������ ��ġ

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
        if (!objectPools.ContainsKey(objPrefab)) //�������� ������ �����ϰ� ����Ʈ�� �߰�
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
            print("���� ����");
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
            print("������ ���� ����");
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

                int randomNum = Random.Range(1, num + 1); // �ִ밪�� ���ϴ� ������ ����
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

                int randomNum = Random.Range(1, poolSize + 1); // �ִ밪�� ���ϴ� ������ ����
                InitObjOnPosition(missionObj, missionTf, randomNum);

                // �̼� ������Ʈ�� ������ missionObjCount ����Ʈ�� �߰�
                missionObjCount.Add(randomNum);
            }
        }
    }
}
