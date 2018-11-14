using System.Collections.Generic;

namespace Todo.Core.Entities
{
    public class ModelStateResult
    {
        public string Message { get; set; }

        public List<FaultyModel> FaultyModels { get; set; }
    }

    public class FaultyModel
    {
        public string PropertyName { get; set; }

        public List<string> Errors { get; set; }

        public FaultyModel()
        {
            Errors = new List<string>();
        }
    }
}
