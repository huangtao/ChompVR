using UnityEngine.SceneManagement;

public class MenuController : Singleton<MenuController>
{
    public void LoadScene(Scene scene)
    {
        string sceneToLoad = "";

        
        if (scene == Scene.Last)
            scene = (Scene)ApplicationModel.CurrentLevel;


        switch (scene)
        {
            case Scene.One:
                sceneToLoad = "Level 1";
                ApplicationModel.CurrentLevel = 1;
                break;

            case Scene.Two:
                sceneToLoad = "Level 2";
                ApplicationModel.CurrentLevel = 2;
                break;

            case Scene.Three:
                sceneToLoad = "Level 3";
                ApplicationModel.CurrentLevel = 3;
                break;

            case Scene.Tutorial:
                sceneToLoad = "TutorialScene";
                ApplicationModel.CurrentLevel = 0;
                break;
                
            case Scene.Menu:
            default:
                sceneToLoad = "Menu";
                break;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
