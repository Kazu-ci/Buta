using UnityEngine;

public class BoundReflect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("衝突！");
        Rigidbody otherRb = collision.rigidbody;
        if (otherRb == null) return;

        // 接触点から法線ベクトルを取得
        ContactPoint contact = collision.contacts[0];
        Vector3 inVelocity = otherRb.velocity;
        Vector3 normal = contact.normal;

        // 反射ベクトルを計算
        Vector3 reflected = Vector3.Reflect(inVelocity, normal);

        // 反射後の速度を適用（速度維持 or 減衰）
        otherRb.velocity = reflected.normalized * inVelocity.magnitude;

    }
}
