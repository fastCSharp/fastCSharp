using System;
using System.Timers;

namespace fastCSharp
{
    /// <summary>
    /// 日期相关操作
    /// </summary>
    public unsafe static class date
    {
        /// <summary>
        /// 每毫秒计时周期数
        /// </summary>
        public const long MillisecondTicks = 10000;
        /// <summary>
        /// 一天的计时周期数
        /// </summary>
        public const long DayTiks = 24L * 60L * 60L * 1000L * MillisecondTicks;
        /// <summary>
        /// 32位除以60转乘法的乘数
        /// </summary>
        public const ulong Div60_32Mul = ((1L << Div60_32Shift) + 59) / 60;
        /// <summary>
        /// 32位除以60转乘法的位移
        /// </summary>
        public const int Div60_32Shift = 21 + 32;
        /// <summary>
        /// 16位除以60转乘法的乘数
        /// </summary>
        public const uint Div60_16Mul = ((1U << Div60_16Shift) + 59) / 60;
        /// <summary>
        /// 16位除以60转乘法的位移
        /// </summary>
        public const int Div60_16Shift = 21;
        /// <summary>
        /// 时间转换成字符串(精确到秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>时间字符串</returns>
        public unsafe static string toString(this DateTime time)
        {
            char* chars = stackalloc char[20];
            ToString(time, time.Ticks % DayTiks, chars);
            return new string(chars, 0, 19);
        }
        /// <summary>
        /// 时间转换成字符串(精确到秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="dayTiks">当天的计时周期数</param>
        /// <param name="chars">时间字符串</param>
        public unsafe static void ToString(DateTime time, long dayTiks, char* chars)
        {
            int data0 = time.Year, data1 = (data0 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data0 -= data1 * 10;
            int data2 = (data1 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data1 -= data2 * 10;
            int data3 = (data2 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data2 -= data3 * 10;
            data0 += '0';
            data1 += '0';
            data0 <<= 16;
            *(int*)(chars + 2) = (data1 += data0);
            data2 += '0';
            data3 += '0';
            data2 <<= 16;
            *(int*)chars = (data3 += data2);

            data0 = time.Month;
            data1 = (data0 + 6) >> 4;
            data0 -= data1 * 10;
            data1 += '0';
            data0 += '0';
            data1 <<= 16;
            *(int*)(chars + 6) = (data0 += ('/' << 16));
            *(int*)(chars + 4) = (data1 += '/');

            data0 = time.Day;
            data1 = (data0 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data0 -= data1 * 10;
            data1 += '0';
            data0 += '0';
            data0 <<= 16;
            *(int*)(chars + 8) = (data1 += data0);

            data3 = (int)(dayTiks / (1000 * 10000));
            data2 = (int)(((ulong)data3 * Div60_32Mul) >> Div60_32Shift);
            data3 -= data2 * 60;
            data0 = (data2 * (int)Div60_16Mul) >> Div60_16Shift;
            data2 -= data0 * 60;

            data1 = (data0 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data0 -= data1 * 10;
            data1 += '0';
            data0 += '0';
            data1 <<= 16;
            *(int*)(chars + 12) = (data0 += (':' << 16));
            *(int*)(chars + 10) = (data1 += ' ');

            data1 = (data2 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data2 -= data1 * 10;
            data1 += '0';
            data2 += '0';
            data2 <<= 16;
            *(int*)(chars + 14) = (data1 += data2);

            data1 = (data3 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data3 -= data1 * 10;
            data1 += '0';
            data3 += '0';
            data1 <<= 16;
            *(int*)(chars + 18) = data3;
            *(int*)(chars + 16) = (data1 += ':');
        }
        /// <summary>
        /// 时间转换成字符串(精确到毫秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>时间字符串</returns>
        public unsafe static string toSqlMillisecond(this DateTime time)
        {
            long dayTiks = time.Ticks % DayTiks;
            char* chars = stackalloc char[24];
            ToString(time, dayTiks, chars);
            chars[19] = '.';
            int data0 = (int)(((ulong)(dayTiks % (1000 * 10000)) * number.Div10000Mul) >> number.Div10000Shift);
            int data1 = (data0 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data0 -= data1 * 10;
            int data2 = (data1 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
            data1 -= data2 * 10;
            data0 += '0';
            data1 += '0';
            data2 += '0';
            data1 <<= 16;
            chars[22] = (char)data0;
            *(int*)(chars + 20) = (data2 += data1);
            return new string(chars, 0, 23);
        }
        /// <summary>
        /// 日期转换成整数表示
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>整数表示</returns>
        public static int toInt(this DateTime date)
        {
            return date != default(DateTime) ? (date.Year << 9) + (date.Month << 5) + date.Day : 0;
        }
        /// <summary>
        /// 当前日期转换成整数表示
        /// </summary>
        /// <returns>整数表示</returns>
        public static int ToInt()
        {
            DateTime date = nowTime;
            return (date.Year << 9) + (date.Month << 5) + date.Day;
        }
        /// <summary>
        /// 整数表示转换成日期
        /// </summary>
        /// <param name="dateInt">整数表示</param>
        /// <returns>日期</returns>
        public static DateTime GetDate(int dateInt)
        {
            DateTime date = default(DateTime);
            try
            {
                int year = dateInt >> 9, month = (dateInt >> 5) & 15, day = dateInt & 31;
                if (year >= 1900 && year < 10000 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                {
                    date = new DateTime(year, month, day);
                }
            }
            catch { }
            return date;
        }
        /// <summary>
        /// 星期
        /// </summary>
        private static readonly pointer weekData;
        /// <summary>
        /// 月份
        /// </summary>
        private static readonly pointer monthData;
        /// <summary>
        /// 时间转字节流
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>字节流</returns>
        public unsafe static byte[] toBytes(this DateTime date)
        {
            byte[] data = new byte[29];
            fixed (byte* fixedData = data)
            {
                *(int*)fixedData = weekData.Int[(int)date.DayOfWeek];
                int value = date.Day, value10 = (value * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                *(int*)(fixedData + sizeof(int)) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (' ' << 24)) | 0x303000;
                value = date.Year;
                *(int*)(fixedData + sizeof(int) * 2) = monthData.Int[date.Month - 1];
                value10 = (value * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                int value100 = (value10 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                int value1000 = (value100 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                *(int*)(fixedData + sizeof(int) * 3) = (value1000 + ((value100 - value1000 * 10) << 8) + ((value10 - value100 * 10) << 16) + ((value - value10 * 10) << 24)) | 0x30303030;


                value100 = (int)(date.Ticks % DayTiks / (1000 * 10000));
                value1000 = (int)(((ulong)value100 * Div60_32Mul) >> Div60_32Shift);
                value100 -= value1000 * 60;
                value = (value1000 * (int)Div60_16Mul) >> Div60_16Shift;
                value1000 -= value * 60;

                value10 = (value * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                *(int*)(fixedData + sizeof(int) * 4) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (':' << 24)) | 0x303000;
                value10 = (value1000 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                value = (value100 * (int)number.Div10_16Mul) >> number.Div10_16Shift;
                *(int*)(fixedData + sizeof(int) * 5) = (value10 + ((value1000 - value10 * 10) << 8) + (':' << 16) + (value << 24)) | 0x30003030;
                *(int*)(fixedData + sizeof(int) * 6) = ((value100 - value * 10) + '0') + (' ' << 8) + ('G' << 16) + ('M' << 24);
                *(fixedData + sizeof(int) * 7) = (byte)'T';
            }
            return data;
        }
        /// <summary>
        /// 精确到秒的时间
        /// </summary>
        private static DateTime nowTime;
        /// <summary>
        /// 精确到秒的时间
        /// </summary>
        public static DateTime NowTime
        {
            get { return nowTime; }
        }
        /// <summary>
        /// 刷新时间
        /// </summary>
        private static void refreshTime(object sender, ElapsedEventArgs e)
        {
            nowTime = DateTime.Now;
            timer.Interval = 1000 - nowTime.Millisecond;
        }
        /// <summary>
        /// 刷新时间的定时器
        /// </summary>
        private static Timer timer = new Timer(1000);
        static date()
        {
            int dataIndex = 0;
            pointer[] datas = unmanaged.Get(false, 7 * sizeof(int), 12 * sizeof(int));
            weekData = datas[dataIndex++];
            monthData = datas[dataIndex++];

            int* write = weekData.Int;
            *write = 'S' + ('u' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'M' + ('o' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'T' + ('u' << 8) + ('e' << 16) + (',' << 24);
            *++write = 'W' + ('e' << 8) + ('d' << 16) + (',' << 24);
            *++write = 'T' + ('h' << 8) + ('u' << 16) + (',' << 24);
            *++write = 'F' + ('r' << 8) + ('i' << 16) + (',' << 24);
            *++write = 'S' + ('a' << 8) + ('t' << 16) + (',' << 24);

            write = monthData.Int;
            *write = 'J' + ('a' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'F' + ('e' << 8) + ('b' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'A' + ('p' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('y' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('l' << 16) + (' ' << 24);
            *++write = 'A' + ('u' << 8) + ('g' << 16) + (' ' << 24);
            *++write = 'S' + ('e' << 8) + ('p' << 16) + (' ' << 24);
            *++write = 'O' + ('c' << 8) + ('t' << 16) + (' ' << 24);
            *++write = 'N' + ('o' << 8) + ('v' << 16) + (' ' << 24);
            *++write = 'D' + ('e' << 8) + ('c' << 16) + (' ' << 24);

            nowTime = DateTime.Now;
            timer.Elapsed += refreshTime;
            timer.Start();
        }
    }
}
