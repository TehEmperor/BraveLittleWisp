using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTreeIllumination : MonoBehaviour
{
   [SerializeField] ParticleSystem[] spiritsToLit;

   private void Start() 
   {
      GetSpirits();   
      LitSpirits(FindObjectOfType<WorldMapKeeper>().GetGatheredSpirits());
   }
   private void GetSpirits()
   {
      spiritsToLit = GetComponentsInChildren<ParticleSystem>();            
   }

   public void LitSpirits(int amount)
   {
      amount = amount * 2;
      if(amount > spiritsToLit.Length) return;
      for(int i = 0; i < amount; i++)
      {
         spiritsToLit[i].Play();
      }      
   }
}
