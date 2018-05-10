using System.Collections.Generic;
using System.Linq;

namespace STEM.Model
{
    public class Temple
    {
        private readonly string _value;

        private readonly List<Style> _styles;

        public Temple(string value, List<Style> styles)
        {
            _value = value;
            _styles = styles;
        }

        public bool IsTempleOk()
        {
            string tempValue = _value;

            if (string.IsNullOrWhiteSpace(tempValue) || tempValue.Length % 2 == 1)
            {
                return false;
            }

            foreach (var c in tempValue)
            {
                if (!_styles.Any(x => x.Open == c || x.Close == c))
                {
                    return false;
                }
            }

            foreach (var style in _styles)
            {
                if (tempValue.Count(x => x.Equals(style.Open)) != tempValue.Count(x => x.Equals(style.Close)))
                {
                    return false;
                }
            }

            while (tempValue.Any())
            {
                bool removed = false;

                for (int i = 0; i < tempValue.Length - 1; i++)
                {
                    if (_styles.Any(x => x.Open == tempValue[i] && x.Close == tempValue[i + 1]))
                    {
                        tempValue = tempValue.Remove(i, 2);
                        removed = true;
                        break;
                    }
                }

                if (!removed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}