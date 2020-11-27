using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BioMad_backend.Helpers
{
    public class ControllerVersioningConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace?.Split(".").Last().ToLower();
            if (!apiVersion.StartsWith("v"))
            {
                apiVersion = "v1";
            }

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}