public abstract class WindowOpenButton<T> : MenuButton where T : Window
{
    public void Open() => _menu.Open<T>(GetPayload());
    protected override void OnButtonClick() => Open();
    protected virtual Payload_Base GetPayload() => null;
}
