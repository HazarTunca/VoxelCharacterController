using UnityEngine;
using TMPro;

namespace PBM
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsDisplay;

        private void Start()
        {
            Application.targetFrameRate = 60;
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
