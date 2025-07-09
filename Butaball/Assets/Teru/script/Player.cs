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
    Vector3 front;
    float speed;
    public LayerMask collisionMask;
    enum State
    {
        Idle,
        Move,
        Charge,
        Inertia,
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
    void Update()
    {
        Angle();
        Think();
        Move();
        Debug.Log(state);
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
                if(move.ReadValue<Vector2>()==new Vector2(0,0)) { state = State.Inertia; }
                break;
            case State.Charge:
                if (!charge.IsPressed()) { state = State.Bound; }
                break;
                case State.Inertia:
                if (speed == 0) {state=State.Idle; }
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
                if(chargePow >= 1)
                {
                    chargePow = 1;
                }
                if (!charge.IsPressed())
                {
                    rb.AddForce(moveDir * mChragePow); 
                }
                break;
             case State.Inertia:
                float drag = 0.01f;
                speed *= (1 - drag);
                if(speed < 0.05) { speed = 0; }
                rb.linearVelocity += moveDir  * Time.deltaTime;
                break;
            case State.Bound:
                bTime += Time.deltaTime;
                break;
            case State.Die:
                break;
        }
    }
    public void OnMove()
    {
        speed += accelaration*Time.deltaTime;
        if (speed >= mSpeed) { speed= mSpeed; }
        rb.linearVelocity += speed * moveDir * Time.deltaTime;
    }
    void Bound()
    {
        /*SphereCollider sphereCollider = GetComponent<SphereCollider>();
        float radius = sphereCollider.radius * transform.localScale.x; // スケール込み
        Vector3 rayOrigin = transform.position + moveDir.normalized * radius;*/
        //Debug.DrawRay(transform.position, moveDir.normalized * , Color.red);
        if (Physics.SphereCast(transform.position, 0.5f,transform.position, out RaycastHit hit, 0.1f, collisionMask))
        {
            moveDir = Vector3.Reflect(moveDir.normalized, hit.normal).normalized;
            //state = State.Inertia;
            Debug.Log("oppai");
            transform.position = hit.point - moveDir.normalized * 0.05f;
        }
    }
    void Angle()
    {
        var inputAxis = action.actions["Move"].ReadValue<Vector2>();
        h = inputAxis.x;
        v = inputAxis.y;   // W,S（-1〜1）
        //カメラの正面を取得
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラの右側を取得
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        //移動方向を格納
        moveDir = camForward * v + camRight * h;
        moveDir.Normalize();

        if (moveDir != Vector3.zero)
        {
            //進行方向に体を回転
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 bound = Vector3.Reflect(moveDir.normalized, collision.contacts[0].normal).normalized;
        rb.AddForce(bound * speed*20);
        moveDir = bound;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
