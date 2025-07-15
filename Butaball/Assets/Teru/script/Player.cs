using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float accelaration;
    [SerializeField] PlayerInput action;
    [SerializeField] float rotateSpeed;
    [SerializeField] float mChragePow;
    [SerializeField] float mSpeed;// 移動速度
    float h,v;
    Rigidbody rb;
    InputAction move;
    InputAction charge;
    float bTime;
    float cTime;
    float chargePow;
    Vector3 moveDir;
    float speed;
    public LayerMask collisionMask;
    enum State
    {
        Idle,
        Move,
        Charge,
        Bound,
        Die,
    }
    State state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state=State.Idle;
        rb=GetComponent<Rigidbody>();
        move = action.actions["Move"];
        charge = action.actions["Charge"];
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Think();
        if (state != State.Bound)
        {
            Angle();
            Move();
        }
        else
        {
            Debug.Log("ooo");
        }

    }
    void Think()
    {
        switch (state)
        {
            case State.Idle:
                if (move.ReadValue<Vector2>()!=new Vector2(0,0) ){ state = State.Move; }
                if (charge.IsPressed()) { state = State.Charge; }
                break;
            case State.Move:
                if (charge.IsPressed()) { state = State.Charge; }
                if(move.ReadValue<Vector2>()==new Vector2(0,0)) { state = State.Idle;speed = 0; }
                break;
            case State.Charge:
                if (!charge.IsPressed()) { state = State.Bound; }
                break;
            case State.Bound:
                if (bTime >= 1f) { state = State.Idle; bTime = 0; }
                break;
            case State.Die:
                break;
        }
    }
    private void Move()
    {
        switch (state)
        {
            case State.Move:
                OnMove();
                break;

            case State.Charge:
                cTime += Time.deltaTime;
                chargePow = cTime / 5;
                if (chargePow >= 1)
                {
                    chargePow = 1;
                }
                if (!charge.IsPressed())
                {
                    rb.AddForce(moveDir * mChragePow);
                }
                break;


            case State.Bound:
                bTime += Time.deltaTime;
                // 反射中は速度を直接操作しない（物理演算に任せる）
                break;

            case State.Die:
                // 動かないようにゼロ代入など
                rb.linearVelocity = Vector3.zero;
                break;
        }
    }
    public void OnMove()
    {
        speed += accelaration * Time.fixedDeltaTime;
        if (speed > mSpeed) speed = mSpeed;
        rb.linearVelocity = moveDir * speed;
    }
    void Angle()
    {
        var inputAxis = action.actions["Move"].ReadValue<Vector2>();
        h = inputAxis.x;
        v = inputAxis.y;   
        //カメラの正面を取得
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラの右側を取得
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        //移動方向を格納
        moveDir = camForward * v + camRight * h;
        moveDir.Normalize();

        if (moveDir != Vector3.zero)
        {
            
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Boundだよ");
        Rigidbody otherRb = collision.rigidbody;
            if (otherRb == null) return;
            ContactPoint contact = collision.contacts[0];
            Vector3 reflected = Vector3.Reflect(otherRb.linearVelocity, contact.normal);
            otherRb.linearVelocity = reflected;

            // プレイヤーを反射状態にする
            state = State.Bound;
            bTime = 0;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
