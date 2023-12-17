public abstract class WindowOpenButton<T> : MenuButton where T : Window
{
    public void Open() => Menu.Instance.Open<T>(GetPayload());
    protected override void OnButtonClick() => Open();
    protected virtual Payload_Base GetPayload() => null;
}
