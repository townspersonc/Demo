public class WindowToggleButton<T> : WindowOpenButton<T> where T : Window
{
    protected override void OnButtonClick()
    {
        var wind = Menu.Instance.GetWindow<T>();

        if (wind != null)
        {
            if (wind.CanOpen)
            {
                Open();
            }
            else if(wind.CanClose)
            {
                Menu.Instance.Close(wind);
            }
        }
    }
}