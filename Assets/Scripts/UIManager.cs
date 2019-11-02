using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject HUDPanel;
    public GameObject MenuPanel;

    public Text SpeedText;
    public Text DistanceText;
    public Text CheckPointText;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game_Manager.instance.mGameState == Game_Manager.GameState.GameOn || Game_Manager.instance.mGameState == Game_Manager.GameState.GameEnding
            || Game_Manager.instance.mGameState == Game_Manager.GameState.GamePaused)
        {
            ShowCarSpeedDistance();
        }
    }

    void ShowCarSpeedDistance()
    {
        SpeedText.text = "Speed: " + Mathf.Clamp((int)(PlayerCar.instance.CarSpeed * 10), 0, 400)+ "km/h";
        DistanceText.text = "Distance: " + (float)System.Math.Round((PlayerCar.instance.DistanceCovered / 1000), 1) + "km";
    }

    public void PlayButtonAction()
    {
        Game_Manager.instance.mGameState = Game_Manager.GameState.GameStarting;
        HUDPanel.SetActive(true);
        MenuPanel.SetActive(false);
        Game_Manager.instance.Player.SetActive(true);
        PathManagerScript.instance.ActivatePaths();
        AICarManager.instance.SetAICarsInitially();
    }
}
