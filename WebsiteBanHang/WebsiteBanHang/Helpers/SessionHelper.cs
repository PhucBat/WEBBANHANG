using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebsiteBanHang.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void RemoveFromSession(this ISession session, string key)
        {
            session.Remove(key);
        }

        public static bool ExistsInSession(this ISession session, string key)
        {
            return session.GetString(key) != null;
        }
    }
} 