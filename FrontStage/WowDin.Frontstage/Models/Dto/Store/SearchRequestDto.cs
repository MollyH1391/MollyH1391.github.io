﻿namespace WowDin.Frontstage.Models.Dto.Store
{
    public class SearchRequestDto
    {
        public string Method { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Evaluate { get; set; }

    }
}