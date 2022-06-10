using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKN_Activity_Hunt : AKN_Activity
{
    public override void DoActivity()
    {
        Debug.Log("Hunt");
    }

    public override string SetActivityName()
    {
        return base.activityName = "Hunt";
    }
}
