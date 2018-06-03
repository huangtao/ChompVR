using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Class TalkManager.
/// </summary>
public class TalkManager : Singleton<TalkManager>
{
    #region Talking UI Setup

    [SerializeField]
    private Image[] talkingBackground;

    
    /// <summary>
    /// Gets the talking background.
    /// </summary>
    /// <value>The talking background.</value>
    public Image[] TalkingBackground
    {
        get
        {
            if (talkingBackground == null)
            {
                talkingBackground = GameObject.Find("TalkingBackground").GetComponentsInChildren<Image>();

            }
            return talkingBackground;
        }
    }


    [SerializeField]
    private Text talkingText;


    /// <summary>
    /// Gets the talking text.
    /// </summary>
    /// <value>The talking text.</value>
    public Text TalkingText
    {
        get
        {
            if (talkingText == null)
            {
                talkingText = GameObject.Find("TalkingText").GetComponent<Text>();
            }
            return talkingText;
        }
    }


    [SerializeField]
    private Sound talkSound;


    /// <summary>
    /// Gets the talking sound.
    /// </summary>
    /// <value>The talking sound.</value>
    public Sound TalkSound
    {
        get
        {
            if (talkSound == null)
            {
                talkSound = Sound.TalkSound;
            }
            return talkSound;
        }
    }

    private bool tipIsActive = false;

    private string[] charsToPauseOn = new string[] { " ", ".", ",", "!", "?", "'", "\"", ":", ";", "-", "_" };

    private const bool playTicOnDefault = false;

    #endregion

    #region Default Optional Parameter Constants

    private const float defaultTextSpeed = .2f;
    private const float defaultMultipleForGap = .25f;
    private const float defaultFadeSpeed = .5f;
    private const float defaultDisplayTime = 2f;

    #endregion

    #region Text Manipulating Methods

    private void StartTextImmediate()
    {
        ApplicationModel.GameState = GameState.Paused;

        PlayerController.Instance.IsDisplayingTutorialMessage = true;
        TalkingText.text = "";
        TalkingText.enabled = true;
        foreach (Image i in TalkingBackground)
            i.enabled = true;
    }


    private void StopTextImmediate()
    {
        StopAllCoroutines();
        TalkingText.text = "";
        TalkingText.enabled = false;
        foreach (Image i in TalkingBackground)
            i.enabled = false;
        PlayerController.Instance.IsDisplayingTutorialMessage = false;
        ApplicationModel.GameState = GameState.Playing;
    }


    private void ClearTheText(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        StopCoroutine("AnimateMultipleText");
        StopCoroutine("AnimateText");
        TalkingText.text = "";
    }

    #endregion

    #region Displaying Text Methods

    /// <summary>
    /// Displays the specified message for the specified length of time.
    /// Can use custom text speed.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="displayFor">The time to display.</param>
    /// <returns>IEnumerator.</returns>
    public IEnumerator DisplayFor(string message, float displayFor, float textSpeed = defaultTextSpeed, bool playTic = playTicOnDefault)
    {
        yield return StartCoroutine(DisplayTextFor(message, displayFor, textSpeed, playTic));
    }


    private IEnumerator DisplayTextFor(string message, float displayFor, float textSpeed, bool playTic)
    {
        StartTextImmediate();
        StartCoroutine(AnimateText(message, textSpeed, playTic));
        yield return new WaitForSeconds(displayFor);
        StopTextImmediate();
    }


    /// <summary>
    /// Displays multiple messages.
    /// Can use custom text speed or custom message gaps.
    /// </summary>
    /// <param name="messages">The messages.</param>
    /// <param name="displayForSeconds">The display for seconds.</param>
    /// <param name="textSpeed">The text speed.</param>
    /// <param name="messageGap">The message gap.</param>
    /// <returns>IEnumerator.</returns>
    public IEnumerator DisplayMultipleFor(string[] messages, float[] displayForSeconds, float textSpeed = defaultTextSpeed, float messageGap = defaultMultipleForGap, bool playTic = playTicOnDefault)
    {
        yield return StartCoroutine(DisplayMultipleTextFor(messages, displayForSeconds, textSpeed, messageGap, playTic));
    }


    private IEnumerator DisplayMultipleTextFor(string[] messages, float[] displayForSeconds, float textSpeed, float messageGap, bool playTic)
    {
        StartTextImmediate();
        yield return StartCoroutine(AnimateMultipleText(messages, displayForSeconds, textSpeed, messageGap, playTic));
        StopTextImmediate();
    }
    
    #endregion

    #region Animating Text Methods

    private IEnumerator AnimateText(string message, float customTextSpeed, bool playTic)
    {
        int i = 0;
        while (i < message.Length)
        {
            TalkingText.text += message[i];

            if (playTic)
            {
                if (!charsToPauseOn.Any(("" + message[i]).Contains))
                {
                    MusicManager.Instance.PlayOneShot(TalkSound);
                }
            }

            i++;
            yield return new WaitForSeconds(customTextSpeed);
        }
    }


    private IEnumerator AnimateMultipleText(string[] messages, float[] displayForSeconds, float customTextSpeed, float customMessageGap, bool playTic)
    {
        Coroutine cor = null;

        int i = 0;
        while (i < messages.Length)
        {

            IEnumerator ienum = AnimateText(messages[i], customTextSpeed, playTic);
            cor = StartCoroutine(ienum);

            while (ienum.MoveNext())
            {
                yield return StartCoroutine(WaitAndClear(displayForSeconds[i], cor));
                ClearTheText(cor);
                break;
            }

            yield return new WaitForSeconds(customMessageGap);
            i++;
        }
    }

    #endregion

    #region Text Handling Methods

    /// <summary>
    /// Stops the text immediately or after a specified number of seconds.
    /// </summary>
    /// <param name="afterSeconds">The after seconds.</param>
    public void StopText(float afterSeconds = 0.0f)
    {
        StartCoroutine(StopTheText(afterSeconds));
    }


    private IEnumerator StopTheText(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        StopTextImmediate();
    }


    private IEnumerator WaitAndClear(float second, Coroutine cor)
    {
        yield return new WaitForSeconds(second);
        ClearTheText(cor);
    }

    #endregion

    private void StopAllGhosts()
    {
        //foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost")) {
        // ghost.GetComponent<DialogueController>().StopNPCTalking();
        //}
    }
}
