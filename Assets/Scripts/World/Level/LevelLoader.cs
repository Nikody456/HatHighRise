using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoSingleton<LevelLoader>
{
    string _firstLevelName = "SteveScene2";
    int _scoringSceneIndex = 1;
    int _currentSceneIndex = 0;
    Scene _currentScene;

    public delegate void SceneIsLoading(bool cond);
    public event SceneIsLoading OnSceneIsLoading;

    private bool _isScoringScene = false;

    private void Start()
    {
       
    }

    public void LoadFirstLevel()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        if (_firstLevelName.Equals(""))
        {
             _currentScene = SceneManager.LoadScene(_scoringSceneIndex+1, parameters);
        }
        else
        {
            _currentScene = SceneManager.LoadScene(_firstLevelName, parameters);
        }
        _currentSceneIndex = _currentScene.buildIndex;
        //Init our score prefs
        PlayerPrefs.SetInt(GameConstants.HAT_SCORE_KEY, 0);
    }
    public void LoadNextLevel()
    {
        OnSceneIsLoading?.Invoke(true);
        if (!_isScoringScene)
        {
            _isScoringScene = true;
            var totalScore = PlayerPrefs.GetInt(GameConstants.HAT_SCORE_KEY);
            var levelScore = GameCanvas.Instance.GetScore();
            PlayerPrefs.SetInt(GameConstants.HAT_SCORE_KEY, totalScore + levelScore);
            SceneManager.LoadScene(_scoringSceneIndex);
        }
        else
        {
            _isScoringScene = false;
            /// Mandatory the victory scene ends the sequence
            SceneManager.LoadScene(++_currentSceneIndex);
            GameCanvas.Instance.UpdateScore(0);
        }
        OnSceneIsLoading?.Invoke(false);
    }
}
