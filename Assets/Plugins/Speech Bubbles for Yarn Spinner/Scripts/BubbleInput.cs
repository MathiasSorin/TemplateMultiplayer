namespace Yarn.Unity.Addons.SpeechBubbles
{
    using UnityEngine;
    using TMPro;
    using Yarn.Unity;
    using Yarn.Unity.Attributes;
    using Yarn.Markup;
    using System.Threading;
    using System.Collections.Generic;

#if USE_INPUTSYSTEM
    using UnityEngine.InputSystem;
#endif

#nullable enable

    public sealed class BubbleInput : MonoBehaviour, IActionMarkupHandler
    {
        private enum ControlMode
        {
            ControlledLineAdvancer, IndependentLineAdvancer, Basic
        }
        [SerializeField] private ControlMode controlMode = ControlMode.Basic;
        
        [SerializeField] BubbleDialogueView? view;
        [SerializeField] DialogueRunner? runner;

        [Space]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [SerializeField] private bool separateHurryUpAndAdvanceControls = false;
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [SerializeField] private bool multiAdvanceIsCancel = false;
        [ShowIf(nameof(multiAdvanceIsCancel))]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [SerializeField] private int advanceRequestsBeforeCancellingLine = 2;
        [Space]
        [SerializeField] LineAdvancer.InputMode inputMode = LineAdvancer.InputMode.KeyCodes;

        // these are the keycodes to be forwarded onto the line advancer
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] KeyCode hurryUpLineKeyCode = KeyCode.Space;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [ShowIf(nameof(separateHurryUpAndAdvanceControls))]
        [Indent]
        [SerializeField] KeyCode nextLineKeyCode = KeyCode.Escape;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] KeyCode hurryUpOptionsKeyCode = KeyCode.Space;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] KeyCode cancelDialogueKeyCode = KeyCode.None;

        // these are the keycodes that are bubble specific
        [Space]
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [Indent]
        [SerializeField] KeyCode nextOptionKey = KeyCode.RightArrow;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [Indent]
        [SerializeField] KeyCode prevOptionKey = KeyCode.LeftArrow;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [HideIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] KeyCode selectOptionKey = KeyCode.Space;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.KeyCodes)]
        [ShowIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] KeyCode advanceContentKey = KeyCode.Space;

#if USE_INPUTSYSTEM
        // these are the actions to be forwarded onto the line advancer
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] InputActionReference? hurryUpLineAction;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [ShowIf(nameof(separateHurryUpAndAdvanceControls))]
        [Indent]
        [SerializeField] InputActionReference? nextLineAction;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] InputActionReference? hurryUpOptionsAction;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] InputActionReference? cancelDialogueAction;

        // these are the actions specific to the bubble
        [Space]
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [Indent]
        [SerializeField] InputActionReference? nextOptionInput;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [Indent]
        [SerializeField] InputActionReference? prevOptionInput;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [HideIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] InputActionReference? selectOptionInput;

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.InputActions)]
        [ShowIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] InputActionReference? advanceContentInput;
