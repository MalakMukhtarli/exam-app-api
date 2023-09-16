namespace ExamApp.Business.Models.Pupils;

public class SavePupilRequest
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int GradeId { get; set; }
}