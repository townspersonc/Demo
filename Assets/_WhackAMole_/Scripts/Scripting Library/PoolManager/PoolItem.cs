using UnityEngine;

public abstract class PoolItem : MonoBehaviour
{
    public virtual void OnReleaseFromPool() { }
    public virtual void OnReturnToPool() { }

    public void ReturnToPool() => PoolManager.Put(this);
}
