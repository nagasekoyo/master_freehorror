using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要

public class Door : MonoBehaviour
{
    [Header("移動先のシーン名")]
    public string nextSceneName = "Stage2"; // インスペクターで変更可能

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 触れた相手（またはその親）がプレイヤーか判定
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            Debug.Log(nextSceneName + " へ移動します！");

            // 指定したシーンに移動
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
