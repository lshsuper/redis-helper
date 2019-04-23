using redis.helper.tools;
using System;

namespace redis.helper
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.客户端对象获取
            var client=RedisHelper.Instance;
            //2.示例操作
            client.Set("name","lsh",null);
            Console.WriteLine(client.Get<string>("name"));
            Console.Read();
        }
    }
}
