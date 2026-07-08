using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemManager : MonoBehaviour
{
    private Tilemap itemTilemap;
    public int hasItemCount = 0; // 拾ったアイテムの数

    void Start()
    {
        // 自分自身（Item_Tilemap）のタイルマップコンポーネントを取得
        itemTilemap = GetComponent<Tilemap>();
    }

    // プレイヤーがタイルマップの判定（Trigger）に触れた瞬間に実行される
    void OnTriggerEnter2D(Collider2D other)
    {
        // 触れてきた相手のタグが「Player」のときだけ処理する
        if (other.CompareTag("Player"))
        {
            // プレイヤーが今いる世界（Grid）の座標を、タイルのマス目座標に変換する
            Vector3Int cellPosition = itemTilemap.WorldToCell(other.transform.position);

            // そのマスにタイル（みどり〇）が存在するかチェック
            if (itemTilemap.HasTile(cellPosition))
            {
                // そのマスのタイルを消去する（空気にする）
                itemTilemap.SetTile(cellPosition, null);

                // アイテム所持数を1増やす
                hasItemCount++;
                Debug.Log("アイテムを拾った！ 現在の所持数: " + hasItemCount);
            }
        }
    }
}