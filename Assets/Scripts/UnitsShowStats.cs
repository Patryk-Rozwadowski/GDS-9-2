using UnityEngine;

public class UnitsShowStats : MonoBehaviour {
    [Header("UnitStats")] [SerializeField] private GameObject canvasPresentingStats;

    private void Awake() {
        canvasPresentingStats = GetComponent<Canvas>().gameObject;
    }

    private void OnMouseOver() {
        canvasPresentingStats.SetActive(true);
    }
}