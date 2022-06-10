using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class AKN_Activity : MonoBehaviour
{
    public string activityName;

    public virtual void DoActivity()
    {

    }

    public virtual string SetActivityName()
    {
        return activityName;
    }
}
