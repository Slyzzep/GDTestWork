using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class SplitEnemie : Enemie
{
    public GameObject[] Characters;


    private void OnEnable()
    {
        OnDie += DeadCheck; 
    }

    private void OnDisable()
    {
        OnDie -= DeadCheck; 
    }
    private void FixedUpdate()
    {
        SceneManager.Instance._haveSplitEnemy = true;

    }

    private void DeadCheck()
    {
        if (isDead)
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



