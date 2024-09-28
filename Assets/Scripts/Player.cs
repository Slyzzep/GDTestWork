using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
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

    private float _currDist;
    private Enemie _currClosestEnemie;


    private float lastAttackTime = 0;
    private bool isDead = false;
        public Animator AnimatorController;

    [SerializeField] private Transform _camTransform;
    private float _horizontalInput;
    private float _verticalInput;

    [SerializeField] private int _moveSpeed;
    private void Start()
    {
        SceneManager.Instance.OnEnemieDie += Heal;

       
    }
    private void OnDestroy()
    {
        SceneManager.Instance.OnEnemieDie -= Heal;
    }
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
        _currClosestEnemie = closestEnemie;
        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            _currDist = distance;

            if (distance <= AttackRange)
            {
                Vector3 direction = closestEnemie.transform.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);
                _superAttackButton.interactable = true;
            }
          
            else if (distance > AttackRange)
            {
                _superAttackButton.interactable = false;
            }
        }

        if (_superAttackButtonImg.fillAmount != 1f)
        {
            _superAttackButtonImg.fillAmount = (Time.time - lastAttackTime) / _superAttackSpeed;
        }
        InputButtons();
        
    }
    private void DefaultAttack()
    {       
            if (Time.time - lastAttackTime > AtackSpeed )
            {
                lastAttackTime = Time.time;
            AnimatorController.SetTrigger("Attack");
            if (_currDist <= AttackRange)
            {
                _currClosestEnemie.Hp -= Damage;
            }
        }

    }
    private void SuperAttack()
    {
        if (Time.time - lastAttackTime > _superAttackSpeed)
        {
            
            lastAttackTime = Time.time;
            AnimatorController.SetTrigger("SuperAttack");
            _superAttackButtonImg.fillAmount = 0f;
         if (_currDist <= AttackRange)
            {
                _currClosestEnemie.Hp -= Damage + 4;
            }
        }
    }

    private void InputButtons()
    {
        if (isDead) return;
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (_horizontalInput != 0 || _verticalInput != 0)
        {

            PlayerMove();
            AnimatorController.SetFloat("Speed", 1);
        }
        else
        {
            AnimatorController.SetFloat("Speed", 0);
        }
    }

    public void PlayerMove()
    {
        Vector3 forward = _camTransform.forward;
        Vector3 right = _camTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _verticalInput + right * _horizontalInput;

        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        
    }
    public void DefaultAttackOnClick()
    {
        if (isDead) return;
        DefaultAttack();
    }
    public void SuperAttackOnClick()
    {
        if (isDead) return;
        SuperAttack();
    }
    private void Heal(Enemie enemie)
    {
        Hp += 2;
       
    }
    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }


}
