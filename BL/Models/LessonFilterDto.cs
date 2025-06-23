public class LessonFilterDto
{
    public string? Gender { get; set; }
    public int? age { get; set; }
    public DateOnly? SpecificDate { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Status { get; set; }
    public int? SubjectId { get; set; }
    public int? TeacherId { get; set; }
}
