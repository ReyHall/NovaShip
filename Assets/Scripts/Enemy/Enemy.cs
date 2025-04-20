using UnityEngine;
public class Enemy : MonoBehaviour
{
    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnDestroyed;

    private void OnDestroy()
    {
        if (OnDestroyed != null) OnDestroyed.Invoke();
    }
}
