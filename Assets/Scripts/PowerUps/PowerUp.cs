using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour
{
    [Tooltip("Lista de possíveis power-ups para troca aleatória")]
    public GameObject[] powerUpOptions;

    private SpriteRenderer spriteRenderer;
    private PowerUpType powerUpType;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        powerUpType = GetComponent<PowerUpType>();
    }

    public void TrocarPowerUp()
    {
        if (powerUpOptions.Length <= 1 || powerUpType == null) return;

        string currentID = powerUpType.powerUpID;

        // Cria uma lista de índices disponíveis, excluindo o índice do power-up atual
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < powerUpOptions.Length; i++)
        {
            var option = powerUpOptions[i];
            var optionType = option.GetComponent<PowerUpType>();
            if (optionType != null && optionType.powerUpID != currentID && !string.IsNullOrEmpty(optionType.powerUpID))
            {
                availableIndices.Add(i);
            }
        }

        // Se não houver opções diferentes, não faz nada
        if (availableIndices.Count == 0) return;

        // Seleciona aleatoriamente um índice da lista de disponíveis
        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        GameObject newPowerUp = powerUpOptions[randomIndex];
        PowerUpType newType = newPowerUp.GetComponent<PowerUpType>();

        // Atualiza o powerUpID e o sprite
        powerUpType.powerUpID = newType.powerUpID;

        Sprite newSprite = newPowerUp.GetComponent<SpriteRenderer>()?.sprite;
        if (newSprite != null) spriteRenderer.sprite = newSprite;
    }

    public IEnumerator MoverParaCimaSuavemente(float distancia, float duracao)
    {
        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoFinal = posicaoInicial + Vector3.up * distancia;
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracao)
        {
            transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, tempoDecorrido / duracao);
            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transform.position = posicaoFinal;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
