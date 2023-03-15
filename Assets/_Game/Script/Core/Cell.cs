using UnityEngine;

[SelectionBase]
public class Cell : MonoBehaviour
{
    public enum State
    {
        CELL,
        START,
        END,
        SEARCH,
        EXPAND,
        PATH
    }

    public SpriteRenderer main;
    public SpriteRenderer bg;
    public State state;
    [SerializeField] private Color[] bgColors;

    public void ChangeState(State newState, bool force = false)
    {
        if (!force && (state == State.START || state == State.END)) return;
        
        state = newState;
        bg.color = bgColors[(int) state];
    }

    [ButtonEditor]
    public void UpdateState() => ChangeState(state);
}
