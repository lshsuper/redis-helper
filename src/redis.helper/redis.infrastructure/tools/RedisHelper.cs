using System;
using System.Collections.Generic;
using System.Text;

namespace redis.helper.tools
{
    /// <summary>
    /// 静态
    /// </summary>
    public class RedisHelper
    {
        private static object _lockRedis = new object();
        private static RedisContext _context;
        /// <summary>
        /// 开放单例
        /// </summary>
        public static RedisContext Instance
        {

            get
            {
                if (_context != null)
                    return _context;
                lock (_lockRedis)
                {
                    if (_context != null)
                        return _context;
                    _context = new RedisContext();
                }

                return _context;
            }
        }

        #region +Extension

        #endregion
    }
}
