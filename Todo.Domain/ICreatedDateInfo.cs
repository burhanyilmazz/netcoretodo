using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Domain
{
    public interface ICreatedDateInfo
    {
        [DataType(DataType.DateTime)]
        DateTime CreatedDate { get; set; }
    }
}
