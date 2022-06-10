    using UnityEngine;

[CreateAssetMenu(fileName = "AKN_Player", menuName = "AKN RPG Final/Character/AKN_Player", order = 0)]
public class AKN_PlayerSO : AKN_CharacterSO
{
    [Header("ATTRIBUTES")]
    public int availableAttributePoints;
    public int strength;        //Strength measures bodily power, athletic Training, and the extent to which you can exert raw physical force.
    public int dexterity;       //Dexterity measures agility, reflexes, and balance.
    public int constitution;    //Constitution measures health, stamina, and vital force.
    public int intelligence;    //Intelligence measures mental acuity, accuracy of recall, and the ability to reason.
    public int wisdom;          //Wisdom reflects how attuned you are to the world around you and represents perceptiveness and intuition.
    public int charisma;        //Charisma measures your ability to interact effectively with others. It includes such factors as confidence and eloquence, and it can represent a charming or commanding Personality.

}

//https://roll20.net/compendium/dnd5e/CategoryIndex%3ARules#content
//https://roll20.net/compendium/dnd5e/Ability%20Scores#toc_12