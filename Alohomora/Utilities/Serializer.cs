using Alohomora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Alohomora.Utilities
{
    public static class Serializer
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public static string PersonModelsToJson(IEnumerable<PersonModel> personModel)
        {
            return _serializer.Serialize(personModel);
        }

        public static IEnumerable<PersonModel> GetPersonModels(string json)
        {
            return _serializer.Deserialize(json, typeof(IEnumerable<PersonModel>)) as IEnumerable<PersonModel>;
        }
    }
}
