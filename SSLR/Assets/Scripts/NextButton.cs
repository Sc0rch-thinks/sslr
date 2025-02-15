using UnityEngine;

public class NextButton : MonoBehaviour
{
   /// <summary>
   /// Next NPC to desk based on scene list
   /// </summary>
   public void CallNext()
   {
      var npcs = NpcManager.instance.currentNpcs;
      var randomnpc = npcs[Random.Range(0, npcs.Count)];
      
      GameManager.instance.SetCurrentNPC(randomnpc);
      randomnpc.GetComponent<NpcMovementRework>().Called();
   }
}
