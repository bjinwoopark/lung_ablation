﻿using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    public class GestureManager : Singleton<GestureManager>
    {
        // Tap and Navigation gesture recognizer.
        public GestureRecognizer NavigationRecognizer { get; private set; }

        // Manipulation gesture recognizer.
        public GestureRecognizer ManipulationRecognizer { get; private set; }

        // Currently active gesture recognizer.
        public GestureRecognizer ActiveRecognizer { get; private set; }

        public bool IsNavigating { get; private set; }

        public Vector3 NavigationPosition { get; private set; }

        public bool IsManipulating { get; private set; }

        public Vector3 ManipulationPosition { get; private set; }

        void Awake()
        {

            // ROTATION Instantiate the NavigationRecognizer.
            NavigationRecognizer = new GestureRecognizer();

            // ROTATION Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
            NavigationRecognizer.SetRecognizableGestures(
                GestureSettings.Tap |
                GestureSettings.NavigationX |
                GestureSettings.NavigationY |
                GestureSettings.NavigationZ);

            // ROTATION Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
            NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
            // ROTATION Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
            NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
            // ROTATION Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
            NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
            // ROTATION Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
            NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
            // ROTATION Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
            NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

            // Instantiate the ManipulationRecognizer.
            ManipulationRecognizer = new GestureRecognizer();

            // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
            ManipulationRecognizer.SetRecognizableGestures(
                GestureSettings.ManipulationTranslate);

            // Register for the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

            ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            // ROTATION Unregister the Tapped and Navigation events on the NavigationRecognizer.
            NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;

            NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
            NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
            NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
            NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

            // ROTATION Unregister the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
        }

        /// <summary>
        /// Revert back to the default GestureRecognizer.
        /// </summary>
        public void ResetGestureRecognizers()
        {
            // Default to the navigation gestures.
            Transition(NavigationRecognizer);
        }

        /// <summary>
        /// Transition to a new GestureRecognizer.
        /// </summary>
        /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (ActiveRecognizer != null)
            {
                if (ActiveRecognizer == newRecognizer)
                {
                    return;
                }

                ActiveRecognizer.CancelGestures();
                ActiveRecognizer.StopCapturingGestures();
            }

            newRecognizer.StartCapturingGestures();
            ActiveRecognizer = newRecognizer;
        }

        private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // Set IsNavigating to be true.
            IsNavigating = true;

            // Set NavigationPosition to be relativePosition.
            NavigationPosition = relativePosition;
        }

        private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // Set IsNavigating to be true.
            IsNavigating = true;

            // Set NavigationPosition to be relativePosition.
            NavigationPosition = relativePosition;
        }

        private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // Set IsNavigating to be false.
            IsNavigating = false;
        }

        private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // Set IsNavigating to be false.
            IsNavigating = false;
        }

        private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsManipulating = true;

                ManipulationPosition = position;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);
            }
        }

        private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            if (HandsManager.Instance.FocusedGameObject != null)
            {
                IsManipulating = true;

                ManipulationPosition = position;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);
            }
        }

        private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = false;
        }

        private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = false;
        }

        private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
        {
            GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnSelect");
            }
        }
    }
}