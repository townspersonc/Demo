using UnityEngine;

[DefaultExecutionOrder(-5)]
public class MonoReferenceable : MonoBehaviour
{
	protected virtual void OnEnable()
	{
         Referencer.Add(this);
	}
	protected virtual void OnDisable()
	{
        Referencer.Remove(this);
	}
}