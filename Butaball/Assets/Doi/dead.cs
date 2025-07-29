using UnityEngine;
using UnityEngine.SceneManagement;
public class dead : MonoBehaviour
{
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="player1")
        {
                player1.alive1 = false;
        }
        if (other.gameObject.tag == "player2")
        {
            player1.alive1 = true;
            SceneManager.LoadScene("Result");
        }
        if (player1.alive1==false)
        {
            SceneManager.LoadScene("Result");
        }
    }
}
