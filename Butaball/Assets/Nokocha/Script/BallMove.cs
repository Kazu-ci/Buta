using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BallMove : MonoBehaviour
{
    public float moveForce = 5f;            //ボールに与える力
    public float moveStopCheck = 0.5f;      //止まったか見る
    public float waitTime = 2f;             //止まってから動き出すまでの時間

    private Rigidbody rb;
    private bool MoveCheck = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (rb.velocity.magnitude < moveStopCheck && !MoveCheck)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    System.Collections.IEnumerator WaitAndMove()
    {
        MoveCheck = true;
        yield return new WaitForSeconds(waitTime);

        //ランダムな方向に力を加える
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        rb.AddForce(randomDir * moveForce, ForceMode.Impulse);

        MoveCheck = false;
    }
}
