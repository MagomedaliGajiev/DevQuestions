namespace DevQuestions.Domain.Reports;

public enum ReportStatus
{
    /// <summary>
    /// Статус открыт.
    /// </summary>
    OPEN,

    /// <summary>
    /// Статус в раблоте.
    /// </summary>
    IN_PROGRESS,

    /// <summary>
    /// Статус решен.
    /// </summary>
    RESOLVED,

    /// <summary>
    /// Статус закрыт.
    /// </summary>
    DISMISSED,
}