using System;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class PhotoToReturnDto
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public DateTime DateADded { get; set; }
        public bool IsMain { get; set; }
    }
}