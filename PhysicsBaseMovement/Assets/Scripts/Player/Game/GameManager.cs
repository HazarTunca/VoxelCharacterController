using UnityEngine;
using TMPro;

namespace HzrController
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsDisplay;

        private void Start()
        {
            Application.targetFrameRate = 240;
        }

        private void Update()
        {
            CountFPS();
        }

        private void CountFPS()
        {
            float fps = 1 / Time.unscaledDeltaTime;
            _fpsDisplay.text = fps.ToString();
        }
    }
}
