using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject[] playerPrefab;
    private GameObject[] gameObjects;

    private void Awake()
    {
        if (Gamepad.all.Count < 2)
        {
            Debug.LogError("コントローラー2つが接続されていません。");
            return;
        }
        gameObjects = new GameObject[Gamepad.all.Count];
        int posX = -3;
        // プレイヤー生成
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            gameObjects[i] = PlayerInput.Instantiate(
            playerPrefab[i],
            playerIndex: i,
            controlScheme: "Gamepad",
            pairWithDevice: Gamepad.all[i]
        ).gameObject;
            gameObjects[i].GetComponent<Transform>().transform.position = new Vector3(posX, 0.5f, 0);
            Debug.Log(gameObjects[i].transform.position);
            posX = 3;
        }
    }

    private void Start()
    {
        //if (Gamepad.all.Count < 2)
        //{
        //    Debug.LogError("コントローラー2つが接続されていません。");
        //    return;
        //}
        //int posX = -3;
        //// プレイヤー生成
        //for (int i = 0; i < Gamepad.all.Count; i++)
        //{
        //    GameObject pi = PlayerInput.Instantiate(
        //    playerPrefab[i],
        //    playerIndex: i,
        //    controlScheme: "Gamepad",
        //    pairWithDevice: Gamepad.all[i]
        //).gameObject;
        //    pi.transform.position = new Vector3(posX, 0.5f, 0);
        //    Debug.Log(pi.transform.position);
        //    posX = 3;
        //}





    }
}
