using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GoodsSpike
    {
        //string g.startDate, g.startTime, g.endDate, g.endTime, g.spikeGoodsId, g.activityNum, g.spikePrice, g.spikeType, g.subtitle
        public string spikeGoodsId { get; set; }
        public string erpGoodsId { get; set; }
        public int spikeType { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public int activityNum { get; set; }
        public string spikePrice { get; set; }
        public int spikelimitMax { get; set; } //spikelimitMax
        public string subtitle { get; set; }
        public string originalPrice { get; set; }
         

    }
}
