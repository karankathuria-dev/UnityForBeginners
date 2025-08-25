
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        Debug.Log("Current score : "+ score);
    }
    private void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }
}
