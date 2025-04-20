using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações de Nível")]
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 10;

    [Header("Movimento e Tiro")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stoppingDistance = 3f;
    [SerializeField] private float shootingRange = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float baseFireRate = 1.5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float behaviorChangeInterval = 5f;

    [Header("Ajuste de Disparo")]
    [SerializeField] private Vector2 fireOffset = new Vector2(0f, 0.5f);

    [Header("Explosão do inimigo nível 1")]
    [SerializeField] private float bombCountdown = 3f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashSpeed = 0.5f;

    private Transform playerTransform;
    private float nextFireTime = 0f;
    private bool aggressive;
    private Coroutine passiveRoutine;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private bool isBombActive = false;
    private bool hasEnteredScreen = false;  // Flag to track if the enemy is fully visible

    private Vector3 targetPosition;
    private bool movingRight = true;
    private float timeSinceLastMove = 0f;
    private float moveChangeInterval = 2f;

    private FlashColorEffect flashColorEffect;
    private TrembleEffect trembleEffect;

    private void Start()
    {
        flashColorEffect = GetComponent<FlashColorEffect>();
        trembleEffect = GetComponent<TrembleEffect>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        mainCamera = Camera.main;
        CalculateScreenBounds();

        StartCoroutine(EnterScene());

        ChooseBehavior();
        InvokeRepeating(nameof(ChooseBehavior), behaviorChangeInterval, behaviorChangeInterval);

        if (level == 1)
        {
            Invoke(nameof(StartBombBehavior), 0.5f);
        }
    }

    private IEnumerator EnterScene()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(0f, -2f, 0f);
        float duration = 1.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        // Marca como visível após a transição de entrada
        hasEnteredScreen = IsFullyVisible();
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (!hasEnteredScreen) return;  // Não reage até estar totalmente visível

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (isBombActive)
        {
            if (distanceToPlayer > 0.5f)
            {
                MoveTowardsPlayer();
            }
            return;
        }

        if (level == 7)
        {
            MoveHorizontally();

            if (distanceToPlayer <= shootingRange && Time.time >= nextFireTime)
            {
                Shoot();
                float adjustedFireRate = Mathf.Max(0.5f, baseFireRate - level * 0.1f);
                nextFireTime = Time.time + adjustedFireRate;
            }

            AvoidPlayerShots();
        }
        else
        {
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
                float adjustedFireRate = Mathf.Max(0.5f, baseFireRate - level * 0.1f);
                nextFireTime = Time.time + adjustedFireRate;
            }
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

    private void MoveHorizontally()
    {
        if (Time.time - timeSinceLastMove >= moveChangeInterval)
        {
            movingRight = !movingRight;
            timeSinceLastMove = Time.time;
        }

        float direction = movingRight ? 1f : -1f;
        Vector3 randomDirection = new Vector3(direction, 0, 0);
        Vector3 nextPosition = transform.position + randomDirection * speed * Time.deltaTime;

        if (IsWithinBounds(nextPosition))
        {
            transform.position = nextPosition;
        }
    }

    private void AvoidPlayerShots()
    {
        // Lógica futura
    }

    private void Shoot()
    {
        if (bulletPrefab != null)
        {
            Vector2 shootDirection = (playerTransform.position - transform.position).normalized;
            Vector3 spawnPosition = transform.position + (Vector3)(shootDirection * fireOffset.y) + new Vector3(fireOffset.x, 0f, 0f);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 90));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection * (bulletSpeed + level * 0.2f);
            }

            Destroy(bullet, 1.5f);
        }
    }

    private void ChooseBehavior()
    {
        float aggressionChance = Mathf.Clamp01(1f - (float)level / maxLevel);
        aggressive = Random.value < aggressionChance;

        if (!aggressive)
        {
            if (passiveRoutine != null) StopCoroutine(passiveRoutine);
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
                if (IsWithinBounds(nextPosition)) transform.position = nextPosition;

                elapsedTime += Time.deltaTime;
                yield return null;

                if (aggressive) yield break;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    private void StartBombBehavior()
    {
        if (!isBombActive)
        {
            // Verifica se o inimigo está completamente visível na tela
            if (hasEnteredScreen && IsFullyVisible())
            {
                isBombActive = true;
                StartCoroutine(StartBombCountdown());
            }
            else
            {
                // Se não estiver visível, espera até ficar visível
                StartCoroutine(WaitUntilVisibleAndStartBombBehavior());
            }
        }
    }

    private IEnumerator WaitUntilVisibleAndStartBombBehavior()
    {
        // Espera até o inimigo estar completamente visível
        while (!IsFullyVisible())
        {
            yield return null;
        }

        // Após o inimigo ficar visível, inicia a lógica de explosão
        isBombActive = true;
        StartCoroutine(StartBombCountdown());
    }

    private IEnumerator StartBombCountdown()
    {
        isBombActive = true;
        float elapsed = 0f;
        bool isRed = false;

        while (elapsed < bombCountdown)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isRed ? Color.white : flashColor;
                isRed = !isRed;
            }

            if (trembleEffect != null)
            {
                trembleEffect.TriggerShake();
            }

            if (flashColorEffect != null)
            {
                flashColorEffect.Flash();
            }

            yield return new WaitForSeconds(flashSpeed);
            elapsed += flashSpeed;
        }

        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            if (explosion) Destroy(explosion, 1.5f);
        }

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= explosionRadius)
            {
                Player player = playerTransform.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(20);
                }

                Debug.Log("Jogador recebeu dano da explosão!");
            }
        }

        // Destruir o inimigo após a explosão
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (level == 1 && other.CompareTag("Player"))
        {
            Explode();
        }
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

    private bool IsFullyVisible()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        return screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height;
    }
}