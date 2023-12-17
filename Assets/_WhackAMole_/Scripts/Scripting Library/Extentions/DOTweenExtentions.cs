using DG.Tweening;

public static class DOTweenExtentions
{
    public static void OverKill(this Tween tween)
    {
        if (tween.IsActive()) tween.Kill();
    }
    public static void OverKill(this Sequence seq)
    {
        if (seq.IsActive()) seq.Kill();
    }
}