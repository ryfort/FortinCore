using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Common.Dtos
{
    public class GithubUserRepo
    {
        public long Id { get; set; }
        public string Node_Id { get; set; }
        public string Name { get; set; }
        public string Html_Url { get; set; }
        public string Description { get; set; }
        public GithubUserDto Owner { get; set; }
    }
}
