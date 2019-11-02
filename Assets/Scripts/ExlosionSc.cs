using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlosionSc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExplosinoFinishEvent()
    {
        Destroy(this.gameObject, 0.2f);
    }

    private void OnDestroy()
    {
        Game_Manager.instance.Player.SetActive(true);
        PlayerCar.instance.StartCoroutine(PlayerCar.instance.StartBlinkingCarAfterPause());
    }
}
