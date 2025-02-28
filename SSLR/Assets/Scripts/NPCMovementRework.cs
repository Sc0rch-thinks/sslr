/*
 * Author: Lin Hengrui Ryan, Livinia Poo
 * Date: 3/2/25
 * Description:
 * Customer walking handling using NavMesh
 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NpcMovementRework : MonoBehaviour
{
    /// <summary>
    /// nav mesh agent
    /// </summary>
    public NavMeshAgent agent;

    /// <summary>
    /// Animation assigning
    /// </summary>
    public Animator animator;
    private static readonly int IsSitting = Animator.StringToHash("isSitting");
    private static readonly int Speed = Animator.StringToHash("Speed");

    /// <summary>
    /// NPC's dialogue variables
    /// </summary>
    public TextMeshProUGUI npcWelcomeText;
    public TextMeshProUGUI initialStatementText;
    public TextMeshProUGUI npcAnswerOneText;
    public TextMeshProUGUI npcAnswerTwoText;
    public TextMeshProUGUI npcAnswerThreeText;
    public TextMeshProUGUI npcClarifiedResponse;

    [SerializeField] private GameObject npcSpeechBubble;
    [SerializeField] private GameObject npcAnswerPanel;

    /// <summary>
    /// NPC Data referemce
    /// </summary>
    public NpcData npcData;
    
    /// <summary>
    /// Variable to store service
    /// </summary>
    public string correctService;

    /// <summary>
    /// Check if NPC should be sitting or standing
    /// </summary>
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

    /// <summary>
    /// Assigning references, NPC movement
    /// </summary>
    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        Backend.instance.FirebaseGet(this);

        npcSpeechBubble.SetActive(false);
        npcAnswerPanel.SetActive(false);
        StartCoroutine(SitDown());
    }

    /// <summary>
    /// Call coroutine to call client
    /// </summary>
    public void Called()
    {
        StartCoroutine(CustomerCalled());
    }

    /// <summary>
    /// Coroutine to get NPC to coe to table
    /// </summary>
    /// <returns></returns>
    public IEnumerator CustomerCalled()
    {
        var pos = NpcManager.instance.desk;
        agent.SetDestination(pos.position);

        while (true)
        {
            var npcpos = gameObject.transform.position;
            npcpos.y = 0;
            var dist = Vector3.Distance(pos.position, npcpos);

            if (dist < 0.5f)
            {
                agent.SetDestination(gameObject.transform.position);
                gameObject.transform.rotation = pos.transform.rotation;

                GameManager.instance.SetCurrentNPC(this.gameObject);

                npcSpeechBubble.SetActive(true);
                npcAnswerPanel.SetActive(true);
                PlayerDialogueInteraction.instance.playerDialogue.SetActive(true);

                yield return new WaitUntil(() => npcData != null);

                correctService = npcData.correctDepartment;

                LoadNPCDialogue();
                break;
            }

            yield return 0;
        }
    }


    /// <summary>
    /// If in front of seat, NPC sits
    /// </summary>
    /// <returns></returns>
    public IEnumerator SitDown()
    {
        var i = Random.Range(0, NpcManager.instance.Seats.Length);
        var seat = NpcManager.instance.Seats[i];
        while (!seat.Available)
        {
            i = Random.Range(0, NpcManager.instance.Seats.Length);
            seat = NpcManager.instance.Seats[i];
            yield return new WaitForSeconds(2f);
        }

        seat.Available = false;
        var sittingPosition = seat.SeatObject.transform.position;
        sittingPosition.y = 0;
        agent.SetDestination(sittingPosition);

        while (true)
        {
            var dist = Vector3.Distance(sittingPosition, gameObject.transform.position);
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
    

    /// <summary>
    /// Despawning NPCs
    /// </summary>
    /// <param name="endDay"></param>
    public void Despawn(bool endDay = false)
    {
        if (endDay)
        {
            var random = Random.Range(0, NpcManager.instance.spawnPoints.Length);
            StartCoroutine(DespawnWhenReached(NpcManager.instance.spawnPoints[random]));
        }
        else
        {
            var random = Random.Range(0, NpcManager.instance.despawnPoints.Length);
            StartCoroutine(DespawnWhenReached(NpcManager.instance.despawnPoints[random]));
        }
    }

    /// <summary>
    /// Only despawn under certain condition
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    private IEnumerator DespawnWhenReached(Transform destination)
    {
        agent.SetDestination(destination.position);
        while (true)
        {
            var dist = Vector3.Distance(destination.position, gameObject.transform.position);
            if (dist < 1)
            {
                agent.SetDestination(gameObject.transform.position);
                Destroy(gameObject);
                break;
            }

            yield return 0;
        }
    }


    /// <summary>
    /// Assign UI text based on NPC Data
    /// </summary>
    public void LoadNPCDialogue()
    {
        if (GameManager.instance.currentNPC != this.gameObject)
        {
            return;
        }

        if (npcData == null)
        {
            Debug.LogError($"NPC Data is null for {gameObject.name}");
            return;
        }

        npcWelcomeText.text = "Hello";
        initialStatementText.text = npcData.initialStatement;

        npcAnswerOneText.text = npcData.answer1;
        npcAnswerTwoText.text = npcData.answer2;
        npcAnswerThreeText.text = npcData.answer3;

        npcClarifiedResponse.text = "I see...";

        PlayerDialogueInteraction.instance.SetPlayerQuestions(npcData.question1, npcData.question2, npcData.question3);
        PlayerDialogueInteraction.instance.SetPlayerResposne(npcData.response3);
    }
}