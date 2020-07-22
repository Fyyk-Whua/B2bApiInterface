using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
 
namespace Registered
{
    /// <summary>  
    /// 类名：RSAFromPkcs8  
    /// 功能：RSA加密、解密、签名、验签  
    /// 详细：该类对Java生成的密钥进行解密和签名以及验签专用类，不需要修改  
    /// 版本：3.0  
    /// 日期：2013-07-08  
    /// </summary>  
    public sealed class RSAFromPkcs8
    {
        private const string publicKey = @"<RSAKeyValue><Modulus>wDflXv2c+lRr5J99u/1gkQdut+7/RTusuxx5zEBF0i8arMAzH43JjqffVRvnuIeSLQYCt2Hcs+zRbjfjmgK3OdDJVzCf9qTA6rWaBebz5al7/jdSo5J9HsSqjTzQXDih2AGpGpKfez9LARRvoLmyDXCttLw6w2BflOfUJnEoxWImLpDE/2zqIOMgbY0m2FOsVOdqaRMsOOeWJUhHRtywYebVoY5/4ZqQ2iQMaddzbwUopnHnY6YYcztyC15JWwGmcO8T9e6CANLCWUGOsUckDff0nNnXeIpM9c94liSSFGoHsoaG3wzTlpUlbS2jcm5xJemfg1dxMEk/AOBsN+lWZaA7LybYYzabV2AdZfabK6lqamVfYhk08ul5rGu+pFhKBsXNo1XFjS79/lpA6/+uTZFW8keIzi8ZaHpcgVwFyV/+gP7F5fx3mPk1LQYdZNUoPuQQnmiWZYCI3Neg2PPdSs3/29V3bzZywZ5UZMMMEdJBrJvPadtwriBnQjiCDT99Pgvq95bQIsT458F6DiFQW7PySlhTClc5bYIJc/Kr+AE88iErkO97ibOjSyrSDeQYL/BR2J+xcpjHLmxrV3mpw8hNFUqhUws+gMIE7dwGh2ihXTKk2EdZkwuVrFSkC685gB6oV2lfztNMsx89FOM5KaT3gPJS9XJvWBJjLcPTg6E=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string privateKey = @"<RSAKeyValue><Modulus>wDflXv2c+lRr5J99u/1gkQdut+7/RTusuxx5zEBF0i8arMAzH43JjqffVRvnuIeSLQYCt2Hcs+zRbjfjmgK3OdDJVzCf9qTA6rWaBebz5al7/jdSo5J9HsSqjTzQXDih2AGpGpKfez9LARRvoLmyDXCttLw6w2BflOfUJnEoxWImLpDE/2zqIOMgbY0m2FOsVOdqaRMsOOeWJUhHRtywYebVoY5/4ZqQ2iQMaddzbwUopnHnY6YYcztyC15JWwGmcO8T9e6CANLCWUGOsUckDff0nNnXeIpM9c94liSSFGoHsoaG3wzTlpUlbS2jcm5xJemfg1dxMEk/AOBsN+lWZaA7LybYYzabV2AdZfabK6lqamVfYhk08ul5rGu+pFhKBsXNo1XFjS79/lpA6/+uTZFW8keIzi8ZaHpcgVwFyV/+gP7F5fx3mPk1LQYdZNUoPuQQnmiWZYCI3Neg2PPdSs3/29V3bzZywZ5UZMMMEdJBrJvPadtwriBnQjiCDT99Pgvq95bQIsT458F6DiFQW7PySlhTClc5bYIJc/Kr+AE88iErkO97ibOjSyrSDeQYL/BR2J+xcpjHLmxrV3mpw8hNFUqhUws+gMIE7dwGh2ihXTKk2EdZkwuVrFSkC685gB6oV2lfztNMsx89FOM5KaT3gPJS9XJvWBJjLcPTg6E=</Modulus><Exponent>AQAB</Exponent><P>4hddR2Kv0Hitgrjt1u3fHr7JLMWYqYq6dryo33C6UZeHiYahU7Hz/C5Ys5NBal3v++D+AeokLLEN+yyqBrIrzddZHLQa7D+L+9S0zmW2qfUAwczFncZZdSwkPzOGAPlRU1w3MfzuiyCb/PDW5u9mT5b3C9m7QjQyOsVNd1jJh9vzdmN+miePPKF+H3O+vPcwjJ6B9tCU2D+J0inZofWLF4nbiKzZG/HMGA2rFPr4D45/2fls7G8xohMRyl1hK4fhy/ajLSuk3kdz50PttdXoKLOWFW/UzYQZSsX2iD31Yrv2arNNzumgtuSv6kLj5uxHE2rEfo2voBCrdszhOHoSbw==</P><Q>2aVqtfD1n9RIyuVSojiNswP11jOhPx2XEHZZWz/IaWeN/RucqpMHCCfr6ziXGgbvP+Rc4vuyg7i/cOVtee/RblAbIl0UvUjfI+SfYCeu+SKDryM3Q+e9BXLXCnz4p9Q/slD3tYSjBStGQyyFYE/NX8RRxoruCnSct5NW2Kg42db8Cj5uMWrWCDCqR1aUAFC2n7lRJLndG1U4DsMZ8DQvejkw2d5fgC28ici+zsQFlIY2Ehz7JpLxbSF1SSTn32rYcOZ+/n5UFsES6O4TZeS6sV/2O8JQiZpO/Am5lKhJdFeiH9qSCi2hn3ZZb4UGr1WX0/ObOY2TMHuwFmgporeS7w==</Q><DP>Wy3NAoL8IHjqkLN/x5P6zW1nuFSf8o/uDwCaojHWEh2oHYBcA2La4LLYNuOGGB12MQBVr/P7TsLAvrEOiNDF9aeJWpgP24LtM0xe9nXyqqwR+BZbA/wAC88Xdx2SR0ZE0/d2kwRMN3OZNPLz2AJoImThkLfoWxStxSHY0e7Op6m+j9okLosRqwP91zVYkOCnha5O/3iy7lsBe+5AxJ4/z+hazs3WaPyksHairj1gWPXA/eknUlqti41hOVlbXLDaEOWQfSECxlnOlk/Ax/pSPEfgmRiWm6NQh+U6PUuvcHgZMoLJOE7j755ei30aaACV0XDXeaouT8EpRSnvElW4pQ==</DP><DQ>JaSlkzhM00r9GBKXuzT61RZ/Q4AEiI0OnqtJgy3B9PETLs7CxwHM+o0lEbbk83x1juBpbFzKIFwoyFb2G1vuJh02xDfDKYXcCjI1moTij3Z3vifiHB2NpM6bANZij2x3Y9j7Lur04yT77wxZOTI0PDcRSbyntSk8HcgfAjP+OTckuoTXvDZVNG9uVA5WZMORAokjzq1S37JakrsN1mzANWyE1XnPnLTMYixVTB+9M1YPLgzTJ3XMyeo6hd8bN5ocO1KybkPdOV5FbgIXWs03uQv245kVIueMd60a6uo8YiHyCX5dOeNwi8sFZvNkF5k/PEpq84ZYCc+qwgVcBieoAw==</DQ><InverseQ>LyIyvHyOv9zk19fHb4FdbOG1s3sf9nVzqbaWCxsbobcR/a2KqJNTT7lIMZKsbMsa49LYVuIgPDHIcxIo81cQMTy+s0Mz7HKUw6cLOH3tNpZkSIw/NFlHvhLrI72f2liXh8jkzHADB+0MhwAU1aQ6Lp6ywNr7aIsNuW91mbMR8J0CoWDhDerWfVbdrAWQVRKiZqZrLZjADJLxVX67UVrJMr4D2SnYkF8rpw1Kf27jyR7dAVTZZSnMHt9hGL4g/jEKA5jyCpDIW+sYw77Qsy4oGVckLLbNjtpDZ2FSbrVToLCKkip5Yr7TDYX3wkw+770rlXvyeBAe4HFlJbKwg/UFTw==</InverseQ><D>leqCgKW4yx0Q0Uz7WaVyrWpolhhXC/5Q59Z7wCvYyOdoHJgCx3dkXljbvJt5DNjfzzHC6Jfw9HudlCMO5s17biJDBjesa0dwFx7wM+4/Bz46sLkhQOY9o8OraWCm+WZKBBi3dTwqGovwybTQo0BxeD4LnMy54CvIIHD3a8wSCnBUazkKDFU7/c5nOko+o7YO0GF1AuKrOO6WkhlfTgtNne3VbJ2ha6mLlDdNXzdklpAldjucRoICy8zvU6KD//C3iZlNriAJUO8QGWEiE23y1tou9C6K+T6Ua5sW57M6I6XWvwxhIJH2bJmCiTZxJgyxPuqJOHz4kLpv4xUmEi1w7ZQGahtdpk2GT/uHIwxIA5ha8zZZirri0/O8PXzpEpqFGscz52RpqJaQHjHwS/N6AA3I8ZNCm1k5SOvKaelsWokRbqYOIa1pUg8vop3tdxHrLiAgsRpxEUEBeqOjdCja6QEuSiWF/wtZghkPPxSfYhlCK02qbuhtRsm7KLlv+7Oj18KWg06kpqnOjqwCJ1sLpScl+B12LjR4+Es92Xpr2dKZ9e/JS1oohcUGUCWI3sGudHncihmPma8orFCR5GnBZDZWZ/4c2Sw9iOVspq4MAwST4vgS5zCqVN8Hs4EeDSf/2DAQvHPRwZCpQsn4abeBn89kISAw1KeiS2Tr6NobdSk=</D></RSAKeyValue>";


