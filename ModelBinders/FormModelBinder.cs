using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace HafifCMS.ModelBinders
{
    public class FormModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            IDictionary<string, object> obj = new ExpandoObject();
            IDictionary<string, object> fields = new ExpandoObject();
            IDictionary<string, object> files = new ExpandoObject();


            obj.Add("fields", fields);
            obj.Add("files", files);

            foreach (string key in request.Form.AllKeys)
            {
                ((IDictionary<string, object>)obj["fields"]).Add(key.Replace("field_", ""), request.Form.Get(key));
            }

            foreach (string key in request.Files.AllKeys)
            {
                ((IDictionary<string, object>)obj["files"]).Add(key.Replace("file_", ""), request.Files.Get(key));
            }


            return (dynamic)obj;
        }
    }
}