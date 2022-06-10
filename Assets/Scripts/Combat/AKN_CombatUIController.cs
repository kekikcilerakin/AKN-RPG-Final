using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AKN_CombatUIController : MonoBehaviour
{
    [Header("COMBAT")]
    public RectTransform enemyPanel;
    public Image friendlyUnitIcon, enemyUnitIcon;
    public Slider friendlyUnitArmorBar, friendlyUnitHealthBar, enemyUnitArmorBar, enemyUnitHealthBar;

    public Sprite unitDeadIcon;

    public void SetUnitPanel(AKN_CharacterSO _character)
    {
        if (_character is AKN_PlayerSO || _character is AKN_TeammateSO)
        {
            friendlyUnitIcon.sprite = _character.icon;

            friendlyUnitArmorBar.maxValue = _character.maximumArmor;
            friendlyUnitArmorBar.value = _character.currentArmor;

            friendlyUnitHealthBar.maxValue = _character.maximumHealth;
            friendlyUnitHealthBar.value = _character.currentHealth;

        }
        else if (_character is AKN_EnemySO)
        {
            enemyPanel.gameObject.SetActive(true);

            enemyUnitIcon.sprite = _character.icon;

            enemyUnitArmorBar.maxValue = _character.maximumArmor;
            enemyUnitArmorBar.value = _character.currentArmor;

            enemyUnitHealthBar.maxValue = _character.maximumHealth;
            enemyUnitHealthBar.value = _character.currentHealth;
        }
    }
}
