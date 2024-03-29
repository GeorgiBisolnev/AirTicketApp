﻿using AirTicketApp.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.AirportModelConstants;

namespace AirTicketApp.Models.AirportModels
{
    public class AirportViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(NameMaximimLength)]
        public string Name { get; set; } = null!;

        public City City { get; set; } = null!;

        [Required]
        public int CityId { get; set; }

        [Required]
        [StringLength(IATAexactlength)]
        public string IATACode { get; set; } = null!;
    }
}
