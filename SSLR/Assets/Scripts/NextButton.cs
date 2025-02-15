using UnityEngine;

public class NextButton : MonoBehaviour
{
   public void CallNext()
   {
      var npcs = NpcManager.instance.currentNpcs;
      var randomnpc = npcs[Random.Range(0, npcs.Count)];
      randomnpc.GetComponent<NpcMovementRework>().Called();
   }
}
