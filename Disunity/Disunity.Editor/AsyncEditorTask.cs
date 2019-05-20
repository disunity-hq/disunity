using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


namespace Disunity.Editor {

    public static class AsyncEditorTask {

        public static void Start(IEnumerator update, Action end = null) {
            EditorApplication.CallbackFunction closureCallback = null;

            closureCallback = () => {
                try {
                    if (update.MoveNext() != false) {
                        return;
                    }

                    end?.Invoke();
                    if (closureCallback != null) {
                        EditorApplication.update -= closureCallback;
                    }
                }
                catch (Exception ex) {
                    end?.Invoke();
                    Debug.LogException(ex);
                    if (closureCallback != null) {
                        EditorApplication.update -= closureCallback;
                    }
                }
            };

            EditorApplication.update += closureCallback;
        }

    }
}
