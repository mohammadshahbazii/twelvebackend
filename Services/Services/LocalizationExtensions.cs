using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataLayer;

namespace Services
{
    public static class LocalizationExtensions
    {
        public static void ApplyTranslation<T>(this T entity, TwelveDbContext db)
            where T : class
        {
            if (entity == null) return;
            var culture = CultureInfo.CurrentUICulture.Name;
            var entityName = typeof(T).Name;
            var idProp = typeof(T).GetProperties().FirstOrDefault(p => p.Name == entityName + "Id");
            if (idProp == null) return;
            var id = (int)(idProp.GetValue(entity) ?? 0);
            var translations = db.EntityTranslations
                .Where(t => t.EntityName == entityName && t.EntityId == id && t.Culture == culture)
                .ToList();
            foreach (var tr in translations)
            {
                var prop = typeof(T).GetProperty(tr.Property);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(entity, tr.Value);
                }
            }
        }

        public static void ApplyTranslations<T>(this IEnumerable<T> entities, TwelveDbContext db)
            where T : class
        {
            foreach (var entity in entities)
            {
                entity.ApplyTranslation(db);
            }
        }
    }
}
