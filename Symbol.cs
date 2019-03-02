using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MiniGIS
{
    public class Symbol
    {
        public StringFormat drawFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public Font Font = new Font("Webdings", 14);
        public int Number = 0x6E;//0x85 - инопланетянин; //Кружок - 0x6E
    }
}
