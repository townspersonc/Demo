using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtentions
{
    public static T Random<T>(this IEnumerable<T> enumerable) => enumerable.ElementAt(enumerable.RandomID());

    public static int RandomID<T>(this IEnumerable<T> enumerable) => UnityEngine.Random.Range(0, enumerable.Count());

    public static void LeaveNRandom<T>(this List<T> list, int newCount)
    {
        while (list.Count > newCount)
        {
            list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
        }
    }

    /// <summary>
    /// returns id as if it where carouseled on numbers from 0 to list.count. For example: id = 7, list.count = 3, result is 1. id = -1, count = 2, result is 2.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int CarouseleID<T>(this IEnumerable<T> list, int id)
    {
        return MathExtentions.RealModulo(id, list.Count());
    }

    /// <summary>
    /// returns item at id as if id where carouseled on numbers from 0 to list.count. For example: id = 7, list.count = 3, result is 1. id = -1, count = 2, result is 2.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T Carousele<T>(this IEnumerable<T> list, int id)
    {
        return list.ElementAt(list.CarouseleID(id));
    }

    /// <summary>
    /// Returns member on imidiate right of ID, edges work like carousele.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T Next<T>(this IEnumerable<T> list, int id)
    {
        return list.ElementAt(list.CarouseleID(id + 1));
    }

    /// <summary>
    /// Returns member on imidiate left of ID, edges work like carousele.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T Prev<T>(this IEnumerable<T> list, int id)
    {
        return list.ElementAt(list.CarouseleID(id - 1));
    }

    public static T ClosestTo<T>(this IEnumerable<T> list, Vector3 position) where T : MonoBehaviour
    {
        if (list.Count() == 0)
        {
            return null;
        }
        return list.Aggregate((t1, t2) => Vector3.SqrMagnitude(t1.transform.position - position) < Vector3.SqrMagnitude(t2.transform.position - position) ? t1 : t2);
    }
    
    public static T ClosestToInRange<T>(this IEnumerable<T> list, Vector3 position) where T : MonoBehaviour
    {
        return list.Aggregate((t1, t2) => Vector3.SqrMagnitude(t1.transform.position - position) < Vector3.SqrMagnitude(t2.transform.position - position) ? t1 : t2);
    }
}
