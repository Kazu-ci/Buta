using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Users;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject[] playerPrefab;
    //UI関連：倉岡追加
    [SerializeField] private GameObject chargeUIPrefab;
    [SerializeField] private RectTransform uiParent;
    private int playerIndex = 0;

    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            // すでにどこかのプレイヤーに使われていればスキップ
            bool alreadyUsed = PlayerInput.all.Any(p => p.user.pairedDevices.Contains(gamepad));
            if (alreadyUsed) continue;

            // Aボタンが押されたら参加
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                JoinPlayer(gamepad);
            }
        }
    }

    void JoinPlayer(Gamepad gamepad)
    {
        var playerInput = PlayerInput.Instantiate(
        playerPrefab[playerIndex],
        controlScheme: "Gamepad",  // Control Scheme 名はInputActionAssetに応じて
        pairWithDevice: gamepad,
        splitScreenIndex: playerIndex++
    );
        Debug.Log($"Player {playerIndex} joined with {gamepad.displayName}");
        var uiInstance = Instantiate(chargeUIPrefab, uiParent);
        var uiManager = uiInstance.GetComponent<ChargeUIManager>();
        uiManager.AssignPlayer(playerInput.GetComponent<Player>());

        // UI位置設定
        RectTransform rect = uiInstance.GetComponent<RectTransform>();
        SetUIPosition(rect, playerIndex);

        //パーティクルシステム関連
        var attractor = FindObjectOfType<ParticleGnerater>();
        if (attractor != null)
        {
            attractor.SetTarget(playerInput.transform);
        }

    }
    //-------------------------------------------------------------------
    //UI関連：倉岡追加箇所
    private void SetUIPosition(RectTransform rect, int index)
    {
        rect.anchorMin = rect.anchorMax = GetAnchorPosition(index);
        rect.pivot = rect.anchorMin;
        rect.anchoredPosition = GetOffsetPosition(index);
    }

    private Vector2 GetAnchorPosition(int index)
    {
        switch (index)
        {
            case 0: return new Vector2(0f, 1f);   // 左上
            case 1: return new Vector2(1f, 1f);   // 右上
            case 2: return new Vector2(0f, 0f);   // 左下
            case 3: return new Vector2(1f, 0f);   // 右下
            default: return new Vector2(0.5f, 0.5f);
        }
    }

    private Vector2 GetOffsetPosition(int index)
    {
        // 左上を基準：X=760, Y=450
        const float xBase = 760f;
        const float yBase = 450f;

        switch (index)
        {
            case 0: return new Vector2(xBase, -yBase);   // 左上
            case 1: return new Vector2(-xBase, -yBase);  // 右上
            case 2: return new Vector2(xBase, yBase);    // 左下
            case 3: return new Vector2(-xBase, yBase);   // 右下
            default: return Vector2.zero;
        }
    }
    //---------------------------------------------------------------
}
