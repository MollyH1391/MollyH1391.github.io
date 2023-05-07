using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.Dto.Home;

namespace WowDin.Frontstage.Models.ViewModel.Home
{
    public class ResponseViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Brands> Brand { get; set; }
        //public IEnumerable<string> Shop { get; set; }
        public string ResponseContent { get; set; }
    }
    
}
