using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarManager : MonoBehaviour
{
    public static AICarManager instance;

    [SerializeField]
    private GameObject AICarPrefab;


    [SerializeField]
    private int NoOfFPooledAICars;


    public List<GameObject> PooledAICars;

    public List<Transform> AICarSpawnPoints;

    public float[] AICarSpeeds; //{8.1f, 8.3f, 8.6f, 9.2f, 9.6f, 9.8f};

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PooledAICars = new List<GameObject>();

        Invoke("GeneratePoolOfAICars", 0.0f);

        StartCoroutine(RegenerateAICarsAfterInitialization());
    }

    void GeneratePoolOfAICars()
    {
        for (int i = 0; i < NoOfFPooledAICars; i++)
        {
            GameObject obj = Instantiate(AICarPrefab);
            obj.name = "AICar";
            obj.GetComponent<AICar>().AICarSpeed = AICarSpeeds[i];

            if (i % 2 == 1)
            {
                obj.transform.position = new Vector3(obj.transform.position.x - 1.0f, (obj.transform.position.y + (i * 1.1f)), obj.transform.position.z);
            }
            else
            {
                obj.transform.position = new Vector3(obj.transform.position.x + 1.0f, (obj.transform.position.y + (i * 1.1f)), obj.transform.position.z);
            }

            obj.SetActive(false);
            PooledAICars.Add(obj);
        }
    }

    public void SetAICarsInitially()
    {
        //int i = 0;
        foreach (GameObject _obj in PooledAICars)
        {
            _obj.SetActive(true);
        }
    }

    public void ActivateAICarsInitially()
    {
        foreach (GameObject _obj in PooledAICars)
        {
            _obj.GetComponent<AICar>().MoveCarInitially();
        }
    }

    IEnumerator RegenerateAICarsAfterInitialization()
    {
        if(PlayerCar.instance == null)
        {
            yield return 0;
        }
        else
        {
            //Debug.Log("Car's Speed: " + PlayerCar.instance.CarSpeed);
            while (PlayerCar.instance.CarSpeed >= 40.0f)
            {
                ChooseTwoRandomAICarsForCompeting();
                float timeToWait = Random.Range(1.0f, 3.0f);
                yield return new WaitForSeconds(timeToWait);
            }
        }

        StopCoroutine("RegenerateAICarsAfterInitialization");

        yield return new WaitForSeconds(3.0f);
        StartCoroutine(RegenerateAICarsAfterInitialization());
    }

    void ChooseTwoRandomAICarsForCompeting()
    {
        foreach (GameObject _obj in PooledAICars)
        {
            if (!_obj.activeInHierarchy)
            {
                _obj.transform.position = AICarSpawnPoints[Random.Range(0 ,AICarSpawnPoints.Count)].position;
                _obj.SetActive(true);
                break;
            }
        }
    }
}
