using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    public enum GameState
    {
        None,
        GameStarting,
        GameOn,
        GameEnding,
        GamePaused,
        GameOver
    }
    public GameState mGameState;

    public GameObject Player;

    public GameObject Explosion;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckGameWinCondition();
    }

    void CheckGameWinCondition()
    {
        GameObject _middlePath = null;

        if (mGameState == GameState.GameOn && (PlayerCar.instance.DistanceCovered/1000) > PlayerCar.instance.MaxDistanceToCover)
        {
            _middlePath = PathManagerScript.instance.PooledPaths.Find(e => e.GetComponent<Path>().order == 2);
            _middlePath.GetComponent<Path>().FinishSign.SetActive(true);

            mGameState = GameState.GameEnding;
        }


        GameObject finishingPath = PathManagerScript.instance.PooledPaths.Find(e => e.GetComponent<Path>().FinishSign.activeInHierarchy);

        if (finishingPath != null && finishingPath.GetComponent<Path>().FinishSign.transform.position.y < Player.transform.position.y)
        {
            mGameState = GameState.GameOver;

            UIManager.instance.CheckPointText.gameObject.SetActive(true);

            PlayerCar.instance.CarSpeed = 0.0f;

            foreach(GameObject _AICar in AICarManager.instance.PooledAICars)
            {
                if (_AICar.activeInHierarchy)
                {
                    _AICar.GetComponent<AICar>().ForwardMovementDone = false;
                }
            }

            Invoke("DelayBeforeGoingBackToMenu", 3.0f);
        }
    }

    void DelayBeforeGoingBackToMenu()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
