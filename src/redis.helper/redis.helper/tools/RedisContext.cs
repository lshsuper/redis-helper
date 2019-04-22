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
    public class RedisContext:IDisposable
    {
        private ConnectionMultiplexer _conn;
        public RedisContext()
        {

            _conn = ConnectionMultiplexer.Connect(GetConfig());
        }

        #region +Disposable
        //析构函数，编译后变成 protected void Finalize()，GC会在回收对象前会调用调用该方法 
        ~RedisContext()
        {
            Dispose(false);
        }

        //通过实现该接口，客户可以显式地释放对象，而不需要等待GC来释放资源，据说那样会降低效率 
        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        //将释放非托管资源设计成一个虚函数，提供在继承类中释放基类的资源的能力 
        protected virtual void ReleaseUnmanageResources()
        {
            //Do something... 
            Console.WriteLine("Do something... ");
        }

        //私有函数用以释放非托管资源 
        private void Dispose(bool disposing)
        {
            ReleaseUnmanageResources();

            //为true时表示是客户显式调用了释放函数，需通知GC不要再调用对象的Finalize方法 
            //为false时肯定是GC调用了对象的Finalize方法，所以没有必要再告诉GC你不要调用我的Finalize方法啦 
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
        #endregion
        
        #region +Basic

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private ConfigurationOptions GetConfig()
        {
            ConfigurationOptions options = new ConfigurationOptions();
            options.Password = "";
            options.EndPoints.Add("");
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
        public bool Set(string key, object value, int db = -1)
        {
            var server = GetDataBase(db);
            return server.StringSet(key,Serialize(value));
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T Get<T>(string key, int db = -1)
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
        public long LPush(string key,object value,int db=-1)
        {
            var server = GetDataBase(db);
            return server.ListLeftPush(key,Serialize(value));
        }
        /// <summary>
        /// 列表左出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public T LPop<T>(string key,int db=-1)
        {
            var server = GetDataBase(db);
            string value= server.ListLeftPop(key);
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

    }
}
