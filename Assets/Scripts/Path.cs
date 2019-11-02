using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public int order;

    public GameObject StartSign;
    public GameObject FinishSign;

    private void FixedUpdate()
    {
        MovePath();
    }

    void MovePath()
    {
        if(Game_Manager.instance.mGameState == Game_Manager.GameState.GameOn || Game_Manager.instance.mGameState == Game_Manager.GameState.GameEnding)
        {
            this.transform.Translate(-Vector3.up * Time.deltaTime * PlayerCar.instance.CarSpeed, Space.World);

            if (this.transform.position.y < 0 && this.transform.position.y > -1 && this.order == 2)
            {
                PathManagerScript.instance.GenerateNextPath(this.transform.position);
            }
            else if (this.transform.position.y < -20.86f && this.order == 3)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
