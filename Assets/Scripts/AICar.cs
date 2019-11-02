using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICar : MonoBehaviour
{
    public float AICarInitialSpeed;

    public float AICarBackwardSpeed;

    public float AICarSpeed;

    public bool ForwardMovementDone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Game_Manager.instance.mGameState == Game_Manager.GameState.GameOn || Game_Manager.instance.mGameState == Game_Manager.GameState.GameEnding
            || Game_Manager.instance.mGameState == Game_Manager.GameState.GameOver || Game_Manager.instance.mGameState == Game_Manager.GameState.GamePaused)
        {
            MoveCarInitially();
            MoveCarBackwards();
        }

        if(Game_Manager.instance.mGameState == Game_Manager.GameState.GameOn)
        {
            DetectCollisionOfPlayer();
        }
    }

    public void MoveCarInitially()
    {
        if (!ForwardMovementDone)
        {
            this.transform.Translate(Vector3.up * AICarInitialSpeed * Time.deltaTime, Space.World);
        }
    }

    public void MoveCarBackwards()
    {
        if (PlayerCar.instance.CarSpeed >= 40.0f)
        {
            this.transform.Translate(-Vector3.up * AICarBackwardSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            ForwardMovementDone = false;
        }
    }

    private void OnBecameInvisible()
    {
        ForwardMovementDone = true;
        this.gameObject.SetActive(false);
    }

    void DetectCollisionOfPlayer()
    {
        float playerDistanceFromAICar = Vector3.Distance(this.transform.position, Game_Manager.instance.Player.transform.position);
        Debug.DrawLine(Game_Manager.instance.Player.transform.position, this.transform.position);

        Debug.Log("Player's Distance from AI :" + playerDistanceFromAICar);

        if (playerDistanceFromAICar < 0.8f)
        {
            Game_Manager.instance.mGameState = Game_Manager.GameState.GamePaused;
            PlayerCar.instance.CarSpeed = 0.0f;
            ForwardMovementDone = false;
            Game_Manager.instance.Player.SetActive(false);
            GameObject exp = Instantiate(Game_Manager.instance.Explosion);
            exp.transform.position = Game_Manager.instance.Player.transform.position;
        }
    }
}
