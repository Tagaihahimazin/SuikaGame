using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitGenerateHandler : MonoBehaviour
{
    public List<GameObject> fruitPrefabs; // フルーツのプレハブリスト
    public KeyCode generateKey = KeyCode.G; // フルーツ生成キー
    private float speed = 0.5f;
    private Vector2 touchStart;
    private bool isMoving = false;
    private bool isDrop = false;
    public GameObject currentfruit;
    private GameObject canvas;
    private Button dropButton;
    private Rigidbody fruitRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        fruitRigidbody = currentfruit.GetComponent<Rigidbody>();
        canvas = GameObject.Find("Canvas");
        dropButton = GameObject.Find("DropButton").GetComponent<UnityEngine.UI.Button>();
        dropButton.onClick.AddListener(GenerateFruit);
        if (fruitRigidbody != null)
        {
            fruitRigidbody.useGravity = false;
            isDrop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.GetComponent<GameManager>().isGameOver)
        {
            return;
        }
#if UNITY_EDITOR
        // 矢印キーで前後左右に移動する
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (this.transform.localPosition.z < 0.15f){
                this.transform.localPosition += speed * Vector3.forward * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (this.transform.localPosition.z > -0.15f){
                this.transform.localPosition += speed * Vector3.back * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (this.transform.localPosition.x > -0.15f){
                this.transform.localPosition += speed * Vector3.left * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (this.transform.localPosition.x < 0.15f){
                this.transform.localPosition += speed * Vector3.right * Time.deltaTime;
            }
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // タッチ開始
                    touchStart = touch.position;
                    isMoving = true;
                    break;

                case TouchPhase.Moved:
                    // タッチ中に移動
                    if (isMoving)
                    {
                        if (this.transform.localPosition.z <= 0.15f && this.transform.localPosition.z >= -0.15f && this.transform.localPosition.x <= 0.15f && this.transform.localPosition.x >= -0.15f)
                        {
                            // タッチした位置からの移動量を計算
                            Vector2 touchEnd = touch.position;
                            float xMove = touchEnd.x - touchStart.x;
                            float yMove = touchEnd.y - touchStart.y;

                            // オブジェクトを移動
                            this.transform.localPosition += speed * new Vector3(xMove, 0, yMove) * Time.deltaTime * 0.001f;
                        }
                        else
                        {
                            if (this.transform.localPosition.z > 0.15f)
                            {
                                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.15f);
                            }
                            if (this.transform.localPosition.z < -0.15f)
                            {
                                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.15f);
                            }
                            if (this.transform.localPosition.x > 0.15f)
                            {
                                this.transform.localPosition = new Vector3(0.15f, this.transform.localPosition.y, this.transform.localPosition.z);
                            }
                            if (this.transform.localPosition.x < -0.15f)
                            {
                                this.transform.localPosition = new Vector3(-0.15f, this.transform.localPosition.y, this.transform.localPosition.z);
                            }
                            if (!fruitRigidbody.useGravity){
                                this.transform.localPosition = this.transform.localPosition;
                            }
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    // タッチ終了
                    isMoving = false;
                    break;
            }
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(generateKey))
        {
            GenerateFruit();
        }
#endif
        if (currentfruit != null)
        {
            Rigidbody fruitRigidbody = currentfruit.GetComponent<Rigidbody>();
            if (!fruitRigidbody.useGravity)
            {
                currentfruit.transform.position = this.transform.position;
            }
        }
    }
    void GenerateFruit()
    {
        // if (!isMoving)
        {
            if (currentfruit != null && !isDrop) {
                isDrop = true;
                DropFruit();
                // ちょっと待つ wait
                Invoke("GenerateRandomFruit", 1.0f);
            }
        }
    }
    void GenerateRandomFruit()
    {
        if (fruitPrefabs.Count > 0)
        {
            // ランダムなフルーツプレハブを選択
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Count)];
            var pos = this.transform.position; // 結合時にcurrentfruitが空になってしまうからエラーが出ている
            pos.y = 0.35f;

            // フルーツを生成(自分自身の親の子要素にする)
            currentfruit = Instantiate(fruitPrefab, pos, Quaternion.identity);
            currentfruit.transform.SetParent(this.transform.parent);

            // 新しく生成されたインスタンスのRigidbodyコンポーネントを取得
            Rigidbody fruitRigidbody = currentfruit.GetComponent<Rigidbody>();

            // Rigidbodyがあれば、重力を無効にする
            if (fruitRigidbody != null)
            {
                fruitRigidbody.useGravity = false;
                isDrop = false;
                currentfruit.GetComponent<FruitEvolutionHandler>().GenFruit();
            }
        }
    }
    void DropFruit()
    {
        currentfruit.GetComponent<FruitEvolutionHandler>().DropFruit();
    }
}
