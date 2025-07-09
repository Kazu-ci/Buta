using UnityEngine;
using UnityEngine.UI;
public class ChargeUI : MonoBehaviour
{
    [SerializeField] Image Chargebar;
    Player pl;
    GameObject player;
    float chargetime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        pl = player.GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
