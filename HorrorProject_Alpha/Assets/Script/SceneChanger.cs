using UnityEngine;
using UnityEngine.SceneManagement; // これが必要です！

public class SceneChanger : MonoBehaviour
{
    // ボタンを押したときに実行する関数
    public void GoToMainGame()
    {
        // MainGameシーンを読み込む
        SceneManager.LoadScene("MainGame");
    }
}
