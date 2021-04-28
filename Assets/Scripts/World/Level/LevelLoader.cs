using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoSingleton<LevelLoader>
{
    string _firstLevelName = "Level_intro";
    int _scoringSceneIndex = 1;
    int _currentSceneIndex = 0;
    Scene _currentScene;

    public delegate void SceneIsLoading(bool cond);
    public event SceneIsLoading OnSceneIsLoading;

    private bool _isScoringScene = false;



    /// <summary>Called from Button </summary>
    public void LoadIntroLevel()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        if (_firstLevelName.Equals(""))
        {
            _currentScene = SceneManager.LoadScene(_scoringSceneIndex + 1, parameters);
        }
        else
        {
            _currentScene = SceneManager.LoadScene(_firstLevelName, parameters);
        }
        _currentSceneIndex = _currentScene.buildIndex;
    }

    public void LoadFirstLevel()
    {

        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        _currentScene = SceneManager.LoadScene(++_currentSceneIndex, parameters);
        //Init our score prefs
        PlayerPrefs.SetInt(GameConstants.HAT_SCORE_KEY, 0);
        Instantiate(Resources.Load<GameObject>("UI/GameCanvas"));
        //A Hack to skip thru
        _isScoringScene = true;
    }
    public void LoadNextLevel()
    {
        OnSceneIsLoading?.Invoke(true);
        Debug.Log($"Trying to load : {_currentSceneIndex} is scoring= {_isScoringScene} ");
        if (!_isScoringScene)
        {
            _isScoringScene = true;
            var totalScore = PlayerPrefs.GetInt(GameConstants.HAT_SCORE_KEY);
            var levelScore = GameCanvas.Instance.GetScore();
            PlayerPrefs.SetInt(GameConstants.HAT_SCORE_KEY, totalScore + levelScore);
            SceneManager.LoadScene(_scoringSceneIndex);
            GameCanvas.Instance.DisplayTotalVsCurrent(true);
        }
        else
        {
            _isScoringScene = false;
            /// Mandatory the victory scene ends the sequence
            SceneManager.LoadScene(++_currentSceneIndex);
            GameCanvas.Instance.DisplayTotalVsCurrent(false);
            GameCanvas.Instance.UpdateScore(0);
            AudioManager.Instance.PlayBackgroundMusic("ScoringLevel");
        }
        OnSceneIsLoading?.Invoke(false);
    }
}
