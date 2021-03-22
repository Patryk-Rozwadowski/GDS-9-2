using UnityEngine;

[CreateAssetMenu(menuName = "Units/Creator")]
public class UnitStatsSO : ScriptableObject {
    [Header("Maximum unit's health.")] [SerializeField]
    public int maxHealth;

    [Header("Unit's damage")] [SerializeField]
    public int damage;

    [Header("Unit's sprite.")] [SerializeField]
    public Sprite sprite;

    [Header("Maximum movement range")] [SerializeField]
    public int movementRange;

    [Header("Maximum attack range")] [SerializeField]
    public int attackRange;
    
    [Header("Attack sound")] [SerializeField]
    public AudioClip attackSound;

    [Header("Death sound")] [SerializeField]
    public AudioClip deathSound;

    [Header("Designer mode - unit sprite")] [SerializeField]
    public Sprite designerModeUnitSprite;
}