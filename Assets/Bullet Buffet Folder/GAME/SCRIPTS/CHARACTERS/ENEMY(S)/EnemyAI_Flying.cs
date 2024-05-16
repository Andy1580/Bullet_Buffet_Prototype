using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Flying : MonoBehaviour
{
    public Transform player; 
    public float chaseSpeed = 5f; 
    public float attackRange = 10f; 
    public float attackCooldown = 2f; 
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float projectileSpeed = 10f; 
    public float projectileLifetime = 3f; 

    private bool isAttacking = false; 

    private void Update()
    {
        
        if (!isAttacking)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        
        transform.LookAt(player);

        // Mover el enemigo en la dirección del jugador
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        // Si el jugador está dentro del rango de ataque, detenerse y atacar
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            
            isAttacking = true;

           
            Invoke("Attack", attackCooldown);
        }
    }

    private void Attack()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        
        projectileRigidbody.AddForce(firePoint.forward * projectileSpeed, ForceMode.VelocityChange);

        
        Destroy(projectile, projectileLifetime);

        
        isAttacking = false;
    }
}
