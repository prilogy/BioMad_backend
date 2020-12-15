using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using BioMad_backend.Data;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace BioMad_backend.Areas.Admin.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent LocalizationSection<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, ApplicationContext context, TModel model)
            where TModel : ILocalizedEntity<TResult>
            where TResult : Translation<TResult>, new()
        {
            var x = expression.Parameters.FirstOrDefault();
            var result = "<h4>Локализация</h4>" +
                         "<div>" +
                         "<table class='table'>" +
                         "<thead>" +
                         "<tr>" +
                         "<th>" +
                         "Культура" +
                         "</th>" +
                         "<th>" +
                         "Название" +
                         "</th>" +
                         "<th></th>" +
                         "</tr>" +
                         "</thead>" +
                         "<tbody>";
            foreach (var item in context.Cultures.ToList())
            {
                var matchedTranslation = model.Translations.FirstOrDefault(x => x.CultureId == item.Id);
                if (matchedTranslation != null)
                {
                    var editLink =
                        result += "<tr>" +
                                  "<td>" +
                                  matchedTranslation.Culture?.Key +
                                  "</td>" +
                                  "<td>" +
                                  (matchedTranslation as IWithName)?.Name +
                                  "</td>" +
                                  "<td>" +
                                  ActionLink(htmlHelper, "Изменить", "Edit", typeof(TResult).Name,
                                      new { Area = "Admin", matchedTranslation.Id }) + " | " +
                                  ActionLink(htmlHelper, "Удалить", "Delete", typeof(TResult).Name,
                                      new { Area = "Admin", matchedTranslation.Id }) +
                                  "</td>" +
                                  "</tr>";
                }
                else
                {
                    result += "<tr>" +
                              "<td>" +
                              item.Key +
                              "</td>" +
                              "<td>" +
                              ActionLink(htmlHelper, "Добавить", "Create", typeof(TResult).Name,
                                  new { Area = "Admin", baseEntityId = model.Id, cultureId = item.Id }) +
                              " </td>" +
                              "<td>" +
                              "</td>" +
                              "<td>" +
                              "</td>" +
                              "</tr>";
                }
            }

            result += "</tbody>" +
                      "</table>" +
                      "</div>";


            return new HtmlString(result);
        }

        private static string ActionLink(IHtmlHelper helper, string linkText, string actionName, string controllerName,
            object routeValues)
        {
            using var writer = new StringWriter();
            var h = helper.ActionLink(linkText, actionName, controllerName, routeValues);
            h.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}