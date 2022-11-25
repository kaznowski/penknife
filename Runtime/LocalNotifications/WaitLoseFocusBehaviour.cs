using System.Threading.Tasks;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for waiting until the application loses focus, allowing notifications on WaitLoseFocus mode to be scheduled.
    /// </summary>
    public class WaitLoseFocusBehaviour : MonoBehaviour
    {
        private static WaitLoseFocusBehaviour _instance;

        private bool _hasFocus = true;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _hasFocus = hasFocus;
        }

        /// <summary>
        /// Pauses execution until the application loses focus.
        /// </summary>
        public static async Task WaitLoseFocus()
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(WaitLoseFocusBehaviour)).AddComponent<WaitLoseFocusBehaviour>();
                DontDestroyOnLoad(_instance);
            }

            await _instance.InternalWaitLoseFocus();
        }

        private async Task InternalWaitLoseFocus()
        {
            while (_hasFocus)
            {
                await Task.Delay(1);
            }

            _instance = null;
            Destroy(gameObject);
        }
    }
}
