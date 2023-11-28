using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescriptionSpawner : TooltipSpawner
{
    private Level myLevel;
    public void SetLevel(Level level)
    {
        myLevel = level;
    }

    public override bool CanCreateTooltip()
    {
        return true;
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
        tooltip.GetComponent<LevelDescriptionUI>().Setup(myLevel);
    }

  
}
