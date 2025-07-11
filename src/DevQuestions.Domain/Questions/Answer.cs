namespace DevQuestions.Domain.Questions;

public class Answer
{
    public Guid Id {  get; set; }
    public Guid UserId { get; set; }
    public required string Text { set; get; }
     public required Question Question { get; set; }
    public List<Guid> Comments { get; set; } = [];
}
