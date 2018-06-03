using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractionController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TalkManager tm;

    private TalkManager TM
    {
        get
        {
            if (tm == null)
            {
                tm = TalkManager.Instance;
            }
            return tm;
        }
    }

    #region Default and Custom Message Options
    private const float defaultTextSpeed = .1f;
    private const float defaultDispTime = 5f;
    private const float defaultMsgGap = .25f;

    [SerializeField]
    private float textSpeed = defaultTextSpeed;

    private float TextSpeed
    {
        get
        {
            if (textSpeed <= 0)
            {
                Debug.LogWarning("textSpeed cannot be <= 0. Reseting value to default.");
                textSpeed = defaultTextSpeed;
            }
            return textSpeed;
        }
    }

    [SerializeField]
    private float displayTime = defaultDispTime;

    private float DisplayTime
    {
        get
        {
            if (displayTime < 0)
            {
                Debug.LogError("displayTime is not defined! Reset to default.");
                displayTime = defaultDispTime;
            }
            return displayTime;
        }
    }

    [SerializeField]
    private float messageGap = defaultMsgGap;

    private float MessageGap
    {
        get
        {
            if (messageGap < 0)
            {
                Debug.LogError("messageGap is not defined!");
                messageGap = defaultMsgGap;
            }
            return messageGap;
        }
    }
    #endregion


    #region Event Setup
    [SerializeField]
    private bool triggerEvent;

    [SerializeField]
    private MonoBehaviour eventScript;

    [SerializeField]
    private string eventMethod;

    [SerializeField]
    private UnityEngine.Object eventMethodParameter;
    #endregion

    #region Scene Transfer Setup
    [SerializeField]
    private bool loadNewScene;

    [SerializeField]
    private int sceneNumberToLoad;
    #endregion

    #region Messages Setup
    #region Regular Directional Interaction
    [SerializeField]
    [TextArea(3, 10)]
    private string regularInteractionMessage;
    #endregion

    #region Extended Dialogue
    [SerializeField]
    private bool hasExtendedDialogue;

    [SerializeField]
    [TextArea(3, 10)]
    private string[] extendedDialogueMessage = new string[1];
    #endregion

    #region Start Message
    [SerializeField]
    private bool isStartMessage;

    [SerializeField]
    [TextArea(3, 10)]
    private string startMessage;

    [SerializeField]
    [TextArea(3, 10)]
    private string[] extendedStartMessage = new string[2];

    [SerializeField]
    private float[] secondsBetweenEachElement = new float[2];

    [SerializeField]
    private float howLongToDisablePlayerMovement;

    [SerializeField]
    private float howLongToDisplayStartMessageFor;
    #endregion

    #region Script Enable
    [SerializeField]
    private bool enableScriptOnFinish;

    [SerializeField]
    private MonoBehaviour scriptToEnable;
    #endregion

    #region Random Messages
    [SerializeField]
    private bool useRandom;

    [SerializeField]
    [TextArea(3, 10)]
    private string[] randomMessages = new string[2];
    #endregion

    [SerializeField]
    bool destroySelf;

    private int previousMessage = 1;
    #endregion

    #region Sound Setup
    [SerializeField]
    private bool playsSoundOnMessage;

    [SerializeField]
    private Sound soundToPlayOnMessage;

    [SerializeField]
    private bool playSoundAtTheStart = true;

    [SerializeField]
    private float playSoundAfter;

    [SerializeField]
    private bool playSoundOnGoal;

    [SerializeField]
    private Sound soundToPlayOnGoal;

    [SerializeField]
    private bool playSoundOnEnd;

    [SerializeField]
    private Sound soundToPlayOnEnd;

    [SerializeField]
    private bool playTicSound;

    [SerializeField]
    private bool playTicSoundAtStart;
    #endregion

    #endregion


    private void Awake()
    {
        if (isStartMessage)
        {
            PlayMessageSound(playSoundAtTheStart);

            StartCoroutine(DisablePlayerMovementOnStart());

            if (hasExtendedDialogue)
            {
                StartCoroutine(DisplayMultipleFor(extendedStartMessage, secondsBetweenEachElement, MessageGap, playTicSoundAtStart));
            }
            else
            {
                StartCoroutine(DisplayMessageFor(startMessage, howLongToDisplayStartMessageFor, playTicSoundAtStart));
            }
        }
    }


    void OnTriggerStay(Collider col)
    {
        if ("Player".Equals(col.tag))
        {
            if (!PlayerController.Instance.IsDisplayingTutorialMessage)
            {
                Interact();
            }
        }
    }


    private void Interact()
    {
        PlayMessageSound(playSoundAtTheStart);

        if (hasExtendedDialogue)
        {
            StartCoroutine(DisplayMultipleFor(extendedDialogueMessage, secondsBetweenEachElement, MessageGap, playTicSound));
        }
        else
        {
            StartCoroutine(DisplayMessageFor(regularInteractionMessage, DisplayTime, playTicSound));
        }
    }


    private void PlayMessageSound(bool playAtStart)
    {
        if (playsSoundOnMessage)
        {
            if (playAtStart)
            {
                MusicManager.Instance.PlayOneShot(soundToPlayOnMessage);
            }
            else
            {
                StartCoroutine(PlayDelayedMessageSound(playSoundAfter));
            }
        }
    }


    private IEnumerator PlayDelayedMessageSound(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        MusicManager.Instance.PlayOneShot(soundToPlayOnMessage);
    }


    private IEnumerator DisablePlayerMovementOnStart()
    {
        yield return new WaitForSeconds(howLongToDisablePlayerMovement);
        PlayerController.Instance.IsDisplayingTutorialMessage = false;
    }


    private IEnumerator DisplayMultipleFor(string[] extendedMsg, float[] secondsBetween, float msgGap, bool ticSound)
    {
        yield return TM.DisplayMultipleFor(extendedMsg, secondsBetween, TextSpeed, msgGap, ticSound);
        CloseMessage();
    }


    private IEnumerator DisplayMessageFor(string message, float displayTime, bool playTic)
    {
        yield return TM.DisplayFor(message, displayTime, TextSpeed, playTic);
        CloseMessage();
    }


    private void CloseMessage()
    {
        if (triggerEvent)
        {
            if (loadNewScene)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumberToLoad);
            }
            else
            {
                eventScript.SendMessage(eventMethod, eventMethodParameter);
            }
        }

        if (playSoundOnEnd)
        {
            MusicManager.Instance.PlayOneShot(soundToPlayOnEnd);
        }

        if (enableScriptOnFinish)
        {
            scriptToEnable.enabled = true;
        }

        ApplicationModel.GameState = GameState.Playing;

        if (destroySelf)
        {
            Destroy(this);
            return;
        }
    }
}
