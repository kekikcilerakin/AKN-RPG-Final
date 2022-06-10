using UnityEngine;

public enum CharacterRace {None, Human, Elf, Dwarf, Animal, Giant};
public enum CharacterClass {None, Rogue, Ranger, Fighter, Sorcerer};
public enum AttackType {Melee, Ranged}

public class AKN_CharacterSO : ScriptableObject
{
    public new string name; // Character's name
    public CharacterRace race; // Character's race
    public CharacterClass characterClass; // Character's class
    public AttackType attackType; // Character's attack type

    [Header("COMBAT")]
    public float maximumHealth; // Character's maximum health in combat
    public float currentHealth; // Character's current health in combat
    
    public float maximumArmor; // Character's maximum armor in combat
    public float currentArmor; // Character's current armor in combat

    public float damage; // Character's damage in combat

    public Sprite icon; // Character's icon in combat

    public int moveRadius = 1; // Character's move radius in combat as in hex tiles
    public int attackRadius = 1; // Character's attack radius in combat as in hex tiles

    private void OnEnable()
    {
        currentHealth = maximumHealth;
        currentArmor = maximumArmor;

        attackRadius = (attackType == AttackType.Ranged) ? 2 : 1; // If character is a ranged unit set it's attack radius to 2. Otherwise set it to 1
    }
    
}