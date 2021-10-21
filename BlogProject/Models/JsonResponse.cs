using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    public class JsonResponse
    {
        public OperationStatu Code { get; set; }

        public string Message { get; set; }

        // bunu öylesine yazdık....
        public string TempProp { get; set; }
    }

    public enum OperationStatu
    {
        Success,  // Success benim için başarılı
        Error,//Benim için başarısız işlemdir.. bunda sonraki tipleri öylesine yazdık
        EmailError,
        ClientSideError
    }
}
