using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AKN_DialogueController : MonoBehaviour
{
    public AKN_Sentence currentSentence;
    private int choiceCount;
    public List<GameObject> choiceList;

    [Header("DRAG from ASSETS")]
    [SerializeField] private GameObject choicePrefab;

    private void Start()
    {       
        if (choicePrefab == null)
            AKN_GameController.instance.AKN_LOGERROR("Choice Prefab in " + gameObject.name + " is NULL", true);

        AKN_UpdateDialoguePanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AKN_UpdateDialoguePanel();
        }
    }

    private void AKN_UpdateDialoguePanel()
    {
        AKN_GameController.instance.UIController.dialoguePanel.localScale = Vector3.zero;

        for (int i = 0; i < choiceList.Count; i++)  //delete previous choice buttons and clear the list
        {
            Destroy(choiceList[i]);
        }
        if (choiceList.Count > 0)
        {
            choiceList.Clear();
        }

        if (currentSentence != null)
        {
            AKN_GameController.instance.playerController.isTraveling = false;
            AKN_GameController.instance.UIController.dialoguePanel.localScale = Vector3.one;
            choiceCount = currentSentence.choices.Count;
            AKN_GameController.instance.UIController.sentenceText.text = currentSentence.sentence;

            if (choiceCount == 0 && currentSentence.nextSentence == null)   //eğer choice ve next sentence yoksa dialog bitiren continue butonu oluştur
            {
                //Debug.Log("No Choice and Next Sentence found. Creating Continue Button.");
                GameObject choiceButton = Instantiate(choicePrefab, transform.position, Quaternion.identity, AKN_GameController.instance.UIController.choicePanel);
                choiceList.Add(choiceButton);
                choiceButton.name = "[CONTINUE]";
                choiceCount++;
                choiceButton.transform.GetChild(0).GetComponent<Text>().text = "[CONTINUE]";
                choiceButton.GetComponent<Button>().onClick.AddListener(AKN_ContinueButtonClicked);
            }
            else if (choiceCount == 0 && currentSentence.nextSentence != null)  //eğer choice yoksa ama next sentence varsa sonraki cümleye geçen next butonu oluştur
            {
                //Debug.Log("No Choice, but Next Sentence found. Creating Next Button.");
                GameObject choiceButton = Instantiate(choicePrefab, transform.position, Quaternion.identity, AKN_GameController.instance.UIController.choicePanel);
                choiceList.Add(choiceButton);
                choiceButton.name = "[NEXT]";
                choiceCount++;
                choiceButton.transform.GetChild(0).GetComponent<Text>().text = "[NEXT]";
                choiceButton.GetComponent<Button>().onClick.AddListener(AKN_NextButtonClicked);
            }
            else if (choiceCount > 0)   //eğer choice varsa choice butonları oluştur
            {
                //Debug.Log(choiceCount + " Choices found. Creating Choice Buttons.");
                for (int i = 0; i < choiceCount; i++)
                {
                    int choiceIndex = i;
                    GameObject choiceButton = Instantiate(choicePrefab, transform.position, Quaternion.identity, AKN_GameController.instance.UIController.choicePanel);
                    choiceList.Add(choiceButton);
                    choiceButton.name = "[CHOICE " + i + " ]";
                    choiceButton.transform.GetChild(0).GetComponent<Text>().text = currentSentence.choices[i].choiceTitle;
                    choiceButton.GetComponent<Button>().onClick.AddListener(() => AKN_ChoiceButtonClicked(choiceIndex));
                }
            }
        }
    }

    private void AKN_ContinueButtonClicked()
    {
        //Debug.Log("Continue Button clicked.");
        currentSentence = null;
        AKN_GameController.instance.UIController.dialoguePanel.localScale = Vector3.zero;
        //menuManager.menuPanel.localScale = Vector3.one;
        AKN_UpdateDialoguePanel();
    }

    private void AKN_NextButtonClicked()
    {
        //Debug.Log("Next Button clicked.");
        currentSentence = currentSentence.nextSentence;
        AKN_UpdateDialoguePanel();
    }

    private void AKN_ChoiceButtonClicked(int i)
    {
        //Debug.Log("Choice Button clicked.");
        if (currentSentence.choices[i].choiceSentence == null)
        {
            if (currentSentence.choices[i].isCombatButton)
            {
                Debug.Log("Combat starting");
                //combatManager.StartCombat(currentSentence.choices[i].enemy);
            }
            else
            {
                //Debug.Log("Next sentence in choice is not found. Finishing the dialogue.");
                currentSentence.choices[i].finishButton = true;
            }

        }
        else
        {
            currentSentence.choices[i].finishButton = false;
        }

        if (currentSentence.choices[i].finishButton)
        {
            //Debug.Log("Dialogue finished.");
            currentSentence = null;
            AKN_GameController.instance.UIController.dialoguePanel.localScale = Vector3.zero;
            //menuManager.menuPanel.localScale = Vector3.one;
        }
        else
        {
            //Debug.Log("A choice selected.");
            currentSentence = currentSentence.choices[i].choiceSentence;
        }
        AKN_UpdateDialoguePanel();
    }

}
