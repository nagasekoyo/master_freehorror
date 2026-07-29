using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    // 鳴らしたい効果音を入れる箱
    public AudioClip seClip;

    void Start()
    {
        // 画面が開いた瞬間（Start）に音を鳴らす
        if (seClip != null)
        {
            // カメラの目の前で音を再生する
            Vector3 cameraPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(seClip, cameraPos, 1.0f);
        }
    }
}