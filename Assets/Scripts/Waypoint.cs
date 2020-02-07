using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Waypoint : MonoBehaviour
{

    [SerializeField] float minDelay = 0f;
    [SerializeField] float maxDelay = 2f;
    [SerializeField] int numberOFLoops; // 0 = ∞
    public enum walkingSpeeds { RUN, WALK }
    [SerializeField] walkingSpeeds walkSpeed;
    [SerializeField] GameObject nextPoint;

    public AITargetController.AIState playAnimation;
    Dictionary<walkingSpeeds, float> walkStates = new Dictionary<walkingSpeeds, float>(); // walking states / its speed
    [SerializeField] bool instantGo = true;
    void Start()
    {
        walkStates.Add(walkingSpeeds.WALK, 0.5f);
        walkStates.Add(walkingSpeeds.RUN, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        AITargetController NPC = other.GetComponent<AITargetController>();
        ThirdPersonCharacter NPCPar = NPC.GetComponent<ThirdPersonCharacter>();
        float rDelay = Random.Range(minDelay, maxDelay);
        int numberOfLoopsCopy = numberOFLoops;
        if (numberOfLoopsCopy == 0)
        {
            StartCoroutine(NPC.PlayAnimation(rDelay, playAnimation, nextPoint)); 
        }
        else
        {
            while (numberOfLoopsCopy > 0)
            {
                numberOfLoopsCopy--;
                if (NPC.isActive)
                {
                    NPC.isActive = false; 

                    NPCPar.m_MoveSpeedMultiplier = walkStates[walkSpeed];
                    NPCPar.m_AnimSpeedMultiplier = walkStates[walkSpeed];

                    StartCoroutine(NPC.PlayAnimation(rDelay, playAnimation, nextPoint));

                }
            }
            if (instantGo)
            {
                NewWaypoint(NPC, rDelay);
            }
            
        }
        


    }
    private void NewWaypoint(AITargetController NPC, float time)
    {
        StartCoroutine(NPC.NextPoint(time));
    }

}
