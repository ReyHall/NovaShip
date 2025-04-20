using UnityEngine;

public class PowerUpTriggerHandler : MonoBehaviour
{
    [SerializeField] private AudioClip audioCollectionItem;
    [SerializeField] private AudioSource audioSource;
    public PowerUp powerUp;

    private void Awake()
    {
        if (powerUp == null)
        {
            powerUp = GetComponent<PowerUp>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioCollectionItem != null)
            {
                GameObject tempAudio = new GameObject("TempAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = audioCollectionItem;
                tempSource.volume = 1f;
                tempSource.Play();

                Destroy(tempAudio, audioCollectionItem.length);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Shot"))
        {
            Destroy(collision.gameObject);
            if (powerUp != null)
            {
                StartCoroutine(powerUp.MoverParaCimaSuavemente(0.5f, 0.2f));
                powerUp.TrocarPowerUp();
            }
        }
    }

}
