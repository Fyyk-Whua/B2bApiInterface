using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Util
{

    //简单Id生成器

    //原理：获取系统时间，并在其后加上一个随机数
    //
    //sample:
    // 将IdentityGenerator实例化后可直接获取
    // Embacle.Identity.IdentityGenerator ig = new Embacle.Identity.IdentityGenerator();
    // long identity = ig.GetIdentity();//identity就是生成的Id
    //
    // 也可以直接获取IdentityGenerator的静态实例，这样整个应用程序域内就只有一个实例。结果是一样的，内存挑剔者使用
    // long identity = Embacle.Identity.IdentityGenerator.GetInstance().GetIdentity();
    //
    //
    //输出样例：前8位是时间戳，刻度取到秒（TimestampStyle.SecondTicks），后3位是随机数，设置随机数长度为3
    //下面是GetIdentity()输出结果样例，如果精度要求更高的话可以自行增加随机数长度或者刻度值，可通过GetItentity
    //的重载方法实现。但程序没有捕捉Int64整型数字溢出异常，也就是说，如果设置时间戳刻度太精细或者随机数长
    //度过长都可能导致溢出的异常。
    //
    //如果要自定义该异常请自行更改GetIdentity(DateTime,long,int)签名的方法，在其中加入自己的验证逻辑，不过
    //使用默认GenIdentity()是不会抛出这样的异常的。理论上支持1秒钟生成1000个不重复Id，但是作者自行测试发现
    //在理论需求达到非得在1秒钟生成900个左右的Id或者更高的话，设置随机数长度3会导致重复检查率大大增加，如果
    //有这一很BT的需求的话请考虑增加随机数长度试试看，或者增加时间戳刻度。我发现一般PC机的时钟刻度只能精确到
    //10毫秒，要比10毫秒更精确本人没有任何办法。
    //
    //    21047037328
    //    21047038123
    //    21047039547
    //    21047040005
    //    21047038927
    //
    // 自定义时间戳刻度：
    // 注意：三个参数：
    //    startDateTime    起始时间
    //                    （默认为：2005-01-01 00:00 000）
    //    TimestampStyle    是时间戳刻度枚举，为方便设置自定义时间戳刻度，如果能看懂换算值的话也可以自行输入具体刻度值    TimestampStyleTicks是自行输入时间戳刻度
    //                    （默认为：TimestampStyle.SecondTicks，即取到秒）
    //    randomLength    是随机数长度
    //                    （默认为：3,即0-999）
    // Embacle.Identity.IdentityGenerator ig = new Embacle.Identity.IdentityGenerator();
    // long identity = ig.GetIdentity(4);//自定义随机数长度为4
    // long identity = ig.GetIdentity(TimestampStyle.TenMillSecondTicks);//自定义随机数长度为10毫秒精度
    // long identity = ig.GetIdentity(TimestampStyle.TenMillSecondTicks,4)//同时设置上面两种精度
    // 匆匆完成，请仔细斟酌使用，如果有改进意见或者想骂人可以直接回复该贴，或者发送邮件给我，注意这体现了您的个人修养
    //    
    //                                                        作者：Janssenkm
    //                                                        信箱：janssenkm#gmail.com（请将#改为@）

    /// <summary>
    /// 简单Id生成器
    /// </summary>
    public class IdentityGenerator
    {
        /// <summary>
        /// 随机数缓存
        /// </summary>
        private static Hashtable ht;
        /// <summary>
        /// 时间戳刻度缓存
        /// </summary>
        private long lastTimeStampStyleTicks;
        /// <summary>
        /// 时间戳缓存（上一次计算ID的系统时间按时间戳刻度取值）
        /// </summary>
        private long lastEndDateTimeTicks;

        public IdentityGenerator()
        {
            if (ht == null)
                ht = new Hashtable();
        }

        /// <summary>
        /// IdentityGenerator的静态实例
        /// </summary>
        private static IdentityGenerator ig;
        public IdentityGenerator GetInstance()
        {
            if (ig == null)
                ig = new IdentityGenerator();
            return ig;
        }

        /// <summary>
        /// 按照时间戳刻度计算当前时间戳
        /// </summary>
        /// <param name="startDateTime">起始时间</param>
        /// <param name="timestampStyleTicks">时间戳刻度值</param>
        /// <returns>long</returns>
        private long GetTimestamp(DateTime startDateTime, long timestampStyleTicks)
        {
            if (timestampStyleTicks == 0)
                throw new Exception("时间戳刻度样式精度值不符，不能为0或负数");
            DateTime endDateTime = DateTime.Now;
            long ticks = (endDateTime.Ticks - startDateTime.Ticks) / timestampStyleTicks;
            return ticks;
        }

        /// <summary>
        /// 静态随机数生成器
        /// </summary>
        private static Random random;
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        private long GetRandom(int length)
        {
            if (length <= 0)
                throw new Exception("随机数长度指派错误，长度不能为0或负数");
            if (random == null)
                random = new Random();

            int minValue = 0;
            int maxValue = int.Parse(System.Math.Pow(10, length).ToString());
            long result = long.Parse(random.Next(minValue, maxValue).ToString());
            return result;
        }

        /// <summary>
        /// 计算一个Id
        /// 以2005-1-1 00:00 000为起始时间刻度
        /// </summary>
        /// <returns>long</returns>
        public long GetIdentity()
        {
            DateTime startDateTime = new DateTime(2005, 1, 1, 0, 0, 0, 0);
            TimestampStyle timestampStyle = TimestampStyle.SecondTicks;
            int randomLength = 3;
            return GetIdentity(startDateTime, timestampStyle, randomLength);
        }

        /// <summary>
        /// 计算一个Id
        /// 以2005-1-1 00:00 000为起始时间刻度
        /// </summary>
        /// <param name="timestampStyle">时间戳刻度</param>
        /// <returns>long</returns>
        public long GetIdentity(TimestampStyle timestampStyle)
        {
            DateTime startDateTime = new DateTime(2005, 1, 1, 0, 0, 0, 0);
            int randomLength = 3;
            return GetIdentity(startDateTime, timestampStyle, randomLength);
        }

        /// <summary>
        /// 计算一个Id
        /// </summary>
        /// <param name="randomLength">随机数长度</param>
        /// <returns>long</returns>
        public long GetIdentity(int randomLength)
        {
            DateTime startDateTime = new DateTime(2005, 1, 1, 0, 0, 0, 0);
            TimestampStyle timestampStyle = TimestampStyle.SecondTicks;
            return GetIdentity(startDateTime, timestampStyle, randomLength);
        }

        /// <summary>
        /// 计算一个Id
        /// </summary>
        /// <param name="timestampStyle">时间戳刻度</param>
        /// <param name="randomLength">随机数长度</param>
        /// <returns>long</returns>
        public long GetIdentity(TimestampStyle timestampStyle, int randomLength)
        {
            DateTime startDateTime = new DateTime(2005, 1, 1, 0, 0, 0, 0);
            return GetIdentity(startDateTime, timestampStyle, randomLength);
        }

        /// <summary>
        /// 计算一个Id
        /// </summary>
        /// <param name="startDateTime">时间戳的起始时间</param>
        /// <param name="timestampStyle">时间戳刻度</param>
        /// <param name="randomLength">随机数长度</param>
        /// <returns>long</returns>
        public long GetIdentity(DateTime startDateTime, TimestampStyle timestampStyle, int randomLength)
        {
            long timestampStyleTicks = long.Parse(timestampStyle.ToString("D"));
            return GetIdentity(startDateTime, timestampStyleTicks, randomLength);
        }

        /// <summary>
        /// 计算一个Id
        /// </summary>
        /// <param name="startDateTime">时间戳的起始时间</param>
        /// <param name="timestampStyleTicks">时间戳刻度(毫微秒单位)</param>
        /// <param name="randomLength">随机数长度</param>
        /// <returns>long</returns>
        public long GetIdentity(DateTime startDateTime, long timestampStyleTicks, int randomLength)
        {
            //新一轮时间戳刻度更新后更新缓存
            //如果该参数不变则不进行此更新
            if (timestampStyleTicks != lastTimeStampStyleTicks)
                ht.Clear();

            //取得时间戳（当前时间按刻度取值）
            long timestamp = GetTimestamp(startDateTime, timestampStyleTicks);

            //新一轮时间戳更新后更新缓存
            if (timestamp != lastEndDateTimeTicks)
                ht.Clear();
            //幂
            long power = long.Parse(Math.Pow(10, randomLength).ToString());
            //随机数
            long rand = GetRandom(randomLength);
            //生成结果（Id）
            long result = timestamp * power + rand;

            //如果发现重复
            if (ht.ContainsKey(result))
            {
                //在随机数长度范围内再重复查找一次
                for (int i = 0; i < power; i++)
                {
                    rand = GetRandom(randomLength);
                    result = timestamp * power + rand;
                    //发现非重复的Id
                    if (!ht.ContainsKey(result))
                    {
                        //将新的Id加入HashTable缓存
                        ht.Add(result, result);
                        break;//找到一个同一时间戳内的Id即退出
                    }
                }
                //此处运行在当前时间戳内无法再继续生成Id的代码，如：
                //
                //throw new Exception("已无法生成更多Id，请增加时间戳刻度TimestampStyle或增加随机数长度randomLength");
            }
            else
            {
                //将新的Id加入HashTable缓存
                ht.Add(result, result);
            }
            //记录当前一轮时间戳（当前时间按刻度取值）
            this.lastEndDateTimeTicks = timestamp;
            //记录当前一轮时间戳刻度
            this.lastTimeStampStyleTicks = timestampStyleTicks;
            return result;
        }
    }

    /// <summary>
    /// 时间戳精度样式<br>
    /// 采用格里高利时间刻度单位－毫微秒（1秒 ＝10,000,000毫微秒 ）
    ///    疑问：MSDN上写的一毫微秒为一百万分之一秒 ＝100,000,000毫微秒，但是我始终无法算出该值，
    ///    不知是我错了还是MSDN的问题实际得出的数字是1秒 ＝10,000,000毫微秒，认为是我错了的话请将
    ///    下面的每一项枚举值都加一个0即可
    /// </summary>
    public enum TimestampStyle : long
    {
        /// <summary>
        /// 时间刻度精度取为1毫秒（此项无意义，因为一般PC机系统时钟只能精确到10毫秒）
        /// </summary>
        MillSecondTicks = 10000,
        /// <summary>
        /// 时间刻度精度取为10毫秒，这是一般PC机系统时钟的最小精度单位
        /// </summary>
        TenMillSecondTicks = 100000,
        /// <summary>
        /// 时间刻度精度取为100毫秒
        /// </summary>
        HundredMillSecondTicks = 1000000,
        /// <summary>
        /// 时间刻度精度取为1秒，即1000毫秒
        /// </summary>
        SecondTicks = 10000000,
        /// <summary>
        /// 时间刻度精度取为5秒
        /// </summary>
        FiveSecondTicks = 50000000,
        /// <summary>
        /// 时间刻度精度取为10秒
        /// </summary>
        TenSecondTicks = 100000000,
        /// <summary>
        /// 时间刻度精度取为1分种（60秒）
        /// </summary>
        MinutesTicks = 600000000
    }
}

