using System;

namespace fastCSharp.unsafer
{
    /// <summary>
    /// 整数相关操作(非安全,请自行确保数据可靠性)
    /// </summary>
    public static class number
    {
        /// <summary>
        /// ascii字符串转数字
        /// </summary>
        /// <param name="numbers">ascii字符串,不能为空</param>
        /// <returns>数字</returns>
        public unsafe static int Parse(byte* numbers)
        {
            int value = *numbers;
            uint xor = 0;
            if (value == '-')
            {
                ++numbers;
                xor = uint.MaxValue;
                value = *numbers;
            }
            if ((uint)(value -= '0') < 10)
            {
                for (int number = *++numbers; (uint)(number -= '0') < 10; number = *numbers)
                {
                    value *= 10;
                    ++numbers;
                    value += number;
                }
                return (int)(((uint)value ^ xor) + (xor & 1));
            }
            return 0;
        }
    }
}
