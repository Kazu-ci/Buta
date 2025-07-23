using UnityEngine;
using UnityEngine.UI;

public class ChargeUIManager : MonoBehaviour
{
  [SerializeField]  Player player;
  [SerializeField]  Image chargebar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {


        chargebar.fillAmount = player.chargePow;
    }
}
