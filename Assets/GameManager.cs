using UnityEngine;
using UnityEngine.UI; // UI要素を使用するために必要

public class GameManager : MonoBehaviour
{
    public GameObject readyPanel; // Readyパネル
    public GameObject playPanel; // Playパネル
    public GameObject gameOverPanel; // ゲームオーバーパネル
    public Text scoreText; // スコアテキスト
    public Text GameOverscoreText; // スコアテキスト

    private int score = 0; // スコア変数
    public bool isGameOver = false; // ゲームオーバーフラグ

    public void Start()
    {
        readyPanel.SetActive(true); // Readyパネルを表示
        playPanel.SetActive(false); // Playパネルを非表示
        gameOverPanel.SetActive(false); // ゲームオーバーパネルを非表示
    }

    public void GameStart()
    {
        readyPanel.SetActive(false); // Readyパネルを非表示
        playPanel.SetActive(true); // Playパネルを表示
    }

    // スコアを更新するメソッド
    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
        GameOverscoreText.text = score.ToString();
    }

    // ゲームオーバー処理
    public void GameOver()
    {
        gameOverPanel.SetActive(true); // ゲームオーバーパネルを表示
        UpdateScore(score); // スコア表示を更新
        isGameOver = true; // ゲームオーバーフラグをtrueに
    }
}
