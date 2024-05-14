using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Flying : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador
    public float chaseSpeed = 5f; // Velocidad de persecución
    public float attackRange = 10f; // Rango de ataque
    public float attackCooldown = 2f; // Tiempo entre cada ataque
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto de origen del proyectil
    public float projectileSpeed = 10f; // Velocidad de la bala
    public float projectileLifetime = 3f; // Tiempo de vida de la bala

    private bool isAttacking = false; // Indica si el enemigo está atacando

    private void Update()
    {
        // Si no estamos atacando, perseguir al jugador
        if (!isAttacking)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mirar hacia la posición del jugador
        transform.LookAt(player);

        // Mover el enemigo en la dirección del jugador
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        // Si el jugador está dentro del rango de ataque, detenerse y atacar
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Detenerse
            isAttacking = true;

            // Iniciar el temporizador de enfriamiento del ataque
            Invoke("Attack", attackCooldown);
        }
    }

    private void Attack()
    {
        // Disparar proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Obtener el componente Rigidbody de la bala
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        // Agregar fuerza hacia adelante a la bala
        projectileRigidbody.AddForce(firePoint.forward * projectileSpeed, ForceMode.VelocityChange);

        // Destruir la bala después de cierto tiempo
        Destroy(projectile, projectileLifetime);

        // Establecer el estado de ataque a falso para permitir que el enemigo persiga nuevamente al jugador
        isAttacking = false;
    }
}
