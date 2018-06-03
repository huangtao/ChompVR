using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractionController))]
public class InteractionEditor : Editor
{
    static bool showEdit = true;
    static bool startMessageFold;
    static bool interactionFold;

    static bool extraFunctionsFold;

    static bool showGoals = false;
    static bool showEvent = false;

    InteractionController t;
    SerializedObject GetTarget;
    int ListSize;


    void OnEnable()
    {
        t = (InteractionController)target;
        GetTarget = new SerializedObject(t);
    }


    public override void OnInspectorGUI()
    {
        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        SerializedProperty tm = GetTarget.FindProperty("tm"); // MyListRef.FindPropertyRelative("defaultTextspeed");

        SerializedProperty textSpeed = GetTarget.FindProperty("textSpeed");
        SerializedProperty messageGap = GetTarget.FindProperty("messageGap");
        SerializedProperty destroySelf = GetTarget.FindProperty("destroySelf");
        SerializedProperty triggerEvent = GetTarget.FindProperty("triggerEvent");
        SerializedProperty eventScript = GetTarget.FindProperty("eventScript");
        SerializedProperty eventMethod = GetTarget.FindProperty("eventMethod");
        SerializedProperty eventMethodParameter = GetTarget.FindProperty("eventMethodParameter");
        SerializedProperty loadNewScene = GetTarget.FindProperty("loadNewScene");
        SerializedProperty sceneNumberToLoad = GetTarget.FindProperty("sceneNumberToLoad");
        SerializedProperty regularInteractionMessage = GetTarget.FindProperty("regularInteractionMessage");
        SerializedProperty hasExtendedDialogue = GetTarget.FindProperty("hasExtendedDialogue");
        SerializedProperty extendedDialogueMessage = GetTarget.FindProperty("extendedDialogueMessage");
        SerializedProperty isStartMessage = GetTarget.FindProperty("isStartMessage");
        SerializedProperty startMessage = GetTarget.FindProperty("startMessage");
        SerializedProperty displayTime = GetTarget.FindProperty("displayTime");
        SerializedProperty extendedStartMessage = GetTarget.FindProperty("extendedStartMessage");
        SerializedProperty secondsBetweenEachElement = GetTarget.FindProperty("secondsBetweenEachElement");
        SerializedProperty howLongToDisablePlayerMovement = GetTarget.FindProperty("howLongToDisablePlayerMovement");
        SerializedProperty howLongToDisplayStartMessageFor = GetTarget.FindProperty("howLongToDisplayStartMessageFor");
        SerializedProperty enableScriptOnFinish = GetTarget.FindProperty("enableScriptOnFinish");
        SerializedProperty scriptToEnable = GetTarget.FindProperty("scriptToEnable");

        SerializedProperty playsSoundOnMessage = GetTarget.FindProperty("playsSoundOnMessage");
        SerializedProperty soundToPlayOnMessage = GetTarget.FindProperty("soundToPlayOnMessage");
        SerializedProperty soundToPlayOnGoal = GetTarget.FindProperty("soundToPlayOnGoal");
        SerializedProperty playSoundOnEnd = GetTarget.FindProperty("playSoundOnEnd");
        SerializedProperty soundToPlayOnEnd = GetTarget.FindProperty("soundToPlayOnEnd");
        SerializedProperty playSoundOnGoal = GetTarget.FindProperty("playSoundOnGoal");
        SerializedProperty playTicSound = GetTarget.FindProperty("playTicSound");
        SerializedProperty playTicSoundAtStart = GetTarget.FindProperty("playTicSoundAtStart");
        SerializedProperty playSoundAtTheStart = GetTarget.FindProperty("playSoundAtTheStart");
        SerializedProperty playSoundAfter = GetTarget.FindProperty("playSoundAfter");


        GUIStyle headerFoldOut = new GUIStyle(EditorStyles.foldout);
        headerFoldOut.fontStyle = FontStyle.Bold;
        headerFoldOut.fontSize = 13;
        headerFoldOut.normal.textColor = Color.black;
        headerFoldOut.active.textColor = Color.grey;
        headerFoldOut.onFocused.textColor = Color.black;

        headerFoldOut.onActive.textColor = Color.grey;
        headerFoldOut.focused.textColor = Color.black;

        //headerFoldOut.onNormal.textColor = Color.black;
        //headerFoldOut.hover.textColor = Color.black;
        //headerFoldOut.onHover.textColor = Color.black;


        //EditorGUILayout.PropertyField(debugonplay, new GUIContent("Debug on Play?", "Quickens things up a bit."));

        //if (Application.isPlaying) {
        //    EditorGUILayout.Space();
        //    EditorGUILayout.PropertyField(debug, new GUIContent("DEBUG", "Quickens things up a bit."));
        //    EditorGUILayout.Space();
        //    EditorGUILayout.Space();
        //}

        //if (debug.boolValue) {
        //    soundToPlay.objectReferenceValue = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/SFX/ding.mp3", typeof(AudioClip));
        //    secondsToWait.floatValue = 1f;
        //    specialExtendedDialogueMessage.arraySize = 1;
        //    specialExtendedDialogueMessage.GetArrayElementAtIndex(0).stringValue = "Debug Test! Extended!";
        //    secondsBetweenEachElement.arraySize = 1;
        //    secondsBetweenEachElement.GetArrayElementAtIndex(0).floatValue = 1f;

        //    specialStartMessageMessage.arraySize = 1;
        //    specialStartMessageMessage.GetArrayElementAtIndex(0).stringValue = "Debug Test! Start!";

        //    howLongToDisplayStartMessageFor.floatValue = 1f;
        //    howLongToDisablePlayerMovement.floatValue = 1f;
        //}

        EditorGUILayout.PropertyField(tm, new GUIContent("Talk Manager", ""));

        if (tm.objectReferenceValue != null)
        {
            showEdit = Foldout(showEdit, "Edit: ", true, EditorStyles.foldout);
        }
        else
        {
            showEdit = false;
        }

        if (showEdit)
        {
            EditorGUILayout.PropertyField(textSpeed, new GUIContent("Text Speed", "The speed at which the text will be displayed.\nHigher is slower."), GUILayout.MaxWidth(275));

            DrawLine();
            startMessageFold = Foldout(startMessageFold, "Start Message", true, headerFoldOut);

            if (startMessageFold)
            {
                DrawLine(true);
                EditorGUILayout.PropertyField(isStartMessage, new GUIContent("Play on Start", ""), GUILayout.MaxWidth(175));

                if (isStartMessage.boolValue)
                {
                    I();
                    EditorGUILayout.PropertyField(playsSoundOnMessage, new GUIContent("Play Sound", ""), GUILayout.MaxWidth(175));

                    if (playsSoundOnMessage.boolValue)
                    {
                        I();
                        EditorGUILayout.PropertyField(soundToPlayOnMessage, new GUIContent("Sound", ""));
                        if (soundToPlayOnMessage.objectReferenceValue != null)
                        {
                            EditorGUILayout.PropertyField(playSoundAtTheStart, new GUIContent("Play at Start?", ""));
                            if (!playSoundAtTheStart.boolValue)
                            {
                                I();
                                EditorGUILayout.PropertyField(playSoundAfter, new GUIContent("Play After", ""));
                                O();
                            }
                        }
                        O();
                    }

                    Space(1);
                    EditorGUILayout.PropertyField(howLongToDisablePlayerMovement, new GUIContent("Disable Player For", ""));
                    Space(1);

                    EditorGUILayout.PropertyField(hasExtendedDialogue, new GUIContent("Extended Message", ""), GUILayout.MaxWidth(175));

                    if (hasExtendedDialogue.boolValue)
                    {
                        I();
                        Space(2);
                        EditorGUILayout.PropertyField(extendedStartMessage, new GUIContent("Extended Start", ""), true);
                        extendedStartMessage.arraySize = extendedStartMessage.arraySize < 2 ? 2 : extendedStartMessage.arraySize;
                        EditorGUILayout.PropertyField(secondsBetweenEachElement, new GUIContent("Start Display Times", ""), true);
                        secondsBetweenEachElement.arraySize = extendedStartMessage.arraySize;
                        EditorGUILayout.PropertyField(playTicSoundAtStart, new GUIContent("Start Tic Sound", ""), true);
                        EditorGUILayout.PropertyField(messageGap, new GUIContent("Message Gap", ""), true);
                        O();
                    }
                    else
                    {


                        I();
                        EditorGUILayout.PropertyField(startMessage, new GUIContent("Start Message", ""));
                        EditorGUILayout.PropertyField(playTicSoundAtStart, new GUIContent("Start Tic Sound", ""), true);
                        EditorGUILayout.PropertyField(howLongToDisplayStartMessageFor, new GUIContent("Start Display Time", ""));
                        O();
                    }
                    O();
                }
                O();
                DrawLine();
            }
            else
            {
                DrawLine();
            }

            interactionFold = Foldout(interactionFold, "Interaction", true, headerFoldOut);
            if (interactionFold)
            {
                DrawLine(true);
                I();

                EditorGUILayout.PropertyField(playsSoundOnMessage, new GUIContent("Play Sound?", ""), GUILayout.MaxWidth(175));

                if (playsSoundOnMessage.boolValue)
                {
                    I();
                    EditorGUILayout.PropertyField(soundToPlayOnMessage, new GUIContent("Sound", ""));
                    EditorGUILayout.PropertyField(playSoundAtTheStart, new GUIContent("Play at Start?", ""));
                    if (!playSoundAtTheStart.boolValue)
                    {
                        I();
                        EditorGUILayout.PropertyField(playSoundAfter, new GUIContent("Play After", ""));
                        O();
                    }
                    O();
                }

                EditorGUILayout.PropertyField(hasExtendedDialogue, new GUIContent("Extended Message", ""));

                if (hasExtendedDialogue.boolValue)
                {
                    I();

                    I();
                    EditorGUILayout.PropertyField(extendedDialogueMessage, new GUIContent("Extended Dialogue", ""), true);
                    extendedDialogueMessage.arraySize = extendedDialogueMessage.arraySize < 2 ? 2 : extendedDialogueMessage.arraySize;
                    EditorGUILayout.PropertyField(secondsBetweenEachElement, new GUIContent("Seconds Between", ""), true);
                    secondsBetweenEachElement.arraySize = extendedDialogueMessage.arraySize;
                    EditorGUILayout.PropertyField(playTicSound, new GUIContent("Tic Sound", ""), true);
                    EditorGUILayout.PropertyField(messageGap, new GUIContent("Message Gap", ""), true);
                    O();
                    O();
                }
                else
                {


                    I();

                    EditorGUILayout.PropertyField(regularInteractionMessage, new GUIContent("Message", ""));
                    EditorGUILayout.PropertyField(playTicSound, new GUIContent("Tic Sound", ""), true);
                    EditorGUILayout.PropertyField(displayTime, new GUIContent("Display Time", ""));

                    O();
                }

                O();
                DrawLine();
            }
            else
            {
                DrawLine();
            }

            extraFunctionsFold = Foldout(extraFunctionsFold, "Extra", true, headerFoldOut);

            if (extraFunctionsFold)
            {
                I();
                DrawLine(true);

                showEvent = Foldout(showEvent, "Events: ", true, EditorStyles.foldout);

                if (showEvent)
                {
                    EditorGUILayout.PropertyField(triggerEvent, new GUIContent("Trigger Event", ""), GUILayout.MaxWidth(175));

                    if (triggerEvent.boolValue)
                    {
                        I();
                        EditorGUILayout.PropertyField(loadNewScene, new GUIContent("New Scene?", ""), GUILayout.MaxWidth(175));

                        if (loadNewScene.boolValue)
                        {
                            I();
                            EditorGUILayout.PropertyField(sceneNumberToLoad, new GUIContent("Scene #", ""), GUILayout.MaxWidth(175));
                            O();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(eventScript, new GUIContent("Event Script", ""));
                            if (eventScript.objectReferenceValue != null)
                            {
                                EditorGUILayout.PropertyField(eventMethod, new GUIContent("Event Method", ""));
                                EditorGUILayout.PropertyField(eventMethodParameter, new GUIContent("Method Parameter", ""));
                            }
                        }
                        O();
                    }
                    Space(3);
                    EditorGUILayout.PropertyField(playSoundOnEnd, new GUIContent("End Sound?", ""), GUILayout.MaxWidth(175));

                    if (playSoundOnEnd.boolValue)
                    {
                        I();
                        EditorGUILayout.PropertyField(soundToPlayOnEnd, new GUIContent("End Sound", ""));
                        O();
                    }
                    Space(3);
                    EditorGUILayout.PropertyField(enableScriptOnFinish, new GUIContent("Enable Script?", ""), GUILayout.MaxWidth(175));

                    if (enableScriptOnFinish.boolValue)
                    {
                        I();
                        EditorGUILayout.PropertyField(scriptToEnable, new GUIContent("Script to Enable", ""));
                        O();
                    }
                    Space(3);
                }
                EditorGUILayout.PropertyField(destroySelf, new GUIContent("Destroy Self?", ""), GUILayout.MaxWidth(175));
                O();
                DrawLine();
            }
            else
            {
                DrawLine();
            }

            //EditorGUILayout.Space();

            //EditorGUILayout.PropertyField(textSpeed, new GUIContent("Text Speed", "The speed at which the text will be displayed.\nHigher is slower."), GUILayout.MaxWidth(175));

            //EditorGUILayout.PropertyField(cantSkip, new GUIContent("Can't Skip", "Can we skip this message? Checked means we can't."));

            //showKeys = Foldout(showKeys, "User Interaction: ", true, EditorStyles.foldout);

            //if (showKeys) {
            //    EditorGUILayout.PropertyField(directionsThatCauseInteractionBits, new GUIContent("Interaction Directions", "Directions that read key presses."), GUILayout.MaxWidth(200));
            //    EditorGUILayout.PropertyField(keyToTriggerTalk, new GUIContent("Key", "Key to press for interaction."));
            //}

            //EditorGUILayout.PropertyField(isStartMessage, new GUIContent("Play on Start", "This message will play at the start of the scene. Does not require interaction from player."));
            //if (isStartMessage.boolValue) {
            //    EditorGUILayout.PropertyField(howLongToDisplayStartMessageFor, new GUIContent("Start Length", "How long the start message will be displayed for."), GUILayout.MaxWidth(175));
            //    EditorGUILayout.PropertyField(howLongToDisablePlayerMovement, new GUIContent("Disable Player", "How many seconds the player will be disabled for."), GUILayout.MaxWidth(175));
            //} else {

            //    if (cantSkip.boolValue) {
            //        EditorGUILayout.PropertyField(displayTime, new GUIContent("Display Time", "If you can't skip the message, we need to know how long it displays for."));
            //    } else {
            //        EditorGUILayout.PropertyField(keyForSkip, new GUIContent("Skip Key", "Key to press to end or move on to the next message."));
            //    }
            //}

            //EditorGUILayout.PropertyField(playsSoundOnMessage, new GUIContent("Play a Sound?", "Play a sound on interaction?"));
            //if (playsSoundOnMessage.boolValue) {
            //    EditorGUILayout.PropertyField(soundToPlayOnMessage, new GUIContent("Sound", "Optional sound to play on interation."));
            //}

            //EditorGUILayout.PropertyField(hasExtendedDialogue, new GUIContent("Extended Dialogue?", "Use this if you have dialog that is more than just a page. For conversational scenes."));

            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            //showGoals = Foldout(showGoals, "Goal Options: ", true, EditorStyles.foldout);

            //if (showGoals) {
            //    if (!completeGoal.boolValue) {
            //        EditorGUILayout.PropertyField(createGoal, new GUIContent("Create a Goal?", "This will create a goal for the user."));
            //    }

            //    if (createGoal.boolValue) {
            //        EditorGUILayout.PropertyField(goalText, new GUIContent("Goal Text", "The text that will be displayed for the goal."));
            //        EditorGUILayout.PropertyField(goalHideTime, new GUIContent("Hide After", "The number of seconds goal stays on screen."));
            //        EditorGUILayout.PropertyField(soundToPlayOnGoal, new GUIContent("Goal Sound", "Sound to notify the user there is a new goal."));
            //    } else {
            //        EditorGUILayout.PropertyField(completeGoal, new GUIContent("Complete a Goal?", "This will complete a goal on dialogue finish."));
            //    }

            //    if (createGoal.boolValue || completeGoal.boolValue) {
            //        //EditorGUILayout.PropertyField(goal, new GUIContent("Goal", "The goal that this object will modify (create or complete)."));
            //    }

            //    EditorGUILayout.Space();

            //}

            //EditorGUILayout.Space();

            //if (isStartMessage.boolValue && hasExtendedDialogue.boolValue) {
            //    EditorGUILayout.Space();
            //    EditorGUILayout.Space();
            //    EditorGUILayout.Space();

            //    EditorGUILayout.PropertyField(extendedStartMessage, new GUIContent("Start Message", "The extended message that is displayed at the start!"), true);

            //    EditorGUILayout.PropertyField(secondsBetweenEachElement, new GUIContent("Seconds Between", "How long to wait until the next element displays."), true);

            //} else {
            //    if (hasExtendedDialogue.boolValue) {
            //        EditorGUILayout.PropertyField(extendedDialogueMessage, new GUIContent("Extended Message", "Use this extended messge for longer texts that will exceed the box like conversations or long speeches."), true);

            //        if (cantSkip.boolValue) {
            //            EditorGUILayout.PropertyField(secondsBetweenEachElement, new GUIContent("Seconds Between", "How long to wait until the next element displays."), true);
            //        }

            //    } else {
            //        EditorGUILayout.Space();
            //        EditorGUILayout.PropertyField(useRandom, new GUIContent("Use Random Msg?", "Use random messages?"));
            //        EditorGUILayout.Space();

            //        if (useRandom.boolValue) {
            //            randomMessages.arraySize = randomMessages.arraySize < 2 ? 2 : randomMessages.arraySize;
            //            EditorGUILayout.PropertyField(randomMessages, new GUIContent("Random Messages", "Little messages that will randomly play."), true);
            //        } else {
            //            if(isStartMessage.boolValue) {
            //                EditorGUILayout.PropertyField(startMessage, new GUIContent("Start Message", "Single page start message."), true);
            //            } else {
            //                EditorGUILayout.PropertyField(regular4DirectionalInteractionMessages, new GUIContent("Object Message", "Short little blib for objects that only laste a line or two. The elements correlate to the directions (e.g. Element 0 will play for Up since it is first)."), true);
            //            }
            //        }
            //    }
            //}
            //EditorGUILayout.Space();
        }


        //EditorGUILayout.Space();
        //EditorGUILayout.Space();

        //showEvent = Foldout(showEvent, "Event: ", true, EditorStyles.foldout);
        //if (showEvent) {
        //    EditorGUILayout.PropertyField(triggerEvent, new GUIContent("Triggers Event", "Whether or not this object will trigger an event."), GUILayout.MaxWidth(175));

        //    if (triggerEvent.boolValue) {
        //        EditorGUILayout.PropertyField(loadNewScene, new GUIContent("Scene Switch?", "Will the event invlove changing to a different scene?"));

        //        if (loadNewScene.boolValue) {
        //            EditorGUILayout.PropertyField(sceneNumberToLoad, new GUIContent("Scene #", "Number ID of the scene to load."));
        //        } else {
        //            EditorGUILayout.PropertyField(eventScript, new GUIContent("Script", "The script which contains the event to trigger."));
        //            EditorGUILayout.PropertyField(eventMethod, new GUIContent("Method", "The method in the script which contains the code to execute."));
        //            EditorGUILayout.PropertyField(eventMethodParameter, new GUIContent("Parameter", "This sends a parameter to the target method."));
        //        }
        //    }
        //}

        //EditorGUILayout.Space();
        //EditorGUILayout.Space();

        //EditorGUILayout.PropertyField(playSoundOnEnd, new GUIContent("Play Sound at End?", "Plays a sound at the end of the interation."));

        //if (playSoundOnEnd.boolValue) {
        //    EditorGUILayout.PropertyField(soundToPlayOnEnd, new GUIContent("End Sound", "This sound will play at the end."));
        //}

        //EditorGUILayout.Space();
        //EditorGUILayout.Space();


        //EditorGUILayout.PropertyField(enableScriptOnFinish, new GUIContent("Enable Script?", "Enable an existing script on the current GameObject after this script completes."));

        //if (enableScriptOnFinish.boolValue) {
        //    EditorGUILayout.PropertyField(scriptToEnable, new GUIContent("Script to Enable", "This script will be enabled at the end."));
        //}

        //EditorGUILayout.Space();

        //if (!isStartMessage.boolValue) {
        //    EditorGUILayout.PropertyField(destroySelf, new GUIContent("Destroy on Finish?", "This will cause the script to destroy itself after its job has been completed."));
        //} else {
        //    EditorGUILayout.LabelField("Cannot destroy self because it's a start msg!");
        //    destroySelf.boolValue = false;
        //}

        //EditorGUILayout.Space();

        GetTarget.ApplyModifiedProperties();
    }


    public static void I()
    {
        EditorGUI.indentLevel++;
    }


    public static void O()
    {
        EditorGUI.indentLevel--;
    }


    public static bool[] Populate(bool[] arr, bool value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
        return arr;
    }


    public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
    {
        Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
        return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
    }


    public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
    {
        return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
    }


    public static void DrawLine(bool large = false, int height = 1)
    {
        if (large)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        else
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
        }
    }


    public static void Space(int spaces)
    {
        for (int i = 0; i < spaces; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
