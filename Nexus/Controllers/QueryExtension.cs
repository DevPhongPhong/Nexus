namespace Nexus.Controllers
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    public static class QueryExtension
    {
        public static IQueryable<T> Query<T>(this IQueryable<T> query, string? search) where T : class
        {
            // Nếu chuỗi tìm kiếm trống, trả về query gốc
            if (string.IsNullOrEmpty(search))
                return query;

            try
            {
                // Chuyển đổi chuỗi JSON thành JObject
                var searchCriteria = JsonConvert.DeserializeObject<JObject>(search);

                foreach (var criteria in searchCriteria)
                {
                    string column = criteria.Key;
                    string value = criteria.Value.ToString();

                    // Áp dụng bộ lọc động
                    if (int.TryParse(value, out int intVal))
                    {
                        query = query.Where(e => EF.Property<int?>(e, column) == intVal);
                    }
                    else if (DateTime.TryParse(value, out DateTime dateVal))
                    {
                        query = query.Where(e => EF.Property<DateTime?>(e, column) == dateVal);
                    }
                    else if (bool.TryParse(value, out bool boolVal))
                    {
                        query = query.Where(e => EF.Property<bool?>(e, column) == boolVal);
                    }
                    else
                    {
                        query = query.Where(e => EF.Property<string>(e, column).Contains(value));
                    }
                }
            }
            catch (JsonException)
            {
                // Nếu không chuyển được JSON, bỏ qua điều kiện lọc
            }

            return query;
        }
    }

}
