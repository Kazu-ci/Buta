using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject[] playerPrefab;
    private int cnt=0;
    private void Awake()
    {
        /*if (Gamepad.all.Count < 2)
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
        }*/
    }

    private void Update()
    {
        PlayerJoin();
    }
    public void PlayerJoin()
    {
        foreach (var gamepad in Gamepad.all)
        {
            // まだユーザーにペアリングされていないデバイスに対して
            if (InputUser.FindUserPairedToDevice(gamepad) == null)
            {
                if (gamepad.buttonSouth.wasPressedThisFrame) //Aボタン
                {
                    if (cnt < playerPrefab.Length)
                    {
                        // プレイヤーを生成してペアリング
                        var player = PlayerInput.Instantiate(playerPrefab[cnt],
                        playerIndex: cnt,
                        controlScheme: "Gamepad",
                        pairWithDevice:gamepad);
                        Debug.Log("Player joined with device: " + gamepad.deviceId);
                        cnt++;
                    }  
                }
            }
        }
    }
}
