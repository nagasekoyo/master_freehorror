using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;        // 追跡速度（プレイヤーより遅めの 2 くらいがおすすめ）
    public LayerMask obstacleLayer;     // 障害物レイヤー

    private Transform playerTransform;   // プレイヤーの位置を覚える変数
    private bool isMoving = false;

    void Start()
    {
        // 画面内の「Player」を自動で見つける
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        // 移動中でなく、プレイヤーが見つかっていれば追跡開始
        if (!isMoving && playerTransform != null)
        {
            Vector3 targetPos = CalculateNextStep();

            // 次のマスに壁がなければ進む
            if (CanMove(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }
    }

    // プレイヤーに近づくための「次の1マス」を計算する
    Vector3 CalculateNextStep()
    {
        Vector3 currentPos = transform.position;
        Vector3 playerPos = playerTransform.position;

        // プレイヤーとの距離の差を計算
        float diffX = playerPos.x - currentPos.x;
        float diffY = playerPos.y - currentPos.y;

        Vector2 bestDir = Vector2.zero;

        // X軸（横）の距離の方が離れている場合、横に1マス近づく
        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            bestDir = diffX > 0 ? Vector2.right : Vector2.left;
        }
        // Y軸（縦）の距離の方が離れている場合、縦に1マス近づく
        else if (Mathf.Abs(diffY) > 0)
        {
            bestDir = diffY > 0 ? Vector2.up : Vector2.down;
        }

        return currentPos + new Vector3(bestDir.x, bestDir.y, 0f);
    }

    bool CanMove(Vector3 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.3f, obstacleLayer);
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
        // 1マス移動するごとに少しだけ考える時間（猶予）を作る
        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }
}