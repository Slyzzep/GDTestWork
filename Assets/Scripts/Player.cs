using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class Player : MonoBehaviour
{
    [SerializeField] private Image _superAttackButtonImg;
    [SerializeField] private Button _superAttackButton;

    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    [SerializeField] private float _superAttackSpeed;
    private bool _defaultAttack;
    private bool _superAttack;

    

    private float lastAttackTime = 0;
    private bool isDead = false;
        public Animator AnimatorController;

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }
        

        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemie.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= AttackRange)
            { 
                transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);
                
                _superAttackButton.interactable = true;
            }
            if (Time.time - lastAttackTime > AtackSpeed && _defaultAttack)
                   {
                _defaultAttack = false;
                lastAttackTime = Time.time;
                AnimatorController.SetTrigger("Attack");
                
                
                if (distance <= AttackRange)
                {
                    //transform.LookAt(closestEnemie.transform);
         
                       closestEnemie.Hp -= Damage;
       
                 }
                    }
            else if (Time.time - lastAttackTime > _superAttackSpeed && _superAttack)
            {
                _superAttack = false;
                lastAttackTime = Time.time;
                //Debug.Log("time of attack" + lastAttackTime);
               // Debug.Log("time beetwen " + (Time.time - lastAttackTime));
                AnimatorController.SetTrigger("Attack");
                
                _superAttackButtonImg.fillAmount = 0f;
                
                if (distance <= AttackRange)
                {
                    closestEnemie.Hp -= Damage+2;

                }
            }

            else if (distance > AttackRange)
            {
                _superAttackButton.interactable = false;
            }
        } 
        
        if(_superAttackButtonImg.fillAmount != 1f )
        {
          
                      _superAttackButtonImg.fillAmount = (Time.time - lastAttackTime) / _superAttackSpeed;
           // Debug.Log(Time.time - lastAttackTime / _superAttackSpeed);
             
        }
    }

    public void DefaultAttack()
    {

        _defaultAttack = true;
    }
    public void SuperAttack()
    {
        if (_superAttackButtonImg.fillAmount == 1f)
        {
            _superAttack = true;
        }
    }
    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }


}
