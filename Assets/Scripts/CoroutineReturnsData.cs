using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineReturnsData  {

    public Coroutine coroutine { get; set; }
    public System.Object result;
    private IEnumerator target;

    public CoroutineReturnsData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}
