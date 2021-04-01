using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

[CreateAssetMenu(menuName = "Units/Creator")]
public class UnitStatsSO : ScriptableObject {
    [Header("Unit's type.")] [SerializeField]
    public UnitTypeEnum unitType;
    
    [Header("Unit's name")] [SerializeField]
    public string unitName;
    
    [Header("Maximum unit's health.")] [SerializeField]
    public int maxHealth;

    [Header("Unit's damage")] [SerializeField]
    public int damage;

    [Header("Unit's sprite.")] [SerializeField]
    public Sprite sprite;

    [Header("Selected unit's sprite")] [SerializeField]
    public Sprite SelectedSprite;

    [Header("Maximum movement range")] [SerializeField]
    public int movementRange;

    [Header("Maximum attack range")] [SerializeField]
    public int attackRange;
    
    [Header("Attack sound")] [SerializeField]
    public AudioClip attackSound;

    [Header("Takes damage sound")] [SerializeField]
    public AudioClip TakeDamageSound;

    [Header("Death sound")] [SerializeField]
    public AudioClip deathSound;

    [Header("Designer mode - unit sprite")] [SerializeField]
    public Sprite designerModeUnitSprite;

    [Header("Damage tag")] [SerializeField]
    public List<UnitTag> attackTags;
    
    [Header("Unit's ability")] [SerializeField]
    public AbilitiesEnum ability;
    
    [Header("Counter Damage")] [SerializeField]
    public int counterDamage;
    
    [Header("This unit counter:")] [SerializeField]
    public UnitTypeEnum counterType;
}

public enum AbilitiesEnum {None, Counter}
public enum UnitTypeEnum {Melee, Distance, None}
