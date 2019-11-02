using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public static PlayerCar instance;

    private float CarSideSpeed = 6.0f;

    //[HideInInspector]
    public float CarSpeed;

    [HideInInspector]
    public float DistanceCovered;

    public float MaxDistanceToCover;

    [HideInInspector]
    public float CarMaxSpeed = 40.0f;

    //public float StartTime;
    //public float CurrentTime;

    //int AverageSpeedCounter = 0;
    //int TotalSpeed = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CarSpeed = 0.0f;
        DistanceCovered = 0.0f;
    }

    private void OnEnable()
    {
        if(Game_Manager.instance.mGameState != Game_Manager.GameState.GamePaused)
        {
            StartCoroutine(StartBlinkingCar());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Game_Manager.instance.mGameState == Game_Manager.GameState.GameOn || Game_Manager.instance.mGameState == Game_Manager.GameState.GameEnding)
        {
            ControlLeftRightCarMovement();
            ControlCarForwardMovement();
            SlowDownTheCar();
            CalculateDistanceCoveredByCar();
        }
    }

    void ControlLeftRightCarMovement()
    {
        this.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * CarSideSpeed, Space.World);

        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, PathManagerScript.instance.leftPathLimit.position.x,
                                                PathManagerScript.instance.rightPathLimit.position.x), this.transform.position.y, this.transform.position.z);
    }

    void ControlCarForwardMovement()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && this.CarSpeed < this.CarMaxSpeed)
        {
            this.CarSpeed += 10 * Time.deltaTime;

            if(this.CarSpeed > 0 && this.CarSpeed < 40 && 
                PathManagerScript.instance.PooledPaths.Find(e => e.GetComponent<Path>().order == 1).GetComponent<Path>().StartSign.activeInHierarchy)
            {
                PathManagerScript.instance.PooledPaths.Find(e => e.GetComponent<Path>().order == 1).GetComponent<Path>().StartSign.SetActive(false);
            }
        }
    }

    void SlowDownTheCar()
    {
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && this.CarSpeed > 0.0f)
        {
            this.CarSpeed -= 10 * Time.deltaTime;
        }
    }

    void CalculateDistanceCoveredByCar()
    {
        if(CarSpeed > 0)
        {
            DistanceCovered += CarSpeed * Time.deltaTime * 5;
        }
    }

    public IEnumerator StartBlinkingCar()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 6; i++)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = !this.gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.5f);
        }

        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Game_Manager.instance.mGameState = Game_Manager.GameState.GameOn;
        AICarManager.instance.ActivateAICarsInitially();
        StopCoroutine("StartBlinkingCar");
    }

    public IEnumerator StartBlinkingCarAfterPause()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 6; i++)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = !this.gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.5f);
        }
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Game_Manager.instance.mGameState = Game_Manager.GameState.GameOn;
        StopCoroutine("StartBlinkingCarAfterPause");
    }
}
