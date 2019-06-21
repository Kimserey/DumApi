using System;
using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DumApi
{
    public class JwtPayload
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("iss")]
        public string Issuer { get; set; }
        [JsonProperty("sub")]
        public string Subject { get; set; }
        [JsonProperty("exp")]
        public double Expiry { get; set; }
        [JsonProperty("iat")]
        public double IssuedAtTime { get; set; }
        [JsonProperty("aud")]
        public string Audiance { get; set; }
    }

    public class JwtOptions
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }

    public interface IJwtToken
    {
        string Generate(string principal);
        JwtPayload Decode(string token);
    }

    public class JwtToken: IJwtToken
    {
        static readonly JwsAlgorithm _algorithm = JwsAlgorithm.HS256;
        private readonly JwtOptions _options;

        public JwtToken(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(string principal)
        {
            DateTime start = new DateTime(1970, 1, 1);
            var exp = new TimeSpan(DateTime.Now.AddMonths(1).Ticks - start.Ticks).TotalSeconds;
            var iat = new TimeSpan(DateTime.Now.Ticks - start.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                Id = Guid.NewGuid().ToString("N"),
                Issuer = _options.Issuer,
                Subject = principal,
                Expiry = exp,
                IssuedAtTime = iat,
                Audiance = _options.Audience
            };

            return JWT.Encode(JsonConvert.SerializeObject(payload), Convert.FromBase64String(_options.Key), _algorithm);
        }

        public JwtPayload Decode(string token)
        {
            return JsonConvert.DeserializeObject<JwtPayload>(JWT.Decode(token, Convert.FromBase64String(_options.Key), _algorithm));
        }
    }
}
