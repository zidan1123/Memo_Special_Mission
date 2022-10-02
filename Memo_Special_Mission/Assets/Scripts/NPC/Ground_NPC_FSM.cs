using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ground_NPC_State
{
    Unsave, Saved, Run, Gift, Exit
}

public class Ground_NPC_FSM : MonoBehaviour
{
    public Ground_NPC_PCGF ground_NPC_PCGF;

    private IState currentState;
    private Dictionary<Ground_NPC_State, IState> Ground_NPC_StateDictionary = new Dictionary<Ground_NPC_State, IState>();

    void Start()
    {
        ground_NPC_PCGF = gameObject.GetComponent<Ground_NPC_PCGF>();

        Ground_NPC_StateDictionary.Add(Ground_NPC_State.Unsave, new Ground_NPC_Unsave(this));
        Ground_NPC_StateDictionary.Add(Ground_NPC_State.Saved, new Ground_NPC_Saved(this));
        Ground_NPC_StateDictionary.Add(Ground_NPC_State.Run, new Ground_NPC_Run(this));
        Ground_NPC_StateDictionary.Add(Ground_NPC_State.Gift, new Ground_NPC_Gift(this));
        Ground_NPC_StateDictionary.Add(Ground_NPC_State.Exit, new Ground_NPC_Exit(this));

        StateTransition(Ground_NPC_State.Unsave);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void StateTransition(Ground_NPC_State state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = Ground_NPC_StateDictionary[state];
        currentState.OnEnter();
    }
}
