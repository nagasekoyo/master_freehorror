using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // 移動速度
    public LayerMask obstacleLayer;    // 障害物レイヤー（InspectorでObstacleを選択）
    
    private bool isMoving = false;
    private Vector2 input;

    void Update()
    {
        // 移動中でない時だけ入力を受け付ける
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // 斜め移動を防ぐ（横入力があれば縦を無視）
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                // 次の移動先を計算
                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0f);

                // 次の地点に障害物がないか円形のセンサーでチェック
                if (CanMove(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }

    // 移動可能か判定するメソッド
    bool CanMove(Vector3 targetPos)
    {
        // 半径0.3の円を投げ、obstacleLayerにぶつかったら移動不可 (半径を大きくしすぎると、狭い通路を通れなくなる)
        return !Physics2D.OverlapCircle(targetPos, 0.3f, obstacleLayer);
    }

    // 1マス分移動させる
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // ぴったり目的地の座標に合わせる
        transform.position = targetPos;
        isMoving = false;
    }
}