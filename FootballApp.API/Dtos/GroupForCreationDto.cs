namespace FootballApp.API.Dtos
{
    public class GroupForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public byte[] Image { get; set; }
        public string Location { get; set; }
        public GroupForCreationDto()
        {
            IsActive = true;
        }
    }
}