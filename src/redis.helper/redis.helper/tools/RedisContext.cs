﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace redis.helper.tools
{

    /// <summary>
    /// 可IOC注入
    /// </summary>
    public class RedisContext
    {
        /// <summary>
        /// 链接对象
        /// </summary>
        private ConnectionMultiplexer _conn;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="type"></param>
        public RedisContext(RedisConfigType type= RedisConfigType.Option)
        {
            switch (type)
            {
                case RedisConfigType.ConnStr:
                    _conn = ConnectionMultiplexer.Connect(GetConfigStr());
                    break;
                case RedisConfigType.Option:
                    _conn = ConnectionMultiplexer.Connect(GetConfigOption());
                    break;
                default:
                    break;
            }
           
        }

        #region +Basic

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private ConfigurationOptions GetConfigOption()
        {
            //1.配置文件加载器
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("app.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            //2.获取redis配置并映射成对象
            var appConfig = new ServiceCollection()
               .AddOptions()
               .Configure<RedisConfig>(configuration.GetSection("redis"))
               .BuildServiceProvider()
               .GetService<IOptions<RedisConfig>>()
               .Value;
            //3.redis配置装备
            ConfigurationOptions options = new ConfigurationOptions();
            appConfig.Points.ForEach(ele =>
            {
                options.EndPoints.Add(ele);
            });
            options.Password = appConfig.Pwd;
            return options;
        }
        private string GetConfigStr()
        {
            return "";
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
        ///  插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Set(string key, object value,TimeSpan? time=null, int db = -1)
        {
            var server = GetDataBase(db);
            return server.StringSet(key, Serialize(value),time);
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
                return default(T);
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
            if (string.IsNullOrEmpty(value)) return default(T);
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
            if (string.IsNullOrEmpty(value)) return default(T);
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
