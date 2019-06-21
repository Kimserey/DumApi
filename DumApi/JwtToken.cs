using System;
using Jose;
using Newtonsoft.Json;

namespace DumApi
{
    public class JwtPayload
    {
        public string Id { get; set; }
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public double Expiry { get; set; }
        public double IssuedAtTime { get; set; }
        public string Audiance { get; set; }
    }

    public static class JwtToken
    {
        public static readonly string KEY = "ZmJjNTVmMmEtOTk5Yy00NDk0LTg1ZTktN2IwNWJlOWE4OWY0";
        public static readonly string AUDIENCE = "com.dumapi";
        public static readonly string ISSUER = "com.dumapi";
        static readonly JwsAlgorithm _algorithm = JwsAlgorithm.HS256;

        public static string Generate(string principal)
        {
            DateTime start = new DateTime(1970, 1, 1);
            var exp = new TimeSpan(DateTime.Now.AddMonths(1).Ticks - start.Ticks).TotalSeconds;
            var iat = new TimeSpan(DateTime.Now.Ticks - start.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                Id = Guid.NewGuid().ToString("N"),
                Issuer = ISSUER,
                Subject = principal,
                Expiry = exp,
                IssuedAtTime = iat,
                Audiance = AUDIENCE
            };

            return JWT.Encode(JsonConvert.SerializeObject(payload), Convert.FromBase64String(KEY), _algorithm);
        }

        public static JwtPayload Decode(string token)
        {
            return JsonConvert.DeserializeObject<JwtPayload>(JWT.Decode(token, Convert.FromBase64String(KEY), _algorithm));
        }
    }
}
