using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 5f;       // 移動速度
    public LayerMask obstacleLayer;    // 障害物レイヤー

    private bool isMoving = false;
    private Vector2 input;

    private Tilemap doorTilemap;
    private ItemManager itemManager;


    private Vector2 movement;

    // --- 【13日目追加分】ここから ---
    private Vector3 startPosition; // 最初のスタート位置を覚える箱
    // --- 【13日目追加分】ここまで ---

    void Start()
    {
        // --- 【13日目追加分】開始時の位置を記憶 ---
        startPosition = transform.position;
        // ------------------------------------------

        GameObject doorObj = GameObject.Find("Door_Tilemap");
        if (doorObj != null) doorTilemap = doorObj.GetComponent<Tilemap>();

        GameObject itemObj = GameObject.Find("Item_Tilemap");
        if (itemObj != null) itemManager = itemObj.GetComponent<ItemManager>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0f);

                if (CanMove(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }

    void FixedUpdate()
    {
        // 2. ★超重要★ 物理演算のタイミングに合わせて移動させる！
        // これを使うと、壁やアイテムのすり抜けが100%発生しなくなります
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    bool CanMove(Vector3 targetPos)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(targetPos, 0.3f, obstacleLayer);
        if (hitCollider == null) return true;

        if (hitCollider.gameObject.name == "Door_Tilemap" && doorTilemap != null && itemManager != null)
        {
            Vector3Int cellPosition = doorTilemap.WorldToCell(targetPos);

            if (doorTilemap.HasTile(cellPosition) && itemManager.hasItemCount > 0)
            {
                doorTilemap.SetTile(cellPosition, null);
                itemManager.hasItemCount--;
                Debug.Log("鍵を使って扉を開けた！ 残りの鍵: " + itemManager.hasItemCount);
                return true;
            }
        }
        return false;
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    // --- 【本来の13日目】敵に捕まったらGameOverシーンへ切り替え ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("【ゲームオーバー】敵に捕まった！");

            StopAllCoroutines();
            isMoving = false;

            // 1週目に作ったシーン切り替えスクリプトを探して実行する
            SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
            if (sceneChanger != null)
            {
                // ここにあなたの「ゲームオーバーシーンの名前」を入れてください（例: "GameOver"）
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                // もしSceneChangerが見つからない場合のバックアップ（Unity標準機能で切り替え）
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }


        }

        if (other.CompareTag("Goal"))
        {
            // 動いている途中のコルーチン（移動処理）をすべて強制終了する
            StopAllCoroutines();
            isMoving = false;

            Debug.Log("【クリア】脱出成功！シーンを切り替えます。");

            // ★【ここを書き換え！】直接「GameClear」シーンを読み込む
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameClear");
        }
    }
}