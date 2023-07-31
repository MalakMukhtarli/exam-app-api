namespace ExamApp.API.Models.Pupils;

public class PupilDto : BaseDto
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public byte Grade { get; set; }
}