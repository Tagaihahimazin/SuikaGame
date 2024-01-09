using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitEvolutionHandler : MonoBehaviour
{
    public GameObject[] FruitPrefabs;
    public int[] scorelist = {0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66};
    public int Level = 0;
    private bool isCollided = false;
    public float waitTime = 1.0f; // 待機時間（秒）

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody fruitRigidbody = this.gameObject.GetComponent<Rigidbody>();
        
        // GameOverオブジェクトに触れた場合の処理
        if (other.gameObject.CompareTag("gameover") && fruitRigidbody.useGravity == true && !isCollided)
        {
            Debug.Log("Game Over");
            canvas.GetComponent<GameManager>().GameOver();

            // ここにゲームオーバー時の処理を追加
            // 例: ゲームオーバーシーンへの遷移、スコアの表示、ゲームのリセットなど

            // 必要に応じて、フルーツのオブジェクトを削除
            // Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCollided) return;

        // // "gameover" タグを持つオブジェクトとの衝突をチェック
        // if (collision.gameObject.CompareTag("gameover"))
        // {
        //     Debug.Log("Game Over");

        //     // Rigidbodyの取得
        //     Rigidbody rb = GetComponent<Rigidbody>();

        //     // Rigidbodyが存在する場合、物理演算を停止
        //     if (rb != null)
        //     {
        //         rb.isKinematic = true;
        //     }
        //     // ここにゲームオーバー時の処理を追加（例：シーンのリロード、ゲームオーバーメニューの表示など）
        //     return;
        // }

        if (collision.gameObject.CompareTag("Untagged")) return;

        if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID()) 
        {
            // 衝突したオブジェクトが同じタグを持つフルーツかどうかをチェック
            if (collision.gameObject.CompareTag(gameObject.tag))
            {
                isCollided = true;
                if (Level < FruitPrefabs.Length - 1)
                {
                    Debug.Log("Level Up");
                    Instantiate(FruitPrefabs[Level+1], transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.Log("MAX LEVEL");
                }
                canvas.GetComponent<GameManager>().UpdateScore(scorelist[Level + 1]);
                Destroy(gameObject); // 現在のフルーツを破壊
                Destroy(collision.gameObject); // 衝突したフルーツを破壊
            }
        }
    }

    public void GenFruit()
    {
        Debug.Log("Gen Fruit");
        isCollided = true;
    }
    public void DropFruit()
    {
        Debug.Log("Drop Fruit");
        Rigidbody fruitRigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (fruitRigidbody != null)
        {
            fruitRigidbody.useGravity = true;
            StartCoroutine(ResetCollision());
        }
    }
    IEnumerator ResetCollision()
    {
        // 1秒待機
        yield return new WaitForSeconds(0.1f);

        // isCollidedをfalseに設定
        isCollided = false;
    }
}