using redis.helper.tools;
using System;

namespace redis.helper
{
    public class a
    {
        public string id { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var client=RedisHelper.Instance;
            //client.Set("name","lsh");
            //Console.WriteLine(client.GeoAdd("beijing", "lsh", 123, 34));
            //Console.WriteLine(client.GeoAdd("beijing", "wpp", 135, 77));
            Console.WriteLine(client.StringSet("name","lsh"));
            Console.WriteLine(client.StringGet<string>("name"));
            Console.WriteLine("Hello World!");
        }
    }
}