        private const string _RegSignPublicKey = @"<RSAKeyValue><Modulus>y8Nq6ZG2FLQLc8NoSSWXcyr3pVWH2+IULoaB6H0OIe9iZwaVqBHm36pGQF6afT+2o5RxtWlV74IbRaP70CvD5SFQDcNCU6K95rB/MPnekGbQ4N2mQq9YFu0HxlEV6Dufb63Rd6Rew51dx5b4nju76aRMjQ4RQKHfcE18bJalq0bMS6t0NK2ZSfGuS0cISA/FVZ4CUXyewATphLJ4YecFtr32/Xdin5Sm5EoQxWPRsYPgehnQEc/WPeTCZSZEmRS2f+QWI6sjoh0mBpr6/opWnaYDEHUJ976bhFB5liY1Nra2u1Pg00JYJUxjCIyOQXqQUNXsRRa7ZUZquCUIuT9AsHq+KR90oKAWfrdcT5oF3XRa9ZusdvuYwtXhqYhMDDDjV3aznv/6cO6igPVhkNBRDDBlAKzhBopBm0dtejhSlCTpCS+rQvUE1ceGlJYqEPH/RnjQb7UVRfJFkJ4YxfMHNNEdm7yEenZamFKo0HibxXbhXlfSWb6kKnoUoM4ehA1Zjvijql+m+mU1QdkZDwXIVxNp2ZP4nAmCa9z3MMmYjewcsQ7WzgXwAoXdtcNKEF8RWjvyQatSSOGo/enz/dGjNXmX+2nWz/c/y11GVCXETe8J6ZgkaoU8YPCsz0sxmRpEc8vvY5qMx0AJtrxbnRTz0WoT09Rb23no4gTr+tR2CJc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string _RegisteredSignPrivateKey = @"<RSAKeyValue><Modulus>y8Nq6ZG2FLQLc8NoSSWXcyr3pVWH2+IULoaB6H0OIe9iZwaVqBHm36pGQF6afT+2o5RxtWlV74IbRaP70CvD5SFQDcNCU6K95rB/MPnekGbQ4N2mQq9YFu0HxlEV6Dufb63Rd6Rew51dx5b4nju76aRMjQ4RQKHfcE18bJalq0bMS6t0NK2ZSfGuS0cISA/FVZ4CUXyewATphLJ4YecFtr32/Xdin5Sm5EoQxWPRsYPgehnQEc/WPeTCZSZEmRS2f+QWI6sjoh0mBpr6/opWnaYDEHUJ976bhFB5liY1Nra2u1Pg00JYJUxjCIyOQXqQUNXsRRa7ZUZquCUIuT9AsHq+KR90oKAWfrdcT5oF3XRa9ZusdvuYwtXhqYhMDDDjV3aznv/6cO6igPVhkNBRDDBlAKzhBopBm0dtejhSlCTpCS+rQvUE1ceGlJYqEPH/RnjQb7UVRfJFkJ4YxfMHNNEdm7yEenZamFKo0HibxXbhXlfSWb6kKnoUoM4ehA1Zjvijql+m+mU1QdkZDwXIVxNp2ZP4nAmCa9z3MMmYjewcsQ7WzgXwAoXdtcNKEF8RWjvyQatSSOGo/enz/dGjNXmX+2nWz/c/y11GVCXETe8J6ZgkaoU8YPCsz0sxmRpEc8vvY5qMx0AJtrxbnRTz0WoT09Rb23no4gTr+tR2CJc=</Modulus><Exponent>AQAB</Exponent><P>94N+/7ggiIJiIT4rEgH6OGlrXFO1l2m+l8Zp4fVqWBoO8My5+Nk1qGQN0gnyh9o2wvw7P0dTqEu93UjiousIcqdDu3S0HgI+jviIHNRR6zsYBBy4K5NBQHJGRpgmKA5lA2/2kHYW4W33zyGiB0YS/LOw1i5S0lytr8kuY5kuD4egEyjc6jFcmCUjC5P4F+t97GdDSMUJdfSa7VMr1dgIdS3M3ltmR/4vhGxGqObDv6rJqpn/oLtn/PK1DhLYjaxGQRW62V61iy+1jLjs/DSziFVZL2/5N30tRJfEiwSaUb7gfFgT1ddO2Z4ia0mZR83JmB0XadeGY5+bolp/1nuo2w==</P><Q>0r/pXG2rUs1wReNc2vg17NIYY0n/saTShyY7jtl/vinIKzrx/UR/Q6AZei0nOFUayMEUX+ZARKrSLgO/fdUQSlIPVSSKM+uxKi4AOOaOO5uIxagXkqMg8PMAXCM/WHWrd1p+JZULO+Z3j6S2cQ7UXLEi4OAZuDxcH5egT8HkaAKSMTd6ifTUEGDpeNJz66hXIxmm5kt85zmuCKiu6xCIikv50vxvytLdqyiFrpDvBZ2r4MBh3KCTrEadFPNx8EmDnmPsJVPrGBaO62dWh6vKVpiC/f3xATEy+2APjsEc3Io9ATDkLM19XasYQPwb6ZZa3e0xS+1IO5rw34aNyMf99Q==</Q><DP>B8euWkNjYmcWxoy5tdsyDkviAAjxkEzWnNazxVJ9gT9wcMk+nz/Um/JpLMz7PqHxTre29Qo86vFWinocBZr1rQTs8Bt+/eJ8LOpK/Pz/hjFZU+fDMjtytZ/h7Z4itOee7Ti7u1a66WMXgv8/pJLjTeYoDNNv7wTSwM/GEYNjG0HcGj4Sk5nxmyavr1F7XuUcFC46wzLOVVLW+9a9bf9YZLaH1gVxdZnbzIHKxsxaItAvfplQm7DIV/8ZCdQ10l4z5x/Tu7lqY3Ggd0foyxStAAAOyZrvbnsUzS8oEmaWozMowz/Rf8tAwz5hPpYVp1gkmg9wCPepVcBSmAvYMNm6OQ==</DP><DQ>yMIkLPYTxBcLoqfJppXX2LbyoHK3bqQSIMhc5+Fs/NuUYQoPxzHfAa6bVnV47QK1NxQmsowGIOOQwGC1o8q5b/LnxDXAqWEWLZYQhCOszj+FdLSBcCCRmrYBW8P/7eZ55oJ/tJFcWD1dG6rOWLjFt17OWOVh2s00/KtV/WQ4jpQUa2nsA0sEUG3hOkVQQ+biyv7+rFawrxuVG46EwkvHpeZmH4R1ggKJQyig4AAUkYb3WmwpTSByTCQgMvsNSNbe2J1bMNvWEeY7Uyfnl+ogH4m9DvM/B+G0LR3+9AAl3DibkGzgj4VYrUf1HMKMXGHsQYhX83of2xfn6SamHePVmQ==</DQ><InverseQ>dwNvHZyTqo29WecIhGqmVta46xGSDf4yiV0+F5JqmOtnRwSrpZnBtKg6uUTrexp0eXVRObtds1sHNIxIsgIlitEOfTGpcj7xYgdSAz16prLbn3niSoXIkkArUpla2bZ5p/fbK5HRiTmaIU/N466ic8wPz0oT86zqShg6UxE01BP/XuWNhEUmRIAVM0q26MGWhtqqvRwFj0plMh7ny0Xg9QSW3csSRihg5WoF+81WJ/cV6d642u1J4KwVEmtRWoQwLB3XKI9v3EEeMTL1X2Kej4WlL3mBty7xWWrgTyT7RnxAZKq7QGwKYTfv8ma89nJgXu3w467lpUF2qWmcYV6IEQ==</InverseQ><D>hrjPAF1R+QBNrh2d3vcW2pOnJ06UxCIHW/ec/t1oMbG36wxkeLpVXr6TMk4acQNmO2OThvF9Wx038OSKQsoc/Gr5JhBa0zd/vX4mqnga2njQVEzYd7C9WnMft9S22lRJhypym1s3OLjcX3GHMf+mr8TsxDpv177vH65rvNQh90uZGdLjw0ygVE6SAb2WUSb0PzZ4q+3sfGMDDrR9eaWolnmlS1LbB03exoPd8NSduXPLQI++jXhQW53blcsmgdw7CfYBWVX2+mGxWZ6wowlOWW+BCSmJDJ+e7W3T5h2fA2ztBpBfzu5Hn2mQ9P8Rs2NY9clrLQbpof1b5CLAUNLsblPK5ZZ8kZsFDLsDEjBszwrIbJyluRXV3ybreugY2SqnGNNaaxezNS+6COgbZpto4aGqKhs38WjkTCpWQOnc9PD0ggKrBBvcsQECKjeUDLvWRS2mTv6HwigtY1zqETHFWYol8mCKbIp30y3lJ5hvQr5WMMY4pKXLUa7rs9f0Ly5w6wdYmAOTpE63vVghwDteAjSZl2atOF83RdhYhzq5OU4wLSriHwhFCMsi7ApZDX/3XpUP5a4d8lKFKxsc4HtL5yAAO6qXQx+PlMv/TfHr0Q5Fre1Hey6imPw8vPShMr+27wppF2xZehYARkbQZQCFZ0H26V+xWVNg/rLkz49jn1k=</D></RSAKeyValue>";

        private const string _RegEnPublicKey = @"<RSAKeyValue><Modulus>rqInoVQNNWY6GlzX2DYb4ZPCIU0rk3WuifQWPUc5fUKaK/cyiA0mZe99blFt2OHcYy3JXyROOjcTQbN1Cvp75bVG/MqSTtLx8tMyWBYq9jYCtQBzwbOE1VFk6tF3QBa/ZkmONkb9DWvd+M/k9/h7mh2QzJJvD5WlrQ+cV3O6w7lqHpx9t6NVoRpb3GBGwzgfv9Uq2KQ+aCsL7G1UMjmM9cKkcJ5wLLBYRoubiFrQzaQh/osDPpkWkTjYVpmKpUBELNnENxiupZwph85oRo8uHtLeR2z1vX51fmS1DdVi0jE3CpEkyWRmrdu0Cpkr+NgPKhqMLa5MwTexoLIW4v0OSCzOJgwLUY0wtp3jUrDKhwAR/wyiIN43Z45KvEDncpo7/QJ+A7ha1lmRQ6O7qu4HFazZURMZKWPHsYndKHVLu9dy3lxaiJMCYQnXtj7aiJFYC9YU6gf8oe7FMnEzc3CPwNIzS5Oiy/tmKd9s97XS9+i6shdG3nQwURwmoOh2oJn8v7erfnHpC/BJ7PlfmiPoHSd3HIGorp9o4BkY0tC81QdTObTVuvXWXIb4L4l5+nKsivIwcB6G/t5VS4wbcHGoau9sqh1KY1uAg7lrQyKWrPvSdy6BOo42UYL7mx2V9UVNmAmd6SeDs0GjGq4Zimib9cbCOyQqhzVa6eBlpJ0YOAc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string _RegisteredDePrivateKey = @"<RSAKeyValue><Modulus>rqInoVQNNWY6GlzX2DYb4ZPCIU0rk3WuifQWPUc5fUKaK/cyiA0mZe99blFt2OHcYy3JXyROOjcTQbN1Cvp75bVG/MqSTtLx8tMyWBYq9jYCtQBzwbOE1VFk6tF3QBa/ZkmONkb9DWvd+M/k9/h7mh2QzJJvD5WlrQ+cV3O6w7lqHpx9t6NVoRpb3GBGwzgfv9Uq2KQ+aCsL7G1UMjmM9cKkcJ5wLLBYRoubiFrQzaQh/osDPpkWkTjYVpmKpUBELNnENxiupZwph85oRo8uHtLeR2z1vX51fmS1DdVi0jE3CpEkyWRmrdu0Cpkr+NgPKhqMLa5MwTexoLIW4v0OSCzOJgwLUY0wtp3jUrDKhwAR/wyiIN43Z45KvEDncpo7/QJ+A7ha1lmRQ6O7qu4HFazZURMZKWPHsYndKHVLu9dy3lxaiJMCYQnXtj7aiJFYC9YU6gf8oe7FMnEzc3CPwNIzS5Oiy/tmKd9s97XS9+i6shdG3nQwURwmoOh2oJn8v7erfnHpC/BJ7PlfmiPoHSd3HIGorp9o4BkY0tC81QdTObTVuvXWXIb4L4l5+nKsivIwcB6G/t5VS4wbcHGoau9sqh1KY1uAg7lrQyKWrPvSdy6BOo42UYL7mx2V9UVNmAmd6SeDs0GjGq4Zimib9cbCOyQqhzVa6eBlpJ0YOAc=</Modulus><Exponent>AQAB</Exponent><P>3QkNy7tNT7/TqH6+5EAYxv3UIBMkolztFWml4I3r4q/UfskcB0/QFsM0Y1ccvdNp8j9VB1bewu3wRDEox1UQYS+Y28E4UmeRvQVYc4Mp+BNUm6q74Jcd3nDo/Udsg2YWQvkdcFbqssnuCsssSktlDkyTCxMMNR2m2q8cvJ365uYZN+YemfRew3QmVN5rVmyVZlha1wxoRRd/Mp4ceNe+Ji73lJhTQC+KkXeiNtV2IOay19zrcavf2JZXPhopUUW7ND44PhmrEsQiJZOn01TJwHR1dscUIQmkk6vrJwHlJDB8OmsCQtWjOwCvSA/JNNU9Dkf1jBXyuJYJ1DkWVTe2DQ==</P><Q>ykIHbJmAsNoSV5xqe5XWI7k+zIc183vdJtSaUAdot4K6ncv6iimYXCXfL0gof720jIklFwlJl0PUOLBd1fm2xLY5ojksToltOHVxkeOCHfLHWgj6CLVqq9WLHVJ6+jOBYHpAZ8qpYqPd5PiYvl12ucr3LEl0wrGY2OtTG6cVnyh5WxJCAJA9T/ROiGM2cMc5wefkNOL0jFh7aqE/Ck3gfglkh0EAX7bFzRit4W0s/4qD1JuJyh3khq4w5KIKnvfHi9G8oQgfGqBmf/ppU6+bkltSdx2FYYcgGGCoxutNiESz4FdxyjAD2GYQUvWp9zocJPYrKAtadKwVAgYgtWTVYw==</Q><DP>tR+BLYwbKxlu0KijthgL9avz0iC9qUpqgFxlL+A8BjK89vPHQvqU8sIcwLaWd7qxG2/oNi5qNeOSOd7qooTC+lBtes4XX/c8hl7F/0ng70av0yuR7tUGBcwSL9ICSp4x6cmG5RJkGgO8Sx33+h1T/uaiq7V9EF1u584Mx0bRXbn5ukOYmeGUcadQ7cSgwl4SAp6uOYASRWOeuJS1khR/3XwpJpu3xGXwr4kP5M0Akq9Yp+iBClGkCTAQ53O6nb19TK8bmU+hTqxnqtEW+eaFuG31/1Hc3cGwGc4L0JgN85zDf39GiAvWSTgjafJ9lwyRjo1K2fL2RWm1PjFVaplPVQ==</DP><DQ>gXe4xuSPwyUkfp5qbwef1qg6sbOPbneIPdXznWzQtD9jc6ThkFVF6R54+gwnmHV3wEn9yvwv9jr9+veyaUj6nHrE87nDSdaganaLJ0bJwJ/qyhBa9P4j9TCxZyin/cbAd2BqH8ZTnQel/26+X4T2fcGVbOzsZ8BFRf2E6nuO1J2U6R4Lz86E4MxTBOz1KUXuC9iDdPMedUQiZEQpVVA6dpNOnWGXeq1Fep7klxtKpZ0l/A9AKXfvE+Mm+wNa5x1Wx4KPS0kml+W7FwrVJzOO0yXP6N2lxqM3gUNGZmay75qCxgVTmogXvgq/Qkts7Atvfp1xTKXP1kVdATmo8FRGXw==</DQ><InverseQ>vmTXWNmyAv1cn4LVzvK9ZJ2stK4nFTbfV8Hpus8vw+wlTqlSejoq05/rnASEqZ4lP32oXyO0cm/SDjKHD9XbhbYi2tYCIvBt5Su+tZy5br8Kp1B4NlfGlWgMT6CMmRUu1DdaTZKunOkQsJgIhJ4mXYhXy5nuzw/RvsUS5I1yboz/OF10RnNymaLA3PWvqDGXoytd+El0Ghc+XiXKad6qFXzwTuPVWxF3H95bGoRZZSttgMvNcs4I/U4rdGB5S2ZgDYtXutTA72cF1S2CMiuJrWQW1AGExjMFEuaJtUqj0FDjfPvcQfxCxiv29MQS3MJi+n+gIVszeMeiZMWoiEg4WQ==</InverseQ><D>DkemM/P6jIZKdANphv2NtBlwA2cbDBjQZ+l0tBgR5GNA1h2FnUht/mHiANZZxCVb7yQ8VjJSz0HXqkSxam9nGDaFkoBCDH4FWNTaRgeyBaYO/JwH2ywBzau1M9pjBPOXPRcAsQGHCcTrRsBBOeGwwb5vK7tBp6WWAZvILgKBoCWWPuCPCygpBV5xaojY4HfDaN1WfpaXn879T5z1CJJaK3J6Hddm1tCot0QNOM4sStSEDxtw7Xp5NPDQB+zQQIkLyhsYh2TWSyaE4t0G+9YTcGPP10laot8nXml/wJzVUtjBZnTN7EXwe4D+rOsvhE6ulsPK+KVLE3WpabmGU5Nbynp6aDLXttQVNQPh7Tb4eUMtxycrW5RpYDXoXXC8NkXMf5cQP+ww/NqioCOWDd6rFsZGjD1Ar4wIHXU1UlumDKBEJ1dMx0OxFjMSEiL0jW/sGFBhwnBL+BeWoZvVPHNmm6F+AkOV4mtErvG9BS7RxbEJhmYx9OkJyvGgnRZ05xIqJLjeiK8nQFpweXDbJob2G3amFt/cgor0Mle5RZ8roT/jX4F2CkEluujG7aeFXfeUcGSvEa3eyVzAkyoXpovyoP/Wdd+aVERH93T9Xmvy3zc7x/bhl+2q4ORmcVD9XMRA1ebbPS0oZ3iUaOIbf3btDJSndb35kyw8UsYGtGkJAek=</D></RSAKeyValue>";

        private const string _RegisteredDePublicKey = @"<RSAKeyValue><Modulus>z96OSCdZS3HAUk1ikqbcNjJbycwmRVgrmiufixprxKWl8XNPUwc5eVkkuh5Aiw+ENDM2dkW/edx73aKopMWFgj7HA7l08orGhJMj7BhvKkJjOu773n3ayoLfX9heh+/CAOAHK+qDoNG7EZ9urS0kq/Y5xUKe3/c1vuVqx1vPVdi+MVbJADIWLoN62WcpwyEHI2MJH75EKTYjpRQVTdV8MiWcltlOPcF1rrZZbMNG4KVaJzz5XmbUQsmAiAQSyOCiFYfikT0agqIWM5d7Jh08qyUGjix32tE4a2OAbsyNaWVJdue3XNXfLznYGhvCPC2+2H99l79gqPt55ACZgwwIIysIg51kCKKd3wmp0AJoh/SCEPKYn+C/11cIPMZDIx36Vo7DfRQkj9tAy0iu82bw1Qbdm1yQQQZagpci6Fgloa+OCB5aXEf1WdDRxJi1+CLbCoRjFhezVDfFvDNSNaUZUiIYE0iy22zWOOWMDOlRwLOVeGpunzgMRV+FzAPdF+O8Ifz/yqB4E11oL9JW0BwDQ4VCFx6EYlP5353bXnT6hAchk/h4OomJ4nUKZu/L82Xu7/R41cKzA805MLUQSzmZqfLDy01WzSP41MPTKqiI9tVoUcqBH6vUju1tRMa6JmD8aq7otW9l3Tel4KljRBdHN9SuKCk1OaUmsYnYYse/99c=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string _RegDePrivateKey = @"<RSAKeyValue><Modulus>z96OSCdZS3HAUk1ikqbcNjJbycwmRVgrmiufixprxKWl8XNPUwc5eVkkuh5Aiw+ENDM2dkW/edx73aKopMWFgj7HA7l08orGhJMj7BhvKkJjOu773n3ayoLfX9heh+/CAOAHK+qDoNG7EZ9urS0kq/Y5xUKe3/c1vuVqx1vPVdi+MVbJADIWLoN62WcpwyEHI2MJH75EKTYjpRQVTdV8MiWcltlOPcF1rrZZbMNG4KVaJzz5XmbUQsmAiAQSyOCiFYfikT0agqIWM5d7Jh08qyUGjix32tE4a2OAbsyNaWVJdue3XNXfLznYGhvCPC2+2H99l79gqPt55ACZgwwIIysIg51kCKKd3wmp0AJoh/SCEPKYn+C/11cIPMZDIx36Vo7DfRQkj9tAy0iu82bw1Qbdm1yQQQZagpci6Fgloa+OCB5aXEf1WdDRxJi1+CLbCoRjFhezVDfFvDNSNaUZUiIYE0iy22zWOOWMDOlRwLOVeGpunzgMRV+FzAPdF+O8Ifz/yqB4E11oL9JW0BwDQ4VCFx6EYlP5353bXnT6hAchk/h4OomJ4nUKZu/L82Xu7/R41cKzA805MLUQSzmZqfLDy01WzSP41MPTKqiI9tVoUcqBH6vUju1tRMa6JmD8aq7otW9l3Tel4KljRBdHN9SuKCk1OaUmsYnYYse/99c=</Modulus><Exponent>AQAB</Exponent><P>6Pu48LZM/eNBpVnSDUCm1K/rig5vl7vw/angmAxydEiZxbFKKA9JpGdTmblgtQTQ/ivDMe2B56QjY8caw9UASo3oxd6rwexZUn36atxvGB+EJ51YM0i0p+lfFp05mkva3CjsbymOBN5SNrcOeYwkLXrqTsoWrTpIWXreZLBVftAUgmJ1c7Ia2m0b7ifp1ZAsThB+eKzApACqMH2UYXkyLLxJhGsM6F3GjZ4eKLz5ZruPd9M2r8xeAoITEvhAMkmgQ7qQJ+kI3D+ypDB0o1hX0bFX5JRfRgLL35I00/vpm6pFZHTwl+JkmX12XfsS9RwmNMZ/a8cmxsWhLgdA2EILow==</P><Q>5GewIDIGKI2GjvWB41oJZfI0gtbZjlIxsVxZCPWYRF0gDIHwETKH9VK4DvqCvoykvAyMFCBIbAHI4/n48lq+Tq8/7tKVVScKY956p+87qxa5gk3uNypsNMwotnCBTN0JjACCfwIwPKWP8hCA37aYLiF8R6cQYUYbKQp8Q8JMLt5uQRWJDgw+z2LWUjaxsa+9XmeJcFHuss8+0Xme1coqXDFu6SXbg/K6muCuMs9QtQs4OLhIlo8k/4fLl7vkvFI3hnJ/Lw9OLbWaMSMUyJGGrzcxVs8uwS7m+Uq5IYWOECCn0S30cW17yrM7tyzs0bXjk5lSKoXgR9LqfDJYaSMmPQ==</Q><DP>E0ARRNpbPDMVznrAb1XjMvmiJZMRx2DBBcSOiSGmJ1OEWSBP90VkGVBsSOxXQD24ovestihgrmoSfoEKBhpIXuCg1hCS8n/71WQRV9kE2OJpwfgvPHWKb8FJmQ2+n7Aa0kwTVRAC6wYPlvPDH2nj51obmAz8mK2TIsmTLJChT8wTlb5a5AdYTqnrP99OY9X4wy57tK7Zb/OaHE2UAAXKjoW0MVvDAkQVTsg8x7LtjH582TK7dwUU03I57zxR2ZXZxx7YIGQR1ljxAr36NTDseKgFkh5sTNWYUM28zbMn1zPXbfh3lKUhGMmUCSngpB4CTiQEjTw0SQI1Uh9JTVUPrw==</DP><DQ>ZC7+2ABZJyx8mvQg1uJFQQwt8D3hC0YOOedxvjZLZaEbT6Em9cQeUoLH7PoAoyf5kepG/wTx/z4BKc4ZXeRjmQvRlSWVDtai/g816bdLis3a7MbV+CiJcdci/HL4pAhICbqngqIpGlDchKasgHQM6B8T7jHfQ2uGuke5Hdd5pw01eyLBDQJeAoUt0L3gzzlwbJopdLTbaF7zBNq9yrR0RCACsA1E7eln5Ess3WiF1ANp06cxX6jF57dem910hQ3jAPvzwWaLOg1v5qGmmhsK4ovo/lS+A0pZUXtvHL8CAxzvvxbTI3WMWOqpBL2V2p5XhgQ4QCKcr6RZ6cQDd4pNWQ==</DQ><InverseQ>EEz9SftuZ3HWL6XsHUFQQPhHKg3fj49yh282fdAAT3MwPkDO/BOYMB/AmRW2tzijY0US7Pn0j/l/jR1OEe+21u7jm7tHmnnUYkjqDEtnNQ4KUj1bG+cnXTfO5c7Z2wLC7nt9yL20OfOO6PDNjmJ2dhfVBaRAwm1LG5K68VSNCYCm49lw8WuV2gZXPcpd4UDMtgr0w+LppUZkX89/6K/J6CdoRwEo6vAu7QR1vOsJDSoSK55q8dibq3Qiplhx2PzHLloZF2A/iPG29+RBSplIaRTJ0Ljz42897wQnNLmjokGLgvO2J8sQLiyybRgApzK/mcUKnUbYlz0u30Zp8pJzdg==</InverseQ><D>sqQvXu8CHIY7o/+BUUs5QRJyM7DQyxOFFU+cIy2npC4/uItChrZUvGbR22mYSmohUcMZcPdsIMxNXyIlEMX3gQF2g0rkqHR/OvxBCOvOzWCUatdrecBrQVLLqVEHnId+EMZ3I1S9nn3f6Ls9oHKFa4uGBnLEmvGXLOF4rK/INZy5hylwQEzLJ1ozP5cbGujNe6nm83LOnSQ76eiijmuD+oy8UB+c0BHskyN/IquHxBQWsFYEcQ6qKGOHpFzrz9rNMPfAzNTYbZ/iuJ/cY9sIgoSlZ5Xrww+/DvtKKu58MTGsuxVUTeI3lhx+DaIBgTpHI0QdqUX9Sfwkur+RLkAu1I3oEDLfNg7rqQ3eX3XlkxnYzOx6/XQguIMSh6WClfaviqu6xtnZt2wU46YXW5PqSLpT7FoD8QKPlAXQq64s5OYOxpHFlOqjpp8CZj9OJwSlWq/9+2qHHBuMh5vECkAl2gbKglCSgYzzjDtaGhat8Suxj5LB6Hy8DHWjpcfMbJ7zgf0tKUzWjDM+CNvjlTyrzhEzRXLX3gUYBSngIxE80CHzax+qwLUA1Q+1gjkZpOFoNYJ6+7KMeNElb7obi4UzMWNUd2C5uYGNu0pKQUUZ7QzhbHkiDkKQ+9ZoXVQdBHgPeie4yeqRxt/rewZFEOacbeCBioFiL+eXbhEKI2Llq/E=</D></RSAKeyValue>";


        
        
        /// <summary>  
        /// 签名  
        /// </summary>  
        /// <param name="content">待签名字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>签名后字符串</returns>  
        public static string sign(string content, string privateKey, string input_charset)
        {
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(content);
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = rsa.SignData(Data, sh);
            return Convert.ToBase64String(signData);
        }


        /// <summary>  
        /// 验签  
        /// </summary>  
        /// <param name="content">待验签字符串</param>  
        /// <param name="signedString">签名</param>  
        /// <param name="publicKey">公钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>true(通过)，false(不通过)</returns>  
        public static bool verify(string content, string signedString, string publicKey, string input_charset)
        {
            bool result = false;
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(content);
            byte[] data = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }

        #region BlockEncrypt 分块加密  RSA加密+base64
        /// <summary>
        /// 分块加密  RSA加密+base64  
        /// </summary>
        /// <param name="content">原文</param>
        /// <returns>加密后的密文字符串</returns>
        public static string BlockEncrypt(string content)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                string strpublicKey = publicKey;// publicKeyReaderXml();
                var inputBytes = Encoding.UTF8.GetBytes(content);//有含义的字符串转化为字节流
                rsaProvider.FromXmlString(strpublicKey);//载入公钥 publicKey_fpup
                int bufferSize = (rsaProvider.KeySize / 8) - 11;//单块最大长度
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    { //分段加密
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp, false);
                        //rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
                }
            }
        }
        #endregion


        #region BlockDecrypt 分块解密
        /// <summary>
        /// 分块解密
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <returns></returns>
        public static string BlockDecrypt(string encryptedInput)
        {
            if (string.IsNullOrEmpty(encryptedInput))
            {
                return string.Empty;
            }

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Convert.FromBase64String(encryptedInput);
                rsaProvider.FromXmlString(privateKey);
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
        #endregion


        /// <summary>  
        /// 加密  
        /// </summary>  
        /// <param name="resData">需要加密的字符串</param>  
        /// <param name="publicKey">公钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>明文</returns> 
        /// <summary>
        /// RSA的加密函数
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns></returns>
        public static string encryptData(string encryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public static string encryptData(string resData, string publicKey, string input_charset)
        {
            byte[] DataToEncrypt = Encoding.ASCII.GetBytes(resData);
            string result = encrypt(DataToEncrypt, publicKey, input_charset);
            return result;
        }

        public static string encryptData(string resData, string input_charset)
        {
            byte[] DataToEncrypt = Encoding.ASCII.GetBytes(resData);
            string result = encrypt(DataToEncrypt, publicKey, input_charset);
            return result;
        }
 
        /// <summary>  
        /// 解密  
        /// </summary>  
        /// <param name="resData">加密字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>明文</returns>  
        public static string decryptData(string decryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privateKey);
                PlainTextBArray = Convert.FromBase64String(decryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public static string decryptData(string resData, string privateKey, string input_charset)
        {
            byte[] DataToDecrypt = Convert.FromBase64String(resData);
            string result = "";
            for (int j = 0; j < DataToDecrypt.Length / 128; j++)
            {
                byte[] buf = new byte[128];
                for (int i = 0; i < 128; i++)
                {
 
                    buf[i] = DataToDecrypt[i + 128 * j];
                }
                result += decrypt(buf, privateKey, input_charset);
            }
            return result;
        }

        public static string decryptData(string resData, string input_charset)
        {
            byte[] DataToDecrypt = Convert.FromBase64String(resData);
            string result = "";
            for (int j = 0; j < DataToDecrypt.Length / 128; j++)
            {
                byte[] buf = new byte[128];
                for (int i = 0; i < 128; i++)
                {

                    buf[i] = DataToDecrypt[i + 128 * j];
                }
                result += decrypt(buf, privateKey, input_charset);
            }
            return result;
        }
 
        #region 内部方法  
 
        private static string encrypt(byte[] data, string publicKey, string input_charset)
        {
            RSACryptoServiceProvider rsa = DecodePemPublicKey(publicKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] result = rsa.Encrypt(data, false);
 
            return Convert.ToBase64String(result);
        }
 
        private static string decrypt(byte[] data, string privateKey, string input_charset)
        {
            string result = "";
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] source = rsa.Decrypt(data, false);
            char[] asciiChars = new char[Encoding.GetEncoding(input_charset).GetCharCount(source, 0, source.Length)];
            Encoding.GetEncoding(input_charset).GetChars(source, 0, source.Length, asciiChars, 0);
            result = new string(asciiChars);
            //result = ASCIIEncoding.ASCII.GetString(source);  
            return result;
        }
 
        private static RSACryptoServiceProvider DecodePemPublicKey(String pemstr)
        {
            byte[] pkcs8publickkey;
            //pemstr = pemstr.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "");//.Replace("\n", "").Replace("\r", "");
            pkcs8publickkey = Convert.FromBase64String(pemstr);
            if (pkcs8publickkey != null)
            {
                RSACryptoServiceProvider rsa = DecodeRSAPublicKey(pkcs8publickkey);
                return rsa;
            }
            else
                return null;
        }
 
        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {
            byte[] pkcs8privatekey;
            pemstr = pemstr.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "");//.Replace("\n", "").Replace("\r", "");
            pkcs8privatekey = Convert.FromBase64String(pemstr);
            if (pkcs8privatekey != null)
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                return rsa;
            }
            else
                return null;
        }
 
        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
 
            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading  
            byte bt = 0;
            ushort twobytes = 0;
 
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes  
                else
                    return null;
 
                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;
 
                twobytes = binr.ReadUInt16();
 
                if (twobytes != 0x0001)
                    return null;
 
                seq = binr.ReadBytes(15);        //read the Sequence OID  
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct  
                    return null;
 
                bt = binr.ReadByte();
                if (bt != 0x04)    //expect an Octet string  
                    return null;
 
                bt = binr.ReadByte();        //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count  
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key  
 
                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }
 
            catch (Exception)
            {
                return null;
            }
 
            finally { binr.Close(); }
 
        }
 
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
 
        private static RSACryptoServiceProvider DecodeRSAPublicKey(byte[] publickey)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"  
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------  
            MemoryStream mem = new MemoryStream(publickey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading  
            byte bt = 0;
            ushort twobytes = 0;
 
            try
            {
 
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes  
                else
                    return null;
 
                seq = binr.ReadBytes(15);       //read the Sequence OID  
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct  
                    return null;
 
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes  
                else
                    return null;
 
                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next  
                    return null;
 
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes  
                else
                    return null;
 
                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;
 
                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)  
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus  
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes  
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order  
                int modsize = BitConverter.ToInt32(modint, 0);
 
                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);
 
                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it  
                    binr.ReadByte();    //skip this null byte  
                    modsize -= 1;   //reduce modulus buffer size by 1  
                }
 
                byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes  
 
                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data  
                    return null;
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)  
                byte[] exponent = binr.ReadBytes(expbytes);
 
                // ------- create RSACryptoServiceProvider instance and initialize with public key -----  
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
 
            finally { binr.Close(); }
 
        }
 
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;
 
            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------  
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading  
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)  
                    binr.ReadByte();    //advance 1 byte  
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes  
                else
                    return null;
 
                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)    //version number  
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;
 
 
                //------  all private key components are Integer sequences ----  
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);
 
                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);
 
                // ------- create RSACryptoServiceProvider instance and initialize with public key -----  
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }
 
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer  
                return 0;
            bt = binr.ReadByte();
 
            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte  
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();    // data size in next 2 bytes  
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;        // we already have the data size  
            }
 
 
 
            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data  
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte  
            return count;
        }
 
        #endregion
 
        #region 解析.net 生成的Pem  
        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {
 
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            bool keySize1024 = (keyData.Length == 162);
            bool keySize2048 = (keyData.Length == 294);
            if (!(keySize1024 || keySize2048))
            {
                throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, (keySize1024 ? 29 : 33), pemModulus, 0, (keySize1024 ? 128 : 256));
            Array.Copy(keyData, (keySize1024 ? 159 : 291), pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }


        /// <summary>
        /// 将pem格式私钥(1024 or 2048)转换为RSAParameters
        /// </summary>
        /// <param name="pemFileConent">pem私钥内容</param>
        /// <returns>转换得到的RSAParamenters</returns>
        private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
        {
            if (string.IsNullOrEmpty(pemFileConent))
            {
                throw new ArgumentNullException("pemFileConent", "This arg cann't be empty.");
            }
            pemFileConent = pemFileConent.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "").Replace("\r", "");
            byte[] keyData = Convert.FromBase64String(pemFileConent);
 
            bool keySize1024 = (keyData.Length == 609 || keyData.Length == 610);
            bool keySize2048 = (keyData.Length == 1190 || keyData.Length == 1192);
 
            if (!(keySize1024 || keySize2048))
            {
                throw new ArgumentException("pem file content is incorrect, Only support the key size is 1024 or 2048");
            }
 
            int index = (keySize1024 ? 11 : 12);
            byte[] pemModulus = (keySize1024 ? new byte[128] : new byte[256]);
            Array.Copy(keyData, index, pemModulus, 0, pemModulus.Length);
 
            index += pemModulus.Length;
            index += 2;
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);
 
            index += 3;
            index += 4;
            if ((int)keyData[index] == 0)
            {
                index++;
            }
            byte[] pemPrivateExponent = (keySize1024 ? new byte[128] : new byte[256]);
            Array.Copy(keyData, index, pemPrivateExponent, 0, pemPrivateExponent.Length);
 
            index += pemPrivateExponent.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemPrime1 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemPrime1, 0, pemPrime1.Length);
 
            index += pemPrime1.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemPrime2 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemPrime2, 0, pemPrime2.Length);
 
            index += pemPrime2.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemExponent1 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemExponent1, 0, pemExponent1.Length);
 
            index += pemExponent1.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemExponent2 = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemExponent2, 0, pemExponent2.Length);
 
            index += pemExponent2.Length;
            index += (keySize1024 ? ((int)keyData[index + 1] == 64 ? 2 : 3) : ((int)keyData[index + 2] == 128 ? 3 : 4));
            byte[] pemCoefficient = (keySize1024 ? new byte[64] : new byte[128]);
            Array.Copy(keyData, index, pemCoefficient, 0, pemCoefficient.Length);
 
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }
        #endregion

        /*
        #region 私钥加密，公钥解密
        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="privateKey">RSA私钥 base64格式</param>
        /// <param name="contentData">待加密的数据</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static string EncryptWithPrivateKey(string privateKey, byte[] contentData, string algorithm = "RSA/ECB/PKCS1Padding")
        {
            return Convert.ToBase64String(EncryptWithPrivateKey(Convert.FromBase64String(privateKey), contentData, algorithm));
        }
        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="contentData">待加密的数据</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static byte[] EncryptWithPrivateKey(byte[] privateKey, byte[] contentData, string algorithm = "RSA/ECB/PKCS1Padding")
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKey);
            return Transform(privateKeyParam, contentData, algorithm, true);
        }
        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="publicKey">RSA公钥  base64格式</param>
        /// <param name="content">待解密数据 base64格式</param>
        /// <param name="encoding">解密出来的数据编码格式，默认UTF-8</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static string DecryptWithPublicKey(string publicKey, string content, string encoding = "UTF-8", string algorithm = "RSA/ECB/PKCS1Padding")
        {
            return Encoding.GetEncoding(encoding).GetString(DecryptWithPublicKey(Convert.FromBase64String(publicKey), Convert.FromBase64String(content), algorithm));
        }
        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="publicKey">RSA公钥</param>
        /// <param name="contentData">待解密数据</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static byte[] DecryptWithPublicKey(byte[] publicKey, byte[] contentData, string algorithm = "RSA/ECB/PKCS1Padding")
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(publicKey);
            return Transform(publicKeyParam, contentData, algorithm, false);
        }
        #endregion
        */
    }
}


