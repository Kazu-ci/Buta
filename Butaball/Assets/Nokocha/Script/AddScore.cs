using UnityEngine;

public class AddScore : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball1"))
        {
            ScoreManager.Player1Score += 1;
            Debug.Log("player1スコア" + ScoreManager.Player1Score);
        }
        else if (other.CompareTag("ball2"))
        {
            ScoreManager.Player2Score += 1;
            Debug.Log("player2スコア" + ScoreManager.Player2Score);
        }
    }
}
