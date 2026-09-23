using UnityEngine;

namespace JyotisSugata.Core.Input
{
    public class InputReaderInitializer : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        private void Awake()
        {
            if (_inputReader != null)
            {
                _inputReader.Initialize();
            }
            else
            {
                Debug.LogWarning("InputReader is not assigned to InputReaderInitializer!");
            }
        }
    }
}
