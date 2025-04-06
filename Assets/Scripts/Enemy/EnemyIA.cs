using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stoppingDistance = 3f;
    [SerializeField] private float shootingRange = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float behaviorChangeInterval = 5f;

    private Transform playerTransform;
    private float nextFireTime = 0f;
    private bool aggressive;
    private Coroutine passiveRoutine;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        mainCamera = Camera.main;
        CalculateScreenBounds();
        EnsureOnScreen();
        ChooseBehavior();
        InvokeRepeating(nameof(ChooseBehavior), behaviorChangeInterval, behaviorChangeInterval);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (aggressive && distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else if (!aggressive && distanceToPlayer < shootingRange - 1f)
        {
            MoveAwayFromPlayer();
        }

        if (distanceToPlayer <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;

        if (IsWithinBounds(nextPosition))
        {
            transform.position = nextPosition;
        }
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        Vector3 nextPosition = transform.position + direction * (speed * 0.75f) * Time.deltaTime;

        if (IsWithinBounds(nextPosition))
        {
            transform.position = nextPosition;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 135));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = (playerTransform.position - firePoint.position).normalized;
                rb.velocity = Vector2.Lerp(Vector2.zero, shootDirection * bulletSpeed, 0.5f);
            }
            Destroy(bullet, 1.5f);
        }
    }

    private void ChooseBehavior()
    {
        aggressive = Random.value > 0.5f;

        if (!aggressive)
        {
            if (passiveRoutine != null)
            {
                StopCoroutine(passiveRoutine);
            }
            passiveRoutine = StartCoroutine(PassiveBehavior());
        }
    }

    private IEnumerator PassiveBehavior()
    {
        while (!aggressive)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
            Vector3 targetPosition = transform.position + randomOffset;

            float elapsedTime = 0f;
            float moveTime = 1f;
            Vector3 startPosition = transform.position;

            while (elapsedTime < moveTime)
            {
                Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);

                if (IsWithinBounds(nextPosition))
                {
                    transform.position = nextPosition;
                }

                elapsedTime += Time.deltaTime;
                yield return null;

                if (aggressive) yield break;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    private void EnsureOnScreen()
    {
        if (mainCamera == null) return;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    private void CalculateScreenBounds()
    {
        if (mainCamera == null) return;

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        minX = -cameraWidth + 0.5f;
        maxX = cameraWidth - 0.5f;
        minY = -cameraHeight + 0.5f;
        maxY = cameraHeight - 0.5f;
    }

    private bool IsWithinBounds(Vector3 position)
    {
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}
