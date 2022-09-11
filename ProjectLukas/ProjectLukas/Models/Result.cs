using System;

namespace ProjectLukas.Models
{
    public class ResultRecord
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public string ExerciseId { get; set; }
        public int ResultPercent { get; set; }
        public int ResultScore { get; set; }
        public int ResultMax { get; set; }
        public string Skill { get; set; }
        public string Language { get; set; }
        public DateTime ResultDate { get; set; }
    }
}
