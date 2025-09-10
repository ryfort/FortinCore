using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Proxy
{
    public class GithubUserDto
    {
        public string Login { get; set; }
        public long Id { get; set; }
        public string Avatar_Url { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
