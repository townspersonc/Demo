using DG.Tweening;
using UnityEngine.SceneManagement;

namespace WhackAMole
{
    public class GameManager : Singleton<GameManager>
    {
        public Configuration Configuration;
        public LevelManager LevelManager;

        public void LevelFinished(bool won)
        {
            var payload = new StartWindow.Payload(won ? StartWindow.Variant.Win : StartWindow.Variant.Lose);

            Menu.Instance.Open<StartWindow>(payload);
        }

        public void LevelAbort()
        {
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartNextLevel() => StartLevel(SaveGame.Level);
        public void StartLevel(int lvl)
        {
            var lvlData = Configuration.LevelConfiguration.GetLevelData(lvl);

            Menu.Instance.Close<StartWindow>();
            LevelManager.StartLevel(lvlData, Configuration.GeneralConfiguration);

        }
    }
}