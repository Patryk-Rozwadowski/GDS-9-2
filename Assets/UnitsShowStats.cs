using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsShowStats : MonoBehaviour
{
    [Header("UnitStats")] [SerializeField] GameObject canvasPresentingStats;

    private void Awake()
    {
        canvasPresentingStats = GetComponent<Canvas>().gameObject;
    }

    private void OnMouseOver()
    {
        canvasPresentingStats.SetActive(true);
    }

}
