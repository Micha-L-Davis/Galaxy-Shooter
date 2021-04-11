using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    public Slider _boostBar;

    
    void Start()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        //get the boost fuel max from player
        _boostBar = GetComponent<Slider>();
        _boostBar.maxValue = player._boostMax;
        _boostBar.value = player._boostMax;
        //set boostBar.maxValue to max boost
        //set boostBar.value max boost
    }

    public void UpdateBoostBar(float currentBoostFuel)
    {
        _boostBar.value = currentBoostFuel;
    }
}
