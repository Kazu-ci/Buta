using UnityEngine;

public class ChargeUIManager : MonoBehaviour
{
    [SerializeField] private Player player; // シーンに置かれたプレイヤーやPrefabを直接アサイン
    [SerializeField] private UnityEngine.UI.Image chargeBarFill;
    [SerializeField] private float maxCharge = 15f;

    void Update()
    {
        if (player == null || chargeBarFill == null) return;
        float ratio = Mathf.Clamp01(player.GetChargePow() / maxCharge);
        chargeBarFill.fillAmount = ratio;
    }

    // プレイヤーを直接セットするメソッド
    public void AssignPlayer(Player p)
    {
        player = p;
    }
}
