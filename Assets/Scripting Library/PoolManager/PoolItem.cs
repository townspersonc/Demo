using UnityEngine;

public abstract class PoolItem : MonoBehaviour
{
    protected PoolManager _poolManager;

    protected virtual void Awake()
    {
        _poolManager = Referencer.Get<PoolManager>();
    }

    public virtual void OnReleaseFromPool() { }
    public virtual void OnReturnToPool() { }

    public void ReturnToPool() => _poolManager.Put(this);
}
