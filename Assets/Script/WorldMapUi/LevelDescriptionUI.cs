using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelDescriptionUI : MonoBehaviour
{
   public void Setup(Level level)
   {
    GetComponentInChildren<TextMeshProUGUI>().text = level.GetLevelDescription();

   }
}
