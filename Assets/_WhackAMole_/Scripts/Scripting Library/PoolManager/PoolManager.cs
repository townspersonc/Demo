using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private List<PoolDefinition> _definitions = null;

    //Poor nameing, This holds items that need to return to their definition parent on next frame(in order to avoid parenting errors on disable.)
    private List<PoolItem> _lostLambs = new List<PoolItem>();

    private void Update()
    {
        for (int i = _lostLambs.Count - 1; i >= 0; i--)
        {
            var item = _lostLambs[i];

            var def = GetDefinition(item.GetType());
            item.transform.SetParent(def.Container);
            _lostLambs.RemoveAt(i);
        }
    }

    public static T Get<T>(Transform parent, bool enable = true) where T : PoolItem => Instance.GetInternal<T>(parent, enable);

    public static void Put(PoolItem item) => Instance.PutInternal(item);

    private T GetInternal<T>(Transform parent = null, bool enable = true) where T : PoolItem
    {
        PoolItem item = null;
        var keyType = typeof(T);

        var def = GetDefinition(keyType);

        if (def != null)
        {
            if (def.PoolItems.Count == 0) Spawn(keyType);

            item = def.PoolItems[0];
            def.PoolItems.RemoveAt(0);
            def.ActiveItems.Add(item);

            if (parent) item.transform.SetParent(parent);
            if (enable) item.gameObject.Enable();

            item.OnReleaseFromPool();
        }

        return item as T;
    }

    private void PutInternal(PoolItem item)
    {
        var def = GetDefinition(item.GetType());

        if (def.ActiveItems.Remove(item))
        {
            if (item != null && item.gameObject != null)
            {
                item.OnReturnToPool();

                if (def.TotalItemCount >= def.PreferedAmount)
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    item.gameObject.Disable();
                    _lostLambs.Add(item);
                    def.PoolItems.Add(item);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Putting inactive pool item{item.name}");
        }
    }

    private void Spawn(Type keyType, uint amount = 1)
    {
        var def = GetDefinition(keyType);

        for (int i = 0; i < amount; i++)
        {
            PoolItem item = null;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                item = UnityEditor.PrefabUtility.InstantiatePrefab(def.Prefab, def.Container) as PoolItem;
            }
            else
            {
                item = Instantiate(def.Prefab, def.Container);
            }
#else
            item = Instantiate(def.Prefab, def.Container);
#endif

            item.gameObject.Disable();
            def.PoolItems.Add(item);
        }
    }

    private PoolDefinition GetDefinition(Type key) => _definitions.FirstOrDefault(d => d.KeyType == key);

#if UNITY_EDITOR
    [ContextMenu(nameof(Setup))]
    private void Setup()
    {
        for (int i = 0; i < _definitions.Count - 1; i++)
        {
            var iDef = _definitions[i];
            for (int j = i + 1; j < _definitions.Count; j++)
            {
                if (iDef.KeyType == _definitions[j].KeyType)
                {
                    Debug.LogError($"Duplicate PoolItem Keytype '{iDef.KeyType}' detected at index '{j}'", gameObject);
                    return;
                }
            }
        }

        transform.DestroyAllChildren();

        for (int i = 0; i < _definitions.Count; i++)
        {
            var def = _definitions[i];

            def.Container = new GameObject($"{def.KeyType.ToString()}_Container").transform;
            def.Container.SetParent(transform);

            def.PoolItems = new List<PoolItem>();
            def.ActiveItems = new List<PoolItem>();

            if (def.PreferedAmount > 0)
            {
                Spawn(def.KeyType, def.PreferedAmount);
            }
        }
    }
#endif
    [Serializable]
    private class PoolDefinition
    {
        public Type KeyType => Prefab.GetType();
        public PoolItem Prefab;
        [Tooltip("System will try to have this many total prefab instances if possible.")]
        public uint PreferedAmount;
        public Transform Container;
        public List<PoolItem> PoolItems, ActiveItems;
        public int TotalItemCount => PoolItems.Count + ActiveItems.Count;
    }
}
