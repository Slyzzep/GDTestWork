using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;


    public Animator AnimatorController;
    [SerializeField] public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    public bool isDead { private set; get; } = false;
    public event Action OnDie;




    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);
       
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
            Agent.isStopped = false;
        }
        AnimatorController.SetFloat("Speed", Agent.speed); 
        

    }
   


    private void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
        OnDie?.Invoke();
    }

}
