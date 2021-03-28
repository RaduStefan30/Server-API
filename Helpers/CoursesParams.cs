using StudentApp.API.Models;

namespace StudentApp.API.Helpers
{
    public class CoursesParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 6;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value>MaxPageSize) ? MaxPageSize:value; }
        }
        public Assignation Assignation { get; set; }
    }
}