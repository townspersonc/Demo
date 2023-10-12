using UnityEngine;
using System.Collections;

public class WindowNextFrameCloseButton : WindowCloseButton
{
    private Coroutine _nextFrameCloseRoutine;

    public override void Close()
    {
        StopNextFrameCloseRoutine();
        _nextFrameCloseRoutine = StartCoroutine(NextFrameCloseRoutine());
    }
    private IEnumerator NextFrameCloseRoutine()
    {
        yield return new WaitForEndOfFrame();
        Close();
    }
    private void StopNextFrameCloseRoutine()
    {
        if (_nextFrameCloseRoutine != null) StopCoroutine(_nextFrameCloseRoutine);
        _nextFrameCloseRoutine = null;
    }
}
