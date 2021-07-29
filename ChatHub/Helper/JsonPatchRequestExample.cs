using ChatAPI.Models;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Helper
{
    public class JsonPatchRequestExample : IExamplesProvider<Operation[]>
    {
        public Operation[] GetExamples()
        {
            return new[]
            {
            new Operation
            {
                Op = "replace",
                Path = "/name",
                    Value = "Gordon"
            },
            new Operation
            {
                Op = "replace",
                Path = "/surname",
                    Value = "Freeman"
            }
        };
        }
    }

    public class Operation
    {
        public object Value { get; set; }

        public string Path { get; set; }

        public string Op { get; set; }
    }
}