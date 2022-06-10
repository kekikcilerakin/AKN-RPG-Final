using UnityEngine;

[CreateAssetMenu(fileName = "AKN_Choice", menuName = "AKN RPG Final/Dialogue/AKN_Choice", order = 1)]
public class AKN_Choice : ScriptableObject
{
    public string choiceTitle = "new choice";
    public bool finishButton;

    public AKN_Sentence choiceSentence;

    public bool isCombatButton;
    public AKN_EnemySO enemy;
}