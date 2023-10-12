public class WindowToggleButton<T> : WindowOpenButton<T> where T : Window
{
    protected override void OnButtonClick()
    {
        var wind = _menu.GetWindow<T>();

        if (wind != null)
        {
            if (wind.CanOpen)
            {
                Open();
            }
            else if(wind.CanClose)
            {
                _menu.Close(wind);
            }
        }
    }
}