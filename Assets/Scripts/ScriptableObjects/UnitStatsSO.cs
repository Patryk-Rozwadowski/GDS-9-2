using UnityEngine;

[CreateAssetMenu(menuName = "Units/Creator")]
public class UnitStatsSO : ScriptableObject {
    [Header("Maximum unit's health.")] [SerializeField]
    private int maxHealth;

    [Header("Unit's damage")] [SerializeField]
    private int damage;

    [Header("Unit's sprite.")] [SerializeField]
    private Sprite sprite;

    [Header("Maximum movement range")] [SerializeField]
    private int movementRange;

    [Header("Maximum attack range")] [SerializeField]
    private int attackRange;
}