using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Limits & Stats")]
    public int lostBooksCount = 0;
    public int maxLostAllowed = 3; // 3-book loss limit per level
    public int currentLevel = 1;
    public int score = 0;
    public int booksRequiredToAdvance = 5;
    private int booksReturnedThisLevel = 0;

    [Header("Spawning & Difficulty Settings")]
    public GameObject ghostPrefab;
    public GameObject bookwormPrefab;
    public Transform[] ghostSpawnPoints;
    public Transform[] wormSpawnPoints;
    public Bookshelf[] targetBookshelves;

    public float baseGhostInterval = 5f;
    public float baseWormInterval = 7f;

    [Header("UI")]
    public TMP_Text uiText;

    public List<Transform> ActiveBooks { get; private set; } = new List<Transform>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnGhostsRoutine());
        StartCoroutine(SpawnWormsRoutine());
    }

    public void RegisterActiveBook(Transform bookTransform)
    {
        if (!ActiveBooks.Contains(bookTransform))
            ActiveBooks.Add(bookTransform);
    }

    public void UnregisterActiveBook(Transform bookTransform)
    {
        if (ActiveBooks.Contains(bookTransform))
            ActiveBooks.Remove(bookTransform);
    }

    private IEnumerator SpawnGhostsRoutine()
    {
        while (true)
        {
            // Spawn speed increases each level[cite: 1]
            float interval = Mathf.Max(1.5f, baseGhostInterval - (currentLevel * 0.4f));
            yield return new WaitForSeconds(interval);

            if (ghostSpawnPoints.Length > 0 && targetBookshelves.Length > 0)
            {
                Transform spawnPoint = ghostSpawnPoints[Random.Range(0, ghostSpawnPoints.Length)];
                Bookshelf targetShelf = targetBookshelves[Random.Range(0, targetBookshelves.Length)];

                GameObject ghostObj = Instantiate(ghostPrefab, spawnPoint.position, Quaternion.identity);
                GhostAI ghost = ghostObj.GetComponent<GhostAI>();
                if (ghost != null) ghost.Initialize(targetShelf.transform);
            }
        }
    }

    private IEnumerator SpawnWormsRoutine()
    {
        while (true)
        {
            float interval = Mathf.Max(2f, baseWormInterval - (currentLevel * 0.5f));
            yield return new WaitForSeconds(interval);

            if (wormSpawnPoints.Length > 0 && ActiveBooks.Count > 0)
            {
                Transform spawnPoint = wormSpawnPoints[Random.Range(0, wormSpawnPoints.Length)];
                Instantiate(bookwormPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    public void BookReturned()
    {
        score += 100;
        booksReturnedThisLevel++;

        if (booksReturnedThisLevel >= booksRequiredToAdvance)
        {
            NextLevel();
        }

        UpdateUI();
    }

    public void BookDestroyedByWorm()
    {
        lostBooksCount++;
        UpdateUI();

        if (lostBooksCount >= maxLostAllowed)
        {
            Debug.Log("Game Over! Final Score: " + score);
            Time.timeScale = 0f; // Freeze game on loss
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        lostBooksCount = 0; // Refreshes 3-book limit each level
        booksReturnedThisLevel = 0;
        booksRequiredToAdvance += 2; // Increase goal for next level

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (uiText != null)
        {
            uiText.text = $"Level: {currentLevel} | Lost: {lostBooksCount}/{maxLostAllowed} | Score: {score}";
        }
    }
}
