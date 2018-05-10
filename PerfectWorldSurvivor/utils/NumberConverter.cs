using PerfectWorldSurvivor.Common;

namespace PerfectWorldSurvivor.Utils
{
    public class NumberConverter
    {
        public static float ConvertPerfectCharBufferToFloat(CharBuffer buffer)
        {
            return ConvertCharArrayToFloat(buffer.charArray, buffer.startPosition, buffer.endPosition);
        }
       
        public static float ConvertCharArrayToFloat(char[] charArray, int start, int end)
        {
            if (!_CheckCharArrayValid(charArray, start, end))
            {
                return 0;
            }
            int sign = 1;
            if (charArray[start] == _negativeSignChar)
            {
                sign = -1;
                start++;
            }
            int dotIndex = start;
            float later = 0;
            int former = 0;
            int countI = 1;
            for (; dotIndex <= end; dotIndex++)
            {
                if (charArray[dotIndex] == _dotChar)
                {
                    break;
                }
            }
            for (int i = dotIndex - 1; i >= start; i--)
            {
                former += ((charArray[i] - _numberZeroChar)) * countI;
                countI *= 10;
            }
            float count = 10f;
            for (int i = dotIndex + 1; i <= end; i++)
            {
                later += ((charArray[i] - _numberZeroChar)) / count;
                count *= 10;
            }
            return sign * (former + later);
        }

        public static int ConvertCharArrayToInt(char[] charArray, int start, int end)
        {
            if (!_CheckCharArrayValid(charArray, start, end))
            {
                return 0;
            }

            int sign = 1;
            if (charArray[start] == _negativeSignChar)
            {
                sign = -1;
                start++;
            }
            int former = 0;
            int countI = 1;
            for (int i = end; i >= start; i--)
            {
                former += ((charArray[i] - _numberZeroChar)) * countI;
                countI *= 10;
            }
            return sign * former;
        }

        private static bool _CheckCharArrayValid(char[] charArray, int start, int end)
        {
            if (charArray == null)
            {
                return false;
            }
            int arrLen = charArray.Length;
            if (start < 0 || start >= arrLen)
            {
                return false;
            }
            if (end < 0 || end >= arrLen)
            {
                return false;
            }
            if (end < start)
            {
                return false;
            }
            return true;
        }

        private static readonly char _dotChar = '.';

        private static readonly char _negativeSignChar = '-';

        private static readonly char _numberZeroChar = '0';
    }
}
