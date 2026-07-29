using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    public float recoverAmount = 30f; // 電池1つで回復する量
    public AudioClip getSE;           // 拾った時の効果音

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ★ここを GetComponent から GetComponentInParent に変更！
        // （触れたパーツだけでなく、親である Player まで遡って探す）
        FlashlightController controller = other.GetComponentInParent<FlashlightController>();

        if (controller != null)
        {
            controller.RecoverBattery(recoverAmount);

            if (getSE != null)
            {
                AudioSource.PlayClipAtPoint(getSE, Camera.main.transform.position);
            }

            Destroy(gameObject);
        }
    }
}