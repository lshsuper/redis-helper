using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using redis.helper.tools;
using System;
using System.Collections.Generic;
using System.Linq;
namespace redis.helper
{
   
    class Program
    {
        static void Main(string[] args)
        {

            //1.客户端对象获取
            var client = RedisHelper.Instance;
            //2.示例操作
            client.Set("name", 5);
            Console.WriteLine(client.Get<string>("name"));
            Console.Read();
        }
    }
}
