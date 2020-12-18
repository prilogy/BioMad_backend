using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            Expression<Func<TModel, TResult>> expression, ApplicationContext context, TModel model,
            bool forPartial = true,
            int selectedCultureId = default, int selectedTranslationId = default)
            where TModel : ILocalizedEntity<TResult>
            where TResult : Translation<TResult>, new()
        {
            var result = @$"<div class='card'>
                        <div class='card-header font-weight-bold p-3'>                        
                        Локализация
                        </div>
                         <div>
                         <table class='table'>
                         <thead>
                         <tr>
                        <th class='pl-3 font-weight-bold'>
                         Культура
                         </th>
                         <th class='font-weight-bold'>
                         Название
                         </th>
                         <th></th>
                         </tr>
                         </thead>
                         <tbody>";

            var controllerName = typeof(TModel)?.Name?.Replace("Proxy", "");
            var translationControllerName = typeof(TResult)?.Name?.Replace("Proxy", "");

            foreach (var item in context.Cultures.ToList())
            {
                var matchedTranslation = model.Translations.FirstOrDefault(x => x.CultureId == item.Id);
                if (matchedTranslation != null)
                {
                    var isSelectedTranslation = selectedTranslationId == matchedTranslation.Id;
                    result += @$"<tr style='background: {(isSelectedTranslation ? "#1554f622" : "none")}'>
                                  <td class='pl-3'>
                                  {matchedTranslation.Culture?.Key}
                                  </td>
                                  <td>
                                  {(matchedTranslation as IWithName)?.Name}
                                  </td>
                                  <td>"
                              + (forPartial
                                  ? HtmlContentToString(
                                      htmlHelper.TableRowActions(matchedTranslation, x => !isSelectedTranslation
                                          ? new List<TableActionButton>
                                          {
                                              new TableActionButton
                                              {
                                                  Text = "Подробнее",
                                                  Action = "Edit",
                                                  Controller = controllerName,
                                                  Arguments = new
                                                  {
                                                      Area = "Admin",
                                                      id = model.Id,
                                                      translationAction = FormActionType.Edit,
                                                      translationId = matchedTranslation.Id
                                                  }
                                              }
                                          }
                                          : new List<TableActionButton>(), false, false))
                                  : HtmlContentToString(
                                      htmlHelper.TableRowActions(matchedTranslation)))
                              + @"</td>
                                  </tr>";
                }
                else
                {
                    result += @$"<tr style='background: {(selectedCultureId == item.Id ? "#1554f622" : "none")}'>
                              <td class='pl-3'>
                              {item.Key}
                              </td>
                              <td>
                              {(forPartial
                        ? selectedCultureId == item.Id ? "" : ActionLink(htmlHelper, "Добавить", "Edit", controllerName,
                            new { Area = "Admin", id = model.Id, translationAction = FormActionType.Create, cultureId = item.Id })
                        : ActionLink(htmlHelper, "Добавить", "Create", translationControllerName,
                            new { Area = "Admin", baseEntityId = model.Id, cultureId = item.Id }))}
                              </td>
                              <td>
                              </td>
                             
                              </tr>";
                }
            }

            result += @$"</tbody>
                      </table>
                      </div>
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
                 <h1 class='m-0 col-md-6 mb-2 mb-md-0'>{header}</h1> 
                 <div class='d-flex col-md-6 justify-content-md-end'>
                         <form method='get' class='w-100'>
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
            bool editAction = true, bool deleteAction = true)
        {
            var id = model.GetType().GetProperty("Id")?.GetValue(model, null);
            var controllerName = model.GetType().Name.Replace("Proxy", "");

            var appended = appendButtons?.Invoke(model).Aggregate("", (acc, x) => acc +
                ActionLink(htmlHelper, x.Text,
                    x.Action, x.Controller, x.Arguments,
                    new { @class = "btn " + (x.Class ?? "btn-outline-primary") })) ?? "";

            var result = "<div class='btn-group btn-group-sm'>" +
                         (editAction
                             ? ActionLink(htmlHelper, "Подробнее", "Edit", controllerName, new { id },
                                 new { @class = "btn btn-outline-primary" })
                             : "") +
                         (deleteAction
                             ? ActionLink(htmlHelper, "Удалить", "Delete", controllerName, new { id },
                                 new { @class = "btn btn-outline-primary" })
                             : "") +
                         appended
                         + "</div>";
            return new HtmlString(result);
        }

        #endregion

        #region [ Table ]

        public static IHtmlContent Table<TModel>(
            this IHtmlHelper htmlHelper, IEnumerable<TModel> lst,
            Func<TModel, Dictionary<string, object>> func, Func<TModel, List<TableActionButton>> appendButtons = null,
            bool editAction = true, bool deleteAction = true, bool header = true)
            where TModel : new()
        {
            var list = lst.ToList();
            var showButtons = appendButtons != null || editAction || deleteAction;

            var headerDict = func(list.FirstOrDefault() ?? new TModel());
            var result = @$"
            <table class='table'>
                <thead>" 
                + (header ? @$"<tr>
                {headerDict.Aggregate("", (acc2, y) => acc2 + @$"
                        <th class='font-weight-bold {(y.Key == headerDict.FirstOrDefault().Key ? "pl-3" : "")}'>{y.Key}</th>
                        ")
                             }" +
                         (showButtons ? "<th></th>" : "") +
                         "</tr>" : "") +
                @"</thead>
                <tbody>"
                         + list.Aggregate("", (acc, x) => acc + @$"
                <tr>
                   {func(x).Aggregate("", (acc2, y) => acc2 + @$"
                        <td class='{(y.Key == headerDict.FirstOrDefault().Key ? "pl-3" : "")}'>{y.Value?.ToString()}</td>
                        ")
                                                                  }"
                                                              + (showButtons
                                                                  ? "<td>" + HtmlContentToString(
                                                                      htmlHelper.TableRowActions(x, appendButtons,
                                                                          editAction, deleteAction)) + "</td>"
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