using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class ItemManager : MonoBehaviour
{
    private Tilemap itemTilemap;
    public int hasItemCount = 0; // 拾ったアイテムの数

    public TMP_Text keyText;

    public AudioClip itemSEClip;

    void Start()
    {
        // 自分自身（Item_Tilemap）のタイルマップコンポーネントを取得
        itemTilemap = GetComponent<Tilemap>();
    }

    // プレイヤーがタイルマップの判定の中にいる間、ずっと監視する（Stayに変更！）
    void OnTriggerStay2D(Collider2D other)
    {
        // 触れてきた相手のタグが「Player」のときだけ処理する
        if (other.CompareTag("Player"))
        {
            // プレイヤーが今いる世界（Grid）の座標を、タイルのマス目座標に変換する
            Vector3Int cellPosition = itemTilemap.WorldToCell(other.transform.position);

            // ★そのマスにタイルが存在するかチェック
            if (itemTilemap.HasTile(cellPosition))
            {
                // ★1. 本当にアイテムが存在した瞬間だけ、音を鳴らす！
                if (itemSEClip != null)
                {
                    AudioSource.PlayClipAtPoint(itemSEClip, transform.position);
                }

                // ★2. そのマスのタイルを消去する（空気にする）
                itemTilemap.SetTile(cellPosition, null);

                // ★3. アイテム所持数を1増やす
                hasItemCount++;
                Debug.Log("アイテムを拾った！ 現在の所持数: " + hasItemCount);
            }
        }
    }

    public void GetKey()
    {
        hasItemCount++;
        // UIの表示を更新する！
        keyText.text = "鍵 : " + hasItemCount;
    }
}