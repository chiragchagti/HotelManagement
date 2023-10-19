﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models.DTO
{
    public class HotelRoomDTO
    {
        public int Id { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        [Display(Name ="Total Rooms")]
        public int TotalRooms { get; set; }
        public string Images { get; set; }
        public IFormFileCollection Files { get; set; }

        public virtual Hotel Hotel { get; set; }

    }
}
