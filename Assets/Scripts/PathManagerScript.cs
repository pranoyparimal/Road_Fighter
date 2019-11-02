using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManagerScript : MonoBehaviour
{
    public static PathManagerScript instance;

    [SerializeField]
    private GameObject PathPrefab;


    [SerializeField]
    private int NoOfFPooledPaths;


    public List<GameObject> PooledPaths;

    public Transform leftPathLimit;
    public Transform rightPathLimit;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PooledPaths = new List<GameObject>();
        Invoke("GeneratePoolOfPaths", 0.0f);
    }

    void GeneratePoolOfPaths()
    {
        for(int i = 0; i < NoOfFPooledPaths; i++)
        {
            GameObject obj = Instantiate(PathPrefab);
            obj.name = "Path";
            obj.GetComponent<Path>().StartSign.SetActive(false);
            obj.GetComponent<Path>().FinishSign.SetActive(false);
            obj.SetActive(false);
            PooledPaths.Add(obj);
        }
    }

    public void ActivatePaths()
    {
        //20.86
        int j = 0;
       foreach(GameObject _obj in PooledPaths)
        {
            if (!_obj.activeInHierarchy)
            {
                if(j == 0)
                {
                    _obj.GetComponent<Path>().order = 1;
                    _obj.GetComponent<Path>().StartSign.SetActive(true);
                    _obj.transform.position = Vector3.zero;

                }
                else if(j == 1)
                {
                    _obj.GetComponent<Path>().order = 2;
                    Vector3 _pos = _obj.transform.position;
                    _obj.transform.position = new Vector3(_pos.x, _pos.y+20.86f, _pos.z);
                }
                _obj.SetActive(true);
                j++;

                if(j == 2)
                {
                    break;
                }
            }
        }
    }

    public void GenerateNextPath(Vector3 _pos)
    {
        foreach (GameObject _obj in PooledPaths)
        {
            if (!_obj.activeInHierarchy)
            {
                _obj.transform.position = new Vector3(_pos.x, _pos.y + 20.86f, _pos.z);
                GameObject _lowestPath = PooledPaths.Find(e => e.GetComponent<Path>().order == 1);
                _lowestPath.GetComponent<Path>().order = 3;
                GameObject _middlePath = PooledPaths.Find(e => e.GetComponent<Path>().order == 2);
                _middlePath.GetComponent<Path>().order = 1;
                _obj.GetComponent<Path>().order = 2;
                _obj.SetActive(true);
                break;
            }
        }
    }
}
