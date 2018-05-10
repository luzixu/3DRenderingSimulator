using System.Collections.Generic;
using System.Text;
using PerfectWorldSurvivor.Utils;

namespace PerfectWorldSurvivor.Common
{
    public struct CharBuffer
    {
        public char[] charArray;

        public int startPosition;

        public int endPosition;

        public char GetIndex(int index)
        {
            int absoluteIndex = startPosition + index;
            if (absoluteIndex >= 0 && absoluteIndex < charArray.Length)
            {
                return charArray[absoluteIndex];
            }
            Logger.Log("index is out of range in method GetIndex(int index) from struct CharBuffer");
            return '0';
        }
      
        public int Length()
        {
            return endPosition - startPosition + 1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = startPosition; i <= endPosition; i++)
            {
                sb.Append(charArray[i]);
            }
            return sb.ToString();
        }

        public static void Parse(char[] array, int start, int end, List<CharBuffer> contentBuffer)
        {
            if (!_CheckParasValid(array,start,end))
            {
                return;
            }
            if (start <= end && _IsBlank(array[start]))
            {
                start++;
            }
            int pre = start;
            for (int i = start + 1; i <= end; i++)
            {
                if (_IsBlank(array[i]))
                {
                    CharBuffer perfectCharBuffer = new CharBuffer();
                    perfectCharBuffer.charArray = array;
                    perfectCharBuffer.startPosition = pre;
                    perfectCharBuffer.endPosition = i - 1;
                    contentBuffer.Add(perfectCharBuffer);
                    i++;
                    pre = i;
                    if (i <= end && _IsBlank(array[i]))
                    {
                        i++;
                        pre = i;
                    }
                }
            }
            if (pre <= end)
            {
                CharBuffer perfectCharBuffer = new CharBuffer();
                perfectCharBuffer.charArray = array;
                perfectCharBuffer.startPosition = pre;
                perfectCharBuffer.endPosition = end;
                contentBuffer.Add(perfectCharBuffer);
            }
        }
       
        public static List<CharBuffer> Split(char split, char[] array, int start, int end)
        {
            if (!_CheckParasValid(array, start, end))
            {
                return null;
            }

            List<CharBuffer> result = new List<CharBuffer>();
            if (start <= end && _IsSplit(split, array[start]))
            {
                start++;
            }
            int pre = start;
            for (int i = start + 1; i <= end; i++)
            {
                if (_IsSplit(split, array[i]))
                {
                    CharBuffer perfectCharBuffer = new CharBuffer();
                    perfectCharBuffer.charArray = array;
                    perfectCharBuffer.startPosition = pre;
                    perfectCharBuffer.endPosition = i - 1;
                    result.Add(perfectCharBuffer);
                    i++;
                    pre = i;
                    if (i <= end && _IsSplit(split, array[i]))
                    {
                        i++;
                        pre = i;
                    }
                }
            }
            if (pre <= end)
            {
                CharBuffer perfectCharBuffer = new CharBuffer();
                perfectCharBuffer.charArray = array;
                perfectCharBuffer.startPosition = pre;
                perfectCharBuffer.endPosition = end;
                result.Add(perfectCharBuffer);
            }
            return result;
        }
        private static bool _CheckParasValid(char[] array, int start, int end)
        {
            if (array == null)
            {
                return false;
            }
            int arrLen = array.Length;
            if (start < 0 || start >= arrLen || end < 0 || end >= arrLen)
            {
                return false;
            }
            if (start > end)
            {
                return false;
            }
            return true;
        }
        private static bool _IsSplit(char split, char src)
        {
            return split == src;
        }

        private static bool _IsBlank(char element)
        {
            if (element == ' ')
                return true;
            if (element == '\t')
                return true;
            if (element == '\n')
                return true;
            if (element == '\r')
                return true;
            return false;
        }
    }
}
