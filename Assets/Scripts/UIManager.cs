using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private AudioClip winSound;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }
    public void ShowWinScreen()
    {
        audioSource.PlayOneShot(winSound);
        winPanel.SetActive(true);
    }
}
