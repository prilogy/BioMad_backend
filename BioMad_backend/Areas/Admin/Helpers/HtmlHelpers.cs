using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.Pkcs;
using System.Text.Encodings.Web;
using BioMad_backend.Areas.Admin.Models;
using BioMad_backend.Data;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BioMad_backend.Areas.Admin.Helpers
{
    public static class HtmlHelpers
    {
        #region [ Localization section helper ]

        public static IHtmlContent LocalizationSection<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, ApplicationContext context, TModel model)
            where TModel : ILocalizedEntity<TResult>
            where TResult : Translation<TResult>, new()
        {
            var result = @$"<h4>Локализация</h4>
                         <div>
                         <table class='table'>
                         <thead>
                         <tr>
                        <th>
                         Культура
                         </th>
                         <th>
                         Название
                         </th>
                         <th></th>
                         </tr>
                         </thead>
                         <tbody>";

            var controllerName = typeof(TResult)?.Name?.Replace("Proxy", "");

            foreach (var item in context.Cultures.ToList())
            {
                var matchedTranslation = model.Translations.FirstOrDefault(x => x.CultureId == item.Id);
                if (matchedTranslation != null)
                {
                    result += @$"<tr>
                                  <td>
                                  {matchedTranslation.Culture?.Key}
                                  </td>
                                  <td>
                                  {(matchedTranslation as IWithName)?.Name}
                                  </td>
                                  <td>"
                              + HtmlContentToString(
                                  htmlHelper.TableRowActions(matchedTranslation))
                              + @"</td>
                                  </tr>";
                }
                else
                {
                    result += @$"<tr>
                              <td>
                              {item.Key}
                              </td>
                              <td>
                              {ActionLink(htmlHelper, "Добавить", "Create", controllerName,
                        new { Area = "Admin", baseEntityId = model.Id, cultureId = item.Id })}
                              </td>
                              <td>
                              </td>
                              <td>
                              </td>
                              </tr>";
                }
            }

            result += @$"</tbody>
                      </table>
                      </div>";


            return new HtmlString(result);
        }

        #endregion

        #region [ EntityIndexHeader ]

        public static IHtmlContent EntityIndexHeader<TModel>(
            this IHtmlHelper<TModel> htmlHelper, string header, string searchString = default)
        {
            var type = typeof(TModel).GetInterface(nameof(IEnumerable)) == null
                ? typeof(TModel)
                : typeof(TModel).GetGenericArguments()[0];

            var controllerName = type.Name.Replace("Proxy", "");

            var result = @$"
                <div class='row d-flex justify-content-between align-items-center mb-3'>
                 <h1 class='m-0 col-12 col-md-4 mb-2 mb-md-0'>{header}</h1> 
                 <div class='d-flex col-12 col-md-6 justify-content-end'>
                         <form method='get'>
                <div class='input-group'>
                <input type='text' class='form-control' placeholder='Поиск' aria-label='Поиск' value='{searchString}' name='searchString'>"
                         +
                         (searchString == default
                             ? ""
                             : "<div class='input-group-append'>" +
                               ActionLink(htmlHelper, "✖", "Index", controllerName, null,
                                   new { @class = "btn btn-delete" })
                               + "</div>"
                         ) +
                         @$"<div class='input-group-append'>
                <input value='Найти' type='submit' class='btn btn-primary'>
                </div></div></form>"
                         +
                         ActionLink(htmlHelper, "Добавить", "Create", controllerName, null,
                             new { @class = "btn btn-primary ml-2" })
                         +
                         @"</div>
                </div>";

            return new HtmlString(result);
        }

        #endregion

        #region [ TableRowActions ]

        public static IHtmlContent TableRowActions<TModel>(
            this IHtmlHelper htmlHelper, TModel model, Func<TModel, List<TableActionButton>> appendButtons = null,
            List<TableActionButtonType> actionTypes = null)
        {
            actionTypes ??= new List<TableActionButtonType>
                { TableActionButtonType.Delete, TableActionButtonType.Edit };

            var id = model.GetType().GetProperty("Id")?.GetValue(model, null);
            var controllerName = model.GetType().Name.Replace("Proxy", "");

            var result = "<div class='btn-group btn-group-sm'>" +
                         (actionTypes.Contains(TableActionButtonType.Edit)
                             ? ActionLink(htmlHelper, "Изменить", "Edit", controllerName, new { id },
                                 new { @class = "btn btn-outline-primary" })
                             : "") +
                         (actionTypes.Contains(TableActionButtonType.Delete)
                             ? ActionLink(htmlHelper, "Удалить", "Delete", controllerName, new { id },
                                 new { @class = "btn btn-outline-primary" })
                             : "") +
                         (appendButtons != null
                             ? appendButtons(model).Aggregate("", (acc, x) => 
                                 ActionLink(htmlHelper, x.Text,
                                 x.Action, x.Controller, x.Arguments,
                                 new { @class = "btn btn-outline-primary" }))
                             : "")
                         + "</div>";
            return new HtmlString(result);
        }

        #endregion

        #region [ Table ]
        
        public static IHtmlContent Table<TModel>(
            this IHtmlHelper htmlHelper, IEnumerable<TModel> lst,
            Func<TModel, Dictionary<string, object>> func, Func<TModel, List<TableActionButton>> appendButtons = null, List<TableActionButtonType> defaultActionTypes = null)
            where TModel : new()
        {
            defaultActionTypes ??= new List<TableActionButtonType>
                { TableActionButtonType.Delete, TableActionButtonType.Edit };

            var list = lst.ToList();

            var result = @$"
            <table class='table'>
                <thead>
                <tr>
                {func(list.FirstOrDefault() ?? new TModel()).Aggregate("", (acc2, y) => acc2 + @$"
                        <th class='font-weight-bold'>{y.Key}</th>
                        ")
                             }" +
                         (defaultActionTypes.Count > 0 ? "<th></th>" : "") +
                         @$"</tr>
                </thead>
                <tbody>"
                         + list.Aggregate("", (acc, x) => acc + @$"
                <tr>
                   {func(x).Aggregate("", (acc2, y) => acc2 + @$"
                        <td>{y.Value?.ToString()}</td>
                        ")
                                                                  }"
                                                              + (defaultActionTypes.Count > 0
                                                                  ? "<td>" + HtmlContentToString(
                                                                      htmlHelper.TableRowActions(x, appendButtons, defaultActionTypes)) + "</td>"
                                                                  : "") +
                                                              "</tr>"
                         )
                         + @$"</tbody>
                </table>
            ";

            return new HtmlString(result);
        }

        #endregion

        #region [ Helper methods ]

        private static string ActionLink(IHtmlHelper helper, string linkText, string actionName, string controllerName,
            object routeValues, object htmlAttributes = null)
            => HtmlContentToString(helper.ActionLink(linkText, actionName, controllerName, routeValues,
                htmlAttributes));

        private static string HtmlContentToString(IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        #endregion
    }
}