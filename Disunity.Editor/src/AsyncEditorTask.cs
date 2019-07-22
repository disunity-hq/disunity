using System;
using System.Collections;

using UnityEditor;

using UnityEngine;


namespace Disunity.Editor {

    public static class AsyncEditorTask {

        public static void Start(IEnumerator update, Action end = null) {
            void Callback() {
                try {
                    if (update.MoveNext() != false) {
                        return;
                    }

                    end?.Invoke();

                    if ((EditorApplication.CallbackFunction) Callback != null) {
                        EditorApplication.update -= Callback;
                    }
                }
                catch (Exception ex) {
                    end?.Invoke();
                    Debug.LogException(ex);

                    if ((EditorApplication.CallbackFunction) Callback != null) {
                        EditorApplication.update -= Callback;
                    }
                }
            }

            EditorApplication.update += Callback;
        }

    }

}