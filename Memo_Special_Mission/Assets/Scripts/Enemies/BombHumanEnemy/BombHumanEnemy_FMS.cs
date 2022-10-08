using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombHumanEnemy_State
{
    Walk, ThrowBomb, Squat, StandUp, MeleeAttack, Idle
}

public class BombHumanEnemy_FMS : MonoBehaviour
{
    public BombHumanEnemy_PCGF bombHumanEnemy_PCGF;

    private IState currentState;
    private Dictionary<BombHumanEnemy_State, IState> bombHumanEnemy_StateDictionary = new Dictionary<BombHumanEnemy_State, IState>();

    void Start()
    {
        bombHumanEnemy_PCGF = gameObject.GetComponent<BombHumanEnemy_PCGF>();

        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.Walk, new BombHumanEnemy_Walk(this));
        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.ThrowBomb, new BombHumanEnemy_ThrowBomb(this));
        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.Squat, new BombHumanEnemy_Squat(this));
        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.StandUp, new BombHumanEnemy_StandUp(this));
        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.MeleeAttack, new BombHumanEnemy_MeleeAttack(this));
        bombHumanEnemy_StateDictionary.Add(BombHumanEnemy_State.Idle, new BombHumanEnemy_Idle(this));

        StateTransition(BombHumanEnemy_State.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void StateTransition(BombHumanEnemy_State state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = bombHumanEnemy_StateDictionary[state];
        currentState.OnEnter();
    }
}
