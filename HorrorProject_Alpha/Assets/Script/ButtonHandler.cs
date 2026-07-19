using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必須

public class ButtonHandler : MonoBehaviour
{
    // ボタンが押されたときに呼び出す関数
    public void OnTitleButtonClick()
    {
        Debug.Log("タイトルへ戻ります。");

        // ここにあなたの「タイトルシーンの正確な名前」を入れてください
        SceneManager.LoadScene("Title");
    }
}