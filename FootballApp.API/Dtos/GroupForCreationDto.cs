namespace FootballApp.API.Dtos
{
    public class GroupForCreationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public GroupForCreationDto()
        {
            IsActive = true;
        }
    }
}