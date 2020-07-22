using System;
using System.Globalization;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: LunarCalendarUtil.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: 中国农历基类
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/

    public class LunarCalendarUtil
    {
        
        #region ChineseLunisolarCalendar 实例化 
        /// <summary> 
        /// 实例化一个  ChineseLunisolarCalendar     
        /// </summary>
        private static ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();
        #endregion
         
        #region tg 十天干
        /// <summary>
        /// 十天干
        /// </summary>
        private static string[] tg = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        #endregion

        #region dz 十二地支
        /// <summary>
        ///  十二地支
        ///  </summary>
        private static string[] dz = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        #endregion

        #region sx 十二生肖
        /// <summary>
        /// 十二生肖
        /// </summary>
        private static string[] sx = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        #endregion

        #region GetLunisolarYear 返回农历天干地支年
        /// <summary>
        ///  返回农历天干地支年
        ///   </summary>
        ///    <param name="year">农历年</param>
        ///    <returns></returns>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;
                return string.Concat(tg[tgIndex], dz[dzIndex], "[", sx[dzIndex], "]");
            }
            throw new ArgumentOutOfRangeException("无效的年份!");
        }
        #endregion

        #region months 农历月 
        /// <summary>
        /// 农历月
        /// </summary>
        /// <returns></returns>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };
        #endregion

        #region days1 农历日
        /// <summary>
        /// 农历日
        /// </summary>
        private static string[] days1 = { "初", "十", "廿", "三" };
        #endregion

        #region 农历日 days
        /// <summary>
        ///  农历日
        /// </summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
        #endregion

        #region GetLunisolarMonth 返回农历月
        /// <summary>
        /// 返回农历月
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public static string GetLunisolarMonth(int month)
        {
            if (month < 13 && month > 0)
            {
                return months[month - 1];
            }
            throw new ArgumentOutOfRangeException("无效的月份!");
        }
        #endregion

        #region GetLunisolarDay 返回农历日 
        /// <summary>
        /// 返回农历日
        /// </summary>
        /// <param name="day">天</param>
        /// <returns></returns>
        public static string GetLunisolarDay(int day)
        {
            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[(day - 1) / 10], days1[1]);
                }
            }
            throw new ArgumentOutOfRangeException("无效的日!");
        }
        #endregion

        #region GetChineseDateTime 根据公历获取农历日期
        /// <summary> 
        /// 根据公历获取农历日期
        /// </summary>
        /// <param name="datetime">公历日期</param>
        /// <returns></returns>
        public static string GetChineseDateTime(DateTime datetime)
        {
            //农历的年月日
            int year = ChineseCalendar.GetYear(datetime);
            int month = ChineseCalendar.GetMonth(datetime);
            int day = ChineseCalendar.GetDayOfMonth(datetime);
            //获取闰月， 0 则表示没有闰月 
            int leapMonth = ChineseCalendar.GetLeapMonth(year);
            bool isleap = false;
            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月     
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }
            return string.Concat(GetLunisolarYear(year), "年 ", isleap ? "闰" : string.Empty, GetLunisolarMonth(month), "月  ", GetLunisolarDay(day));
        }
        #endregion
    }


}

