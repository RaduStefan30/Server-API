using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 6;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value>MaxPageSize) ? MaxPageSize:value; }
        }

        public int UserId { get; set; }
        public string Faculty { get; set; }
        public string Specialization { get; set; }
        public int Group { get; set; }
        public int Year { get; set; }


    }
}
