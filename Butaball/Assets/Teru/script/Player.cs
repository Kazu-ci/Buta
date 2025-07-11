﻿using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput action;
    [SerializeField] float rotateSpeed;
    [SerializeField] float mChragePow;
    [SerializeField] float speed;  // 移動速度
    float h,v;
    Rigidbody rb;
    InputAction move;
    InputAction charge;
    float bTime;
    float cTime;
    float chargePow;
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
    void Update()
    {
        var inputAxis=move.ReadValue<Vector2>();
        h = inputAxis.x;
        v = inputAxis.y;   // W,S（-1〜1）

        //カメラの正面を取得
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //カメラの右側を取得
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        //移動方向を格納
        Vector3 moveDir = camForward * v + camRight * h;
        moveDir.Normalize();
        if (moveDir != Vector3.zero)
        {
            //進行方向に体を回転
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
        Think();
        Move();
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
                if(move.ReadValue<float>() == 0) { state=State.Bound; }
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
                rb.AddForce(transform.forward * speed);
                break;
            case State.Charge:
                cTime += Time.deltaTime;
                chargePow = cTime / 5;
                if(chargePow >= 1)
                {
                    chargePow = 1;
                }
                if (!charge.IsPressed()) { rb.AddForce(transform.forward * mChragePow); }
                break;
            case State.Bound:
                bTime += Time.deltaTime;
                break;
            case State.Die:
                break;
        }
    }
}
