using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public event Action<Enemie> OnEnemieDie;

    public Player Player;
    public List<Enemie> Enemies;

    public GameObject Lose;
    public GameObject Win;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;

    
    [SerializeField] private Text _statsText;

    public bool _haveSplitEnemy = false;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        OnEnemieDie?.Invoke(enemie);
       if (_haveSplitEnemy) return;
        if (Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        Lose.SetActive(true);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;
        _statsText.text = "Total waves: " + Config.Waves.Length + "\nCurrent wave: " + currWave.ToString();
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

}
