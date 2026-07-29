using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2Dを使うために必要
using TMPro; // TextMeshProを使うために必要

public class FlashlightController : MonoBehaviour
{
    public Light2D flashlight;        // プレイヤーの2Dライト
    public TMP_Text batteryText;      // UIの電池テキスト

    public float maxBattery = 100f;   // 最大バッテリー量
    public float currentBattery;      // 現在のバッテリー量
    public float batteryDrainRate = 5f; // 1秒間に減るバッテリー量

    public float maxLightRadius = 5f;  // バッテリー100%の時のライトの範囲
    public float minLightRadius = 1f;  // バッテリー0%の時の最小ライト範囲

    void Start()
    {
        currentBattery = maxBattery;
    }

    void Update()
    {
        // 1. 時間経過でバッテリーを減らす
        if (currentBattery > 0)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            currentBattery = Mathf.Max(currentBattery, 0f); // 0未満にならないようにする
        }

        // 2. バッテリーの残量に応じてライトの範囲（Outer Radius）を変化させる
        if (flashlight != null)
        {
            float batteryPercent = currentBattery / maxBattery;
            flashlight.pointLightOuterRadius = Mathf.Lerp(minLightRadius, maxLightRadius, batteryPercent);
        }

        // 3. UIテキストの表示を更新する
        if (batteryText != null)
        {
            batteryText.text = "電池 : " + Mathf.CeilToInt(currentBattery) + "%";
        }
    }

    // ★電池アイテムを拾った時に呼び出す処理
    public void RecoverBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Min(currentBattery, maxBattery); // 100%を超えないようにする
        Debug.Log("電池を回復！ 現在のバッテリー: " + currentBattery);
    }
}
