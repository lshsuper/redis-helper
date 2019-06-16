using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace redis.helper.tools
{

    /// <summary>
    /// 可IOC注入
    /// </summary>
    public class RedisContext
    {
        private ConnectionMultiplexer _conn;
        public RedisContext()
        {

            _conn = ConnectionMultiplexer.Connect(GetConfig());
        }




        #region +Basic

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private ConfigurationOptions GetConfig()
        {
            ConfigurationOptions options = new ConfigurationOptions();
            //options.Password = "";
            options.EndPoints.Add("localhost");
            return options;
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private IDatabase GetDataBase(int db)
        {
            return _conn.GetDatabase(db);
        }
        private string Serialize(object str)
        {
            return JsonConvert.SerializeObject(str);
        }
        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion

        #region +String
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool StringSet(string key, object value, int db = -1)
        {
            var server = GetDataBase(db);
            return server.StringSet(key, Serialize(value));
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T StringGet<T>(string key, int db = -1)
        {
            var server = GetDataBase(db);
            string value = server.StringGet(key);
            if (string.IsNullOrEmpty(value))
                return default;
            return Deserialize<T>(value);
        }
        #endregion

        #region +List
        /// <summary>
        /// 列表左进
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public long LPush(string key, object value, int db = -1)
        {
            var server = GetDataBase(db);
            return server.ListLeftPush(key, Serialize(value));
        }
        /// <summary>
        /// 列表左出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T LPop<T>(string key, int db = -1)
        {
            var server = GetDataBase(db);
            string value = server.ListLeftPop(key);
            if (string.IsNullOrEmpty(value)) return default;
            return Deserialize<T>(value);
        }
        /// <summary>
        /// 列表右进
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public long RPush(string key, object value, int db = -1)
        {
            var server = GetDataBase(db);
            return server.ListRightPush(key, Serialize(value));
        }
        /// <summary>
        /// 列表右出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T RPop<T>(string key, int db = -1)
        {
            var server = GetDataBase(db);
            string value = server.ListRightPop(key);
            if (string.IsNullOrEmpty(value)) return default;
            return Deserialize<T>(value);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region +Set

        #endregion

        #region +Lock

        #endregion
        #region +Geo
        public bool GeoAdd(string key, string member, double lng, double lat, int db = -1)
        {
            var server = GetDataBase(db);
            return server.GeoAdd(key,lng,lat, member);
        }
        public GeoPosition?[] GeoPositions(string key,RedisValue []members,int db=-1)
        {
            var server = GetDataBase(db);
          return  server.GeoPosition(key, members);
        }
        public GeoPosition? GetPosition(string key, RedisValue member, int db = -1)
        {
            var server = GetDataBase(db);
            return server.GeoPosition(key, member);
        }
        public GeoRadiusResult[] GeoRedius(string key,
                              double lng,
                              double lat,
                              double radius,
                              GeoUnit unit=GeoUnit.Miles,
                              int count=-1,Order order=Order.Ascending, int db = -1)
        {
            var server = GetDataBase(db);
            return server.GeoRadius(key,lng,lat,radius,unit,count,order);
        }
        #endregion

    }
}
