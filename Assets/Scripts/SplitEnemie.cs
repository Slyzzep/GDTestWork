using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class SplitEnemie : MonoBehaviour
{
    public GameObject[] Characters;


    private Enemie _enemie;
    private void Start()
    {
        _enemie = GetComponent<Enemie>();
    }


    private void Update()
    {
        SceneManager.Instance._haveSplitEnemy = true;
        DeadCheck();
    }
    private void DeadCheck()
    {
        if (_enemie.isDead)
        {
            Vector3 _spawnpos = transform.position;
            foreach (var character in Characters)
            {
                Instantiate(character, _spawnpos, Quaternion.identity);
                _spawnpos += new Vector3(1, 0, 0);
            }
            SceneManager.Instance._haveSplitEnemy = false;
            Destroy(gameObject);
        }
    }
}



