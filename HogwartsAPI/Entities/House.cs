﻿using HogwartsAPI.Enums;

namespace HogwartsAPI.Entities
{
    public class House
    {
        public int Id { get; set; }
        public HouseName Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int TrophyCount { get; set; }
        public virtual IEnumerable<Student>? Students { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
    }
}
