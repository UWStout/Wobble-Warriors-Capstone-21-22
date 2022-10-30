using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarManager : MonoBehaviour
{
    [SerializeField] CircleHealthBar[] healthBars;

    public CircleHealthBar getHealthbar(int index)
    {
        return healthBars[index];
    }
}
