using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AKN_Sentence", menuName = "AKN RPG Final/Dialogue/AKN_Sentence", order = 0)]
public class AKN_Sentence : ScriptableObject
{
    [TextArea(5,100)]
    public string sentence = "new sentence";
    public AKN_Sentence nextSentence;
    public List<AKN_Choice> choices;


}