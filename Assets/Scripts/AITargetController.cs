using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AITargetController : MonoBehaviour
{
    public bool isActive = true; //available for animation
    public enum AIState { IDLE, WANDERING, LOOKAROUND, YELL, SAMBA };
    Dictionary<AIState, string> states = new Dictionary<AIState, string>(); // state / names of the animation it refers to

    public AICharacterControl characterController;
    private Animator anim;

    public GameObject WaypointToGo; // next waypoint


    public AIState state = AIState.WANDERING;
    private bool WaitingForWandering = false; //going for the wandering state
    // Start is called before the first frame update
    void Start()
    {
        states.Add(AIState.IDLE, "idle");
        states.Add(AIState.WANDERING, "wandering");
        states.Add(AIState.LOOKAROUND, "LookAround");
        states.Add(AIState.YELL, "Yelling");
        states.Add(AIState.SAMBA, "Samba");
        anim = this.GetComponent<Animator>();
        characterController = this.GetComponent<AICharacterControl>();
        if(WaypointToGo == null)
        {
            WaypointToGo = GameObject.FindGameObjectWithTag("spawner").GetComponent<SpawnerController>().FirstWayPoint;
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.WANDERING:

                characterController.SetTarget(WaypointToGo.transform);
                break;
            case AIState.IDLE:
                break;
            case AIState.LOOKAROUND:
                anim.SetBool("isLookingAround", true);
                break;
            case AIState.YELL:
                anim.SetBool("isYelling", true);
                break;
            case AIState.SAMBA:
                anim.SetBool("isSamba", true);
                break;
        }
        if (WaitingForWandering && !anim.GetCurrentAnimatorStateInfo(0).IsName(states[state])) 
        {
            //disable all animation states
            anim.SetBool("isSamba", false);
            anim.SetBool("isYelling", false);
            anim.SetBool("isLookingAround", false);
            isActive = true;
            state = AIState.WANDERING;
        }

    }

    public IEnumerator PlayAnimation(float dealy, AIState aniState, GameObject nextWaipoint) //playing chosen animation after certain delay
    {
        WaitingForWandering = false;
        state = AIState.IDLE;
        yield return new WaitForSeconds(dealy);
        state = aniState;
        WaypointToGo = nextWaipoint;

    }
    public IEnumerator NextPoint(float time) //setting the next point little after the animation starts
    {
        yield return new WaitForSeconds(time + 0.5f);
        WaitingForWandering = true;
        
    }
    void GoToNextPoint()
    {
        
        anim.SetBool("isSamba", false);
        anim.SetBool("isYelling", false);
        anim.SetBool("isLookingAround", false);
        state = AIState.WANDERING;
    }

}
