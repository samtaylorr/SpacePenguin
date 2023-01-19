using UnityEngine;

public class Action : MonoBehaviour {
    public GameObject actionModule;
    public string _name;

    public Action(GameObject actionModule, string _name){ this.actionModule = actionModule; this._name = _name; }
}

// These actions are handled by an Animator attached to the WheelManager GameObject in scene.
public enum PlayerActions {
    PLAYER_JUMP,
    PLAYER_ATTACK,
    COMPANION_JUMP,
    COMPANION_ATTACK,
    DUO_ATTACK,
    ITEM,
    TACTICS
}