using Mono.Cecil;
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
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;
    [SerializeField] float drag;  // 抵抗（慣性調整）
    [SerializeField] float cDrag;  // 抵抗（慣性調整）
    float h,v;
    Rigidbody rb;
    InputAction move;
    InputAction charge;
    float bTime;
    float cTime;
    float chargePow;
    Vector3 moveDir;
    public LayerMask collisionMask;
    public Gamepad assignedGamepad;
    enum State
    {
        Idle,
        Move,
        Brake,
        Fire,
        Bound,
        Die,
    }
    State state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state=State.Idle;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;   // 慣性の減衰を設定
        rb.angularDamping = 0f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        move = action.actions["Move"];
        charge = action.actions["Charge"];
    }

    // Update is called once per frame
    private void Update()
    {
        Think();

    }
    private void FixedUpdate()
    {
        CollisionPredictionAndReflect();
        if (state != State.Bound)
        {
            Angle();
            Move();
        }
        Debug.Log(chargePow);
    }
    void Think()
    {
        if (state == State.Brake)
        {
            if (!charge.IsPressed())
            {
                rb.linearDamping = drag;
            }
            return;
        }
        switch (state)
        {
            case State.Idle:
                if (move.ReadValue<Vector2>()!=new Vector2(0,0) ){ state = State.Move; }
                if (charge.IsPressed()) { state = State.Brake; }
                break;
            case State.Move:
                if (charge.IsPressed()) { state = State.Brake; }
                if(move.ReadValue<Vector2>()==new Vector2(0,0)) { state = State.Idle; }
                break;
            case State.Brake:
                if (!charge.IsPressed()) { state = State.Bound; }
                break;
            case State.Bound:
                bTime += Time.deltaTime;
                if (bTime >= 0.5f) 
                {
                    if (rb.linearVelocity.magnitude < 0.1f)
                    {
                        state = State.Idle;  // 停止していればIdleへ
                    }
                    else
                    {
                        state = State.Move;  // 動いていればMoveへ戻す
                    }
                }
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
                if (move.activeControl?.device != assignedGamepad &&
            charge.activeControl?.device != assignedGamepad)
                {
                    Debug.Log("nun");
                    return;
                }
                OnMove();
                break;

            case State.Brake:
                if (move.activeControl?.device != assignedGamepad &&
            charge.activeControl?.device != assignedGamepad)
                {
                    return;
                }
                rb.linearVelocity *= cDrag;
                chargePow += Time.deltaTime*60;
                if(chargePow>=mChragePow) { chargePow=mChragePow; }
                if(!charge.IsPressed()) { state = State.Fire;}
                //if(fire.IsPressed()) { state=State.Fire; }
                break;
            case State.Fire:
                Vector3 velocity = rb.linearVelocity;
                Vector3 moveDirFromVelocity = velocity.normalized;
                rb.AddForce(moveDirFromVelocity * chargePow, ForceMode.VelocityChange);
                chargePow = 0;
                state = State.Bound;
                break;
            case State.Bound:
                Debug.Log("Boundだよ");
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
        if (moveDir.sqrMagnitude > 0.01f)
        {
            // 最大速度制限
            if (rb.linearVelocity.magnitude < maxSpeed)
            {
                rb.AddForce(moveDir * moveForce);
            }
        }
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

    }
  

    void CollisionPredictionAndReflect()
    {
        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < 0.01f) return;

        Vector3 direction = velocity.normalized;
        Ray ray = new Ray(transform.position, Vector3.zero);
        var sphereRadius = 0.7f;
        RaycastHit hit;
        var rayLength = 0.00000f;
        if (Physics.SphereCast(ray, sphereRadius, out hit, rayLength, collisionMask))
        {
            Vector3 hitNormal = hit.normal;
            Vector3 reflected = Vector3.Reflect(velocity, hitNormal);

            rb.linearVelocity = Vector3.zero; // 一度停止
            rb.AddForce(reflected.normalized * 25, ForceMode.VelocityChange);

            Debug.DrawRay(transform.position, direction * hit.distance, Color.red, 0.2f);
            Debug.DrawRay(hit.point, hitNormal, Color.yellow, 0.2f);

        }
        else
        {
            Debug.DrawRay(transform.position, direction * rayLength, Color.green, 0.1f);
        }
    }
  
}
