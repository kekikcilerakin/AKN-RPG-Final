using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AKN_Activity_GatherWood : AKN_Activity
{
    public override void DoActivity()
    {
        Debug.Log("Gather Wood");
    }

    public override string SetActivityName()
    {
        return base.activityName = "Gather Wood";
    }
}
