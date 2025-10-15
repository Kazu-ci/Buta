using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static int Player1Score;
    public static int Player2Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player1Score = 0;
        Player2Score = 0;
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball1"))
        {
            Player1Score += 1;
            Debug.Log("player1スコア" + Player1Score);
        }
        else  if(other.CompareTag("ball2"))
        {
            Player2Score += 1;
            Debug.Log("player2スコア" + Player2Score);
        }
    }
=======
=======
>>>>>>> Stashed changes
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update()
    {
        
    }
}
