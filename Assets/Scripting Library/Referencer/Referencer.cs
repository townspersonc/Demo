using System;
using System.Collections.Generic;
using System.Linq;

public class Referencer : Singleton<Referencer>
{
    private Dictionary<Type, HashSet<object>> _refs = new Dictionary<Type, HashSet<object>>();

    public static void Add(object obj) => Instance.AddInternal(obj.GetType(), obj);
    public static void Add<T>(object obj) => Instance.AddInternal(typeof(T), obj);
    private void AddInternal(Type objType, object obj)
    {
        if (_refs.TryGetValue(objType, out var list))
        {
            ClearNulls(list, objType);
            list.Add(obj);
        }
        else
        {
            _refs.Add(objType, new HashSet<object>() { obj });
        }
    }

    public static void Remove(object obj) => Instance.RemoveInternal(obj.GetType(), obj);
    public static void Remove<T>(object obj) => Instance.RemoveInternal(typeof(T), obj);
    private void RemoveInternal(Type objType, object obj)
    {
        if (_refs.TryGetValue(objType, out var list))
        {
            list.Remove(obj);
            ClearNulls(list, objType);
        }
    }

    public static void Remove<T>() => Instance.RemoveInternal(typeof(T));
    private void RemoveInternal<T>() => RemoveInternal(typeof(T));
    private void RemoveInternal(Type type) => _refs.Remove(type);

    public static T Get<T>() => Instance.GetInternal<T>();
    private T GetInternal<T>()
    {
        var objType = typeof(T);

        if (_refs.TryGetValue(objType, out var list))
        {
            ClearNulls(list, objType);
            if (list.Count > 0) return (T)list.ElementAt(0);
        }

        return default(T);
    }

    public static IReadOnlyCollection<T> GetMultiple<T>() => Instance.GetMultipleInternal<T>();
    private IReadOnlyCollection<T> GetMultipleInternal<T>()
    {
        var objType = typeof(T);

        if (_refs.TryGetValue(objType, out var list))
        {
            ClearNulls(list, objType);
            return list.Cast<T>().ToList();
        }

        return new HashSet<T>();
    }

    private void ClearNulls(HashSet<object> set, Type type)
    {
        set.RemoveWhere(x => x is null);
        if (set.Count == 0) RemoveInternal(type);
    }
}
