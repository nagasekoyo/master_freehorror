using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えを使うために必要！

public class Trap : MonoBehaviour
{
    // 移動先のゲームオーバーシーン名（インスペクターで変更可能）
    public string gameOverSceneName = "GameOver";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 触れた相手、またはその親に PlayerMovement（プレイヤー）がついているかチェック
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            Debug.Log("罠に触れた！ゲームオーバーへ遷移します");

            // 指定したシーンに移動する
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}