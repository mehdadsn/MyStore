using Store.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.HomePage
{
    public class HomePageImages : BaseEntity
    {
        public string Src { get; set; }
        public string Link { get; set; }
        public ImageLocation ImageLocation { get; set; }
    }

    public enum ImageLocation
    {
        TopL1 = 0,
        TopL2 = 1,
        R1 = 2,
        CenterFullWidth = 3,
        G1 = 4,
        G2 = 5,
    }
}
