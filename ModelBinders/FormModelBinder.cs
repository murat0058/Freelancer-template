using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace HafifCMS.ModelBinders
{
    public class FormModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var shouldPerformRequestValidation = controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;

            HttpRequestBase request = controllerContext.HttpContext.Request;

            NameValueCollection form;

            if (shouldPerformRequestValidation)
            {
                form = request.Form;
            }
            else
            {
                form = request.Unvalidated.Form;
            }

            IDictionary<string, object> obj = new ExpandoObject();
            IDictionary<string, object> fields = new ExpandoObject();
            IDictionary<string, object> files = new ExpandoObject();

            obj.Add("fields", fields);
            obj.Add("files", files);

            foreach (string key in form.AllKeys)
            {
                ((IDictionary<string, object>)obj["fields"]).Add(key.Replace("field_", ""), form.Get(key));
            }

            foreach (string key in request.Files.AllKeys)
            {
                ((IDictionary<string, object>)obj["files"]).Add(key.Replace("file_", ""), request.Files.Get(key));
            }

            return (dynamic)obj;
        }
    }
}