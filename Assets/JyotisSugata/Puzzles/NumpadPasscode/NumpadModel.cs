using System;
using System.Text;

namespace JyotisSugata.Puzzles.NumpadPasscode
{
    public class NumpadModel
    {
        private string _secretCode;
        private StringBuilder _enteredCode = new StringBuilder();
        private int _maxDigits;

        public event Action<string> OnDisplayChanged;
        public event Action OnCodeCorrect;
        public event Action OnCodeIncorrect;

        public string SecretCode => _secretCode;

        public void Initialize(int digitCount)
        {
            _maxDigits = digitCount;
            _enteredCode.Clear();
            _secretCode = GenerateRandomCode(digitCount);
            
            UnityEngine.Debug.Log($"[DEV] Numpad Code: {_secretCode}");
            
            OnDisplayChanged?.Invoke(GetDisplayString());
        }

        public void InputDigit(int digit)
        {
            if (_enteredCode.Length >= _maxDigits) return;
            
            _enteredCode.Append(digit.ToString());
            OnDisplayChanged?.Invoke(GetDisplayString());
            
            if (_enteredCode.Length == _maxDigits)
            {
                ValidateCode();
            }
        }

        public void DeleteLastDigit()
        {
            if (_enteredCode.Length == 0) return;
            
            _enteredCode.Remove(_enteredCode.Length - 1, 1);
            OnDisplayChanged?.Invoke(GetDisplayString());
        }

        private void ValidateCode()
        {
            if (_enteredCode.ToString() == _secretCode)
            {
                OnCodeCorrect?.Invoke();
            }
            else
            {
                OnCodeIncorrect?.Invoke();
                _enteredCode.Clear();
                OnDisplayChanged?.Invoke(GetDisplayString());
            }
        }

        private string GenerateRandomCode(int digits)
        {
            Random rand = new Random(Environment.TickCount);
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < digits; i++)
            {
                code.Append(rand.Next(0, 10).ToString());
            }
            return code.ToString();
        }

        private string GetDisplayString()
        {
            StringBuilder display = new StringBuilder();
            
            for (int i = 0; i < _enteredCode.Length; i++)
            {
                display.Append(_enteredCode[i]);
                if (i < _maxDigits - 1) display.Append(" ");
            }
            
            for (int i = _enteredCode.Length; i < _maxDigits; i++)
            {
                display.Append("_");
                if (i < _maxDigits - 1) display.Append(" ");
            }
            
            return display.ToString();
        }
    }
}
