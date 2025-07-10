namespace DevQuestions.Domain.Questions;

public class Answer
{
    public Guid Id {  set; get; }
    public Guid UserId {  set; get; }
    public required string Text {  set; get; }
    public required Question Question { get; set; }
    public List<Guid> Comments { get; set; } = [];
}
