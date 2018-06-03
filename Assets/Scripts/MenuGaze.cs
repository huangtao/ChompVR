using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuGaze : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float timeToActivate = 5f;
    public Material activeMaterial;
    public Text timerText;

    public MenuGazeCommand command = MenuGazeCommand.LoadScene;
    public Scene scene;

    private float gazeTime = 0f;
    private bool gazing = false;

    private Image image;
    private Material initialMaterial;


    void Start ()
    {
        var images = GetComponentsInChildren<Image>();
        if (images != null && images.Length > 0)
        {
            image = images[0];
            initialMaterial = image.material;
        }
    }


    void Update ()
    {
	    if (gazing)
        {
            gazeTime -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(gazeTime).ToString();

            if (gazeTime <= 0)
            {
                image.material = activeMaterial;
                gazing = false;

                switch(command)
                {
                    case MenuGazeCommand.Pause:
                        Pause();
                        break;

                    case MenuGazeCommand.Unpause:
                        Resume();
                        break;

                    default:
                        LoadScene(scene);
                        break;
                }
            }
        }
	}


    void LoadScene(Scene scene)
    {
        MenuController.Instance.LoadScene(scene);
    }


    void Pause()
    {
        ApplicationModel.GameState = GameState.Paused;
    }


    void Resume()
    {
        ApplicationModel.GameState = GameState.Playing;
    }


    public void OnPointerEnter(PointerEventData data)
    {
        gazing = true;
        gazeTime = timeToActivate;
        timerText.text = gazeTime.ToString();
    }


    public void OnPointerExit(PointerEventData data)
    {
        gazing = false;
        gazeTime = 0f;
        timerText.text = "";

        image.material = initialMaterial;
    }
}
