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
        _boostBar = GetComponent<Slider>();
        _boostBar.maxValue = player.boostMax;
        _boostBar.value = player.boostMax;
    }

    public void UpdateBoostBar(float currentBoostFuel)
    {
        _boostBar.value = currentBoostFuel;
    }
}
