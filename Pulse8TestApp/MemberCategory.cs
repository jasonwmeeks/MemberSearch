using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse8TestApp
{
    public class MemberCategory
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? MostSevereDiagnosisID { get; set; }
        public string MostSevereDiagnosisDescription { get; set; }
        public int? DiagnosisCategoryID { get; set; }
        public string CategoryDescription { get; set; }
        public int? CategoryScore { get; set; }
        public int IsMostSevereCategory { get; set; }
    }
}
