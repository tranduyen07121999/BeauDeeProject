using System.Collections.Generic;

namespace Data.ResponseModels
{
    public class ResponseModel<T>
    {
        public ResponseModel(ICollection<T> data)
        {
            Data = data;
            Message = "Successfull";
            Status = 200;
        }
        public string Message { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public ICollection<T> Data { get; set; }
    }
}
