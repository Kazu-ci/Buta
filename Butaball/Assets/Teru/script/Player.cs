using UnityEngine;

public class Player : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Charge,
        Die,
    }
    State state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state=State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Think()
    {
        switch (state)
        {
            case State.Idle:

                break;
            case State.Move:
                break;
            case State.Charge:
                break;
            case State.Die:
                break;
        }
    }
}
