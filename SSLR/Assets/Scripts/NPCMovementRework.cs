/*
 * Author: Lin Hengrui Ryan
 * Date: 3/2/25
 * Description:
 * Customer walking handling using NavMesh
 */


using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovementRework : MonoBehaviour
{
    /// <summary>
    /// nav mesh agent
    /// </summary>
    public NavMeshAgent agent;

    public Animator animator;
    private static readonly int IsSitting = Animator.StringToHash("isSitting");
    private static readonly int Speed = Animator.StringToHash("Speed");

    NpcData npcData;
    
    public void Update()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            animator.SetFloat(Speed, 1);
            animator.SetBool(IsSitting, false);
        }
        else
        {
            animator.SetFloat(Speed, 0);
        }
    }

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(SitDown());
    }

    public void Called()
    {
        StartCoroutine(CustomerCalled());
    }
    
    public IEnumerator CustomerCalled()
    {
        var pos = NpcManager.instance.desk;
        agent.SetDestination(pos.position);
        while (true)
        {
            var npcpos = gameObject.transform.position;
            npcpos.y = 0;
            var dist= Vector3.Distance(pos.position,npcpos);
            if (dist<0.5f)
            {
                agent.SetDestination(gameObject.transform.position);
                gameObject.transform.rotation = pos.transform.rotation;
                break;
                
            }

            yield return 0;
        }
    }


    public IEnumerator SitDown()
    {
        var i = Random.Range(0, NpcManager.instance.Seats.Count);
        var seat = NpcManager.instance.Seats[i];
        while (!seat.Available)
        {
             i = Random.Range(0, NpcManager.instance.Seats.Count);
             seat = NpcManager.instance.Seats[i];
            
        }
        

        var sittingPosition = seat.SeatObject.transform.position;
        sittingPosition.y = 0;
        agent.SetDestination(sittingPosition);
        
        while (true)
        {
            var dist= Vector3.Distance(sittingPosition,gameObject.transform.position);
            // Debug.Log(dist);
            if (dist < 0.05f)
            {
                agent.SetDestination(gameObject.transform.position);
                animator.SetBool(IsSitting, true);
                gameObject.transform.rotation = seat.SeatObject.transform.rotation;
                break;
            }
            yield return 0;
        }
    }

    public void Despawn()
    {
        var random = Random.Range(0, NpcManager.instance.despawnPoints.Length);
        agent.SetDestination(NpcManager.instance.despawnPoints[random].position);
    }

    public void LoadNPCDialogue()
    {
        GameManager.instance.initialStatementText.text = npcData.initialStatement;
        
        GameManager.instance.playerQuestionOneText.text = npcData.question1;
        GameManager.instance.playerQuestionTwoText.text = npcData.question2;
        GameManager.instance.playerQuestionThreeText.text = npcData.question3;
        
        GameManager.instance.npcAnswerOneText.text = npcData.answer1;
        GameManager.instance.npcAnswerTwoText.text = npcData.answer2;
        GameManager.instance.npcAnswerThreeText.text = npcData.answer3;

        GameManager.instance.playerResponse.text = npcData.response3;
    }
}