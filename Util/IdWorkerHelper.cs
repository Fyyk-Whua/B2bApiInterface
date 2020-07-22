using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class IdWorkerHelper
    {
        private static Util.Snowflake.IdWorker _idWorker = null;
        private IdWorkerHelper()
        {

        }

        static IdWorkerHelper()
        {
            _idWorker = new Util.Snowflake.IdWorker(1, 1);
        }
        public static long GenId64()
        {
            return _idWorker.NextId();

        }
    }

}
