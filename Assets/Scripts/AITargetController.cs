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
    private Animator aAnim;

    [SerializeField] GameObject WaypointToGo; // next waypoint


    public AIState state = AIState.WANDERING;
    private bool mWaitingForWandering = false; //going for the wandering state
    // Start is called before the first frame update
    void Start()
    {
        //adding to dictionary
        states.Add(AIState.IDLE, "idle");
        states.Add(AIState.WANDERING, "wandering");
        states.Add(AIState.LOOKAROUND, "LookAround");
        states.Add(AIState.YELL, "Yelling");
        states.Add(AIState.SAMBA, "Samba");
        aAnim = this.GetComponent<Animator>();
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
                aAnim.SetBool("isLookingAround", true);
                break;
            case AIState.YELL:
                aAnim.SetBool("isYelling", true);
                break;
            case AIState.SAMBA:
                aAnim.SetBool("isSamba", true);
                break;
        }
        if (mWaitingForWandering && !aAnim.GetCurrentAnimatorStateInfo(0).IsName(states[state])) 
        {
            //disable all animation states
            aAnim.SetBool("isSamba", false);
            aAnim.SetBool("isYelling", false);
            aAnim.SetBool("isLookingAround", false);
            isActive = true;
            state = AIState.WANDERING;
        }

    }

    public IEnumerator PlayAnimation(float dealy, AIState aniState, GameObject nextWaipoint) //playing chosen animation after certain delay
    {
        mWaitingForWandering = false;
        state = AIState.IDLE;
        yield return new WaitForSeconds(dealy);
        state = aniState;
        WaypointToGo = nextWaipoint;

    }
    public IEnumerator NextPoint(float time) //setting the next point little after the animation starts
    {
        yield return new WaitForSeconds(time + 0.5f);
        mWaitingForWandering = true;
        
    }
    void GoToNextPoint()
    {
        
        aAnim.SetBool("isSamba", false);
        aAnim.SetBool("isYelling", false);
        aAnim.SetBool("isLookingAround", false);
        state = AIState.WANDERING;
    }

}