#endif
        // these are the axes to be forwarded onto the line advancer
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] string hurryUpLineAxis = "Jump";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [ShowIf(nameof(separateHurryUpAndAdvanceControls))]
        [Indent]
        [SerializeField] string nextLineAxis = "Cancel";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] string hurryUpOptionsAxis = "Jump";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [ShowIf(nameof(controlMode), ControlMode.ControlledLineAdvancer)]
        [Indent]
        [SerializeField] string cancelDialogueAxis = "";

        // these are the axes specific to the bubble
        [Space]
        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [Indent]
        [SerializeField] string nextOptionAxisName = "Horizontal";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [HideIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] string selectionOptionButtonName = "Submit";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [ShowIf(nameof(controlMode), ControlMode.Basic)]
        [Indent]
        [SerializeField] string advanceContentButtonName = "Submit";

        [ShowIf(nameof(inputMode), LineAdvancer.InputMode.LegacyInputAxes)]
        [Indent]
        [SerializeField] float axisThreshold = 0.4f;

        private YarnTaskCompletionSource? changeOptionCompletionSource = null;
        private CancellationTokenSource? cancellationSource;
        private LineAdvancer? lineAdvancer;
        private bool isPresenting = false;
        private int frameStarted = 0;

        private bool isMovingBetweenOptions
        {
            get
            {
                return changeOptionCompletionSource != null && !changeOptionCompletionSource.Task.IsCompleted();
            }
        }

        void Start()
        {
            if (view != null)
            {
                view.Typewriter?.ActionMarkupHandlers.Add(this);
            }
            ConfigureAndEnableInputActions();
        }

        void OnEnable()
        {
            if (controlMode == ControlMode.ControlledLineAdvancer)
            {
                OnValidateInternal();
            }
        }
        void OnDisable()
        {
            DisableInputAction();
        }

        void OnValidate()
        {
#if UNITY_EDITOR
            // we're wrapping this call because otherwise unity will throw a warning up
            UnityEditor.EditorApplication.delayCall += ()=> { OnValidateInternal(); };
#endif
        }
        void OnValidateInternal()
        {
#if UNITY_EDITOR
            // it's possible because we are delaying the call this specific instance (the one in the editor) has been destroyed
            // but it still gets the delay call, so we need to make sure we are still around before we do anything
            // another casualty of unitys weird choice to override null checking
            if (this == null)
            {
                return;
            }

            // likewise if we are in playmode we don't want to change anything even if we are also in the editor
            if (Application.isPlaying)
            {
                return;
            }

            if (controlMode != ControlMode.ControlledLineAdvancer)
            {
                if (lineAdvancer != null)
                {
                    DestroyImmediate(lineAdvancer);
                }
                else if (TryGetComponent<LineAdvancer>(out lineAdvancer))
                {
                    DestroyImmediate(lineAdvancer);
                }
                return;
            }
            
            // ok so now we know we are Control mode
            // so now we need a line advancer, either by having one already or by making one
            if (lineAdvancer == null)
            {
                if (!TryGetComponent<LineAdvancer>(out lineAdvancer))
                {
                    lineAdvancer = this.gameObject.AddComponent<LineAdvancer>();
                }
            }
            lineAdvancer.hideFlags |= HideFlags.NotEditable;

            // ok now to grab into the guts of the line advancer and start to fuck with it
            var type = typeof(LineAdvancer);

            var translation = new Dictionary<string, object?>
            {
                { "presenter", view },
                { "runner", runner },
                { "inputMode", inputMode},
                { "separateHurryUpAndAdvanceControls", separateHurryUpAndAdvanceControls},
            };

            lineAdvancer.multiAdvanceIsCancel = multiAdvanceIsCancel;
            lineAdvancer.advanceRequestsBeforeCancellingLine = advanceRequestsBeforeCancellingLine;

            switch (inputMode)
            {
                case LineAdvancer.InputMode.KeyCodes:
                    translation.Add("hurryUpLineKeyCode", hurryUpLineKeyCode);
                    translation.Add("nextLineKeyCode", nextLineKeyCode);
                    translation.Add("hurryUpOptionsKeyCode", hurryUpOptionsKeyCode);
                    translation.Add("cancelDialogueKeyCode", cancelDialogueKeyCode);

                    break;
                case LineAdvancer.InputMode.LegacyInputAxes:
                    translation.Add("hurryUpLineAxis", hurryUpLineAxis);
                    translation.Add("nextLineAxis", nextLineAxis);
                    translation.Add("hurryUpOptionsAxis", hurryUpOptionsAxis);
                    translation.Add("cancelDialogueAxis", cancelDialogueAxis);

                    break;
                case LineAdvancer.InputMode.InputActions:
#if USE_INPUTSYSTEM
                    translation.Add("hurryUpLineAction", hurryUpLineAction);
                    translation.Add("nextLineAction", nextLineAction);
                    translation.Add("hurryUpOptionsAction", hurryUpOptionsAction);
                    translation.Add("cancelDialogueAction", cancelDialogueAction);
#endif
                    break;
            }

            foreach (var pair in translation)
            {
                var field = type.GetField(pair.Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (field == null)
                {
                    Debug.LogWarning($"unable to find the field {pair.Key} on Line Advancer");
                    continue;
                }
                if (pair.Value == null)
                {
                    // Debug.LogWarning($"Found the field {pair.Key} on the Line Advancer but it has no value associated with it");
                    continue;
                }
                field.SetValue(lineAdvancer, pair.Value);
            }
#endif
        }


#if USE_INPUTSYSTEM
        void ConfigureAndEnableInputActions()
        {
            if (inputMode != LineAdvancer.InputMode.InputActions)
            {
                // We're not using input actions, so nothing to do
                return;
            }
            if (nextOptionInput != null)
            {
                nextOptionInput.action.Enable();
                nextOptionInput.action.performed += NextOptionInputFired;
            }
            if (prevOptionInput != null)
            {
                prevOptionInput.action.Enable();
                prevOptionInput.action.performed += PrevOptionInputFired;
            }

            if (controlMode == ControlMode.Basic)
            {
                if (advanceContentInput != null)
                {
                    advanceContentInput.action.performed += AdvanceInput;
                    advanceContentInput.action.Enable();
                }
            }
            else
            {   
                if (selectOptionInput != null)
                {
                    selectOptionInput.action.performed += SelectOptionInput;
                    selectOptionInput.action.Enable();
                }
            }
        }
        void DisableInputAction()
        {
            if (inputMode != LineAdvancer.InputMode.InputActions)
            {
                return;
            }
            if (nextOptionInput != null)
            {
                nextOptionInput.action.performed -= NextOptionInputFired;
            }
            if (prevOptionInput != null)
            {
                prevOptionInput.action.performed -= PrevOptionInputFired;
            }
            if (selectOptionInput != null)
            {
                selectOptionInput.action.performed -= SelectOptionInput;
            }
            if (advanceContentInput != null)
            {
                advanceContentInput.action.performed -= AdvanceInput;
            }
        }
        private void SelectOptionInput(InputAction.CallbackContext context)
        {
            SelectCurrentOption();
        }
        private void PrevOptionInputFired(InputAction.CallbackContext context)
        {
            if (isMovingBetweenOptions)
            {
                ChangeOptionInternal(-1, true);
            }
            else
            {
                ChangeOptionInternal(-1, false);
            }
        }
        private void NextOptionInputFired(InputAction.CallbackContext context)
        {
            if (isMovingBetweenOptions)
            {
                ChangeOptionInternal(1, true);
            }
            else
            {
                ChangeOptionInternal(1, false);
            }
        }
#else
        void ConfigureAndEnableInputActions()
        {
            // this is a no-op just for being cleaner to call it when not in input systems code
        }
        void DisableInputAction()
        {
            // this is a no-op just for being cleaner to call it when not in input systems code
        }
#endif

        void Update()
        {
            if (inputMode == LineAdvancer.InputMode.KeyCodes)
            {
                if (InputSystemAvailability.GetKeyDown(nextOptionKey))
                {
                    if (isMovingBetweenOptions)
                    {
                        ChangeOptionInternal(1, true);
                    }
                    else
                    {
                        ChangeOptionInternal(1, false);
                    }
                    return;
                }
                if (InputSystemAvailability.GetKeyDown(prevOptionKey))
                {
                    if (isMovingBetweenOptions)
                    {
                        ChangeOptionInternal(-1, true);
                    }
                    else
                    {
                        ChangeOptionInternal(-1, false);
                    }
                    return;
                }
                if (controlMode == ControlMode.Basic)
                {
                    if (InputSystemAvailability.GetKeyDown(advanceContentKey))
                    {
                        // when in direct key/axis mode it's possible that the frame an action starts on will auto-cancel presentation
                        // so we just need to make sure it isn't the EXACT same frame
                        if (frameStarted != Time.frameCount)
                        {
                            AdvancePressed();
                        }
                    }
                }
                else
                {
                    if (InputSystemAvailability.GetKeyDown(selectOptionKey))
                    {
                        SelectCurrentOption();
                        return;
                    }
                }
            }
            else if (inputMode == LineAdvancer.InputMode.LegacyInputAxes)
            {
                var axis = InputSystemAvailability.GetAxis(nextOptionAxisName);

                if (axis > axisThreshold)
                {
                    ShowNextOption();
                }
                if (axis < -axisThreshold)
                {
                    ShowPreviousOption();
                }

                if (controlMode == ControlMode.Basic)
                {
                    if (Input.GetButtonUp(advanceContentButtonName))
                    {
                        // when in direct key/axis mode it's possible that the frame an action starts on will auto-cancel presentation
                        // so we just need to make sure it isn't the EXACT same frame
                        if (frameStarted != Time.frameCount)
                        {
                            AdvancePressed();
                        }
                    }
                }
                else
                {
                    if (Input.GetButtonUp(selectionOptionButtonName))
                    {
                        SelectCurrentOption();
                    }
                }
            }
        }

        private void AdvanceInput(InputAction.CallbackContext context)
        {
            AdvancePressed();
        }
        
        private void AdvancePressed()
        {
            if (view == null)
            {
                return;
            }
            if (runner == null)
            {
                return;
            }
            switch (view.CurrentContentType)
            {
                case BubbleDialogueView.ContentType.None:
                    return;
                case BubbleDialogueView.ContentType.Line:
                    if (isPresenting)
                    {
                        view.CancelBubblePresentation();
                    }
                    else
                    {
                        runner.RequestNextLine();
                    }
                    break;
                case BubbleDialogueView.ContentType.Options:
                    if (isPresenting)
                    {
                        view.CancelBubblePresentation();
                    }
                    else
                    {
                        view.SelectOption();
                    }
                    break;
            }
        }

        private async YarnTask ChangeOption(int direction)
        {
            // if this method is called and we don't have a completion source something has gone wrong and it needs to be resolved
            if (changeOptionCompletionSource == null)
            {
                return;
            }
            // likewise if the view is null we can't do anything
            if (view == null)
            {
                return;
            }
            if (cancellationSource == null)
            {
                return;
            }

            // we let the animation finish
            await view.ChangeOption(direction, cancellationSource.Token);

            // we've finished so we want to do some clean up
            // this will also be done later in case it is skipped here
            // but it feels good to be sure here
            changeOptionCompletionSource.TrySetResult();
            cancellationSource?.Dispose();
            cancellationSource = null;
        }

        private void ChangeOptionInternal(int direction, bool cancelCurrentPresentation)
        {
            // if we don't have a view OR it isn't showing options we can't change options
            if (view == null)
            {
                return;
            }
            if (view.CurrentContentType != BubbleDialogueView.ContentType.Options)
            {
                return;
            }

            // there is a change source already
            if (changeOptionCompletionSource != null)
            {
                // if it isn't finished then we just ignore this and move on
                if (!changeOptionCompletionSource.Task.IsCompleted())
                {
                    if (cancelCurrentPresentation)
                    {
                        cancellationSource?.Cancel();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            // likewise we will need to clean up the cancellationSource before going any further
            if (cancellationSource != null)
            {
                cancellationSource.Dispose();
            }

            // at this point now the changeOptionCompletionSource either doesn't exist or we need a fresh one regardless
            // likewise for the token source
            // building a new cancellation token and a completion source for this change
            cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            changeOptionCompletionSource = new YarnTaskCompletionSource();
            cancellationSource.Token.Register(() =>
            {
                changeOptionCompletionSource?.TrySetCanceled();
            });

            // finally we can now request the change
            ChangeOption(direction).Forget();
        }

        public void ShowNextOption()
        {
            ChangeOptionInternal(1, false);
        }
        public void ShowPreviousOption()
        {
            ChangeOptionInternal(-1, false);
        }
        public void SelectCurrentOption()
        {
            if (view == null)
            {
                return;
            }
            if (view.CurrentContentType == BubbleDialogueView.ContentType.Options)
            {
                view.SelectOption();
            }
        }

        public void OnLineDisplayComplete()
        {
            isPresenting = false;
            return;
        }
        public void OnPrepareForLine(MarkupParseResult line, TMP_Text text)
        {
            frameStarted = Time.frameCount;
            isPresenting = true;
        }
        public void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text) { return; }
        public YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken){ return YarnTask.CompletedTask; }
        public void OnLineWillDismiss() { return; }
    }
}