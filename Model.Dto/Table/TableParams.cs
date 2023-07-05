using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Table
{
    public class TableParams
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public string OrderBy { get; set; } = string.Empty;
        public bool IsAsc { get; set; }
        public int Pag { get; set; } = 1;

    }
}
