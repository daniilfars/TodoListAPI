using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.RequestFeatures;

public class TaskQueryParameters
{
    public string? SearchTerm { get; set; } // поиск по заголовку/описанию
    public bool? IsCompleted { get; set; } // фильтр по статусу выполнения
    public DateTime? FromDate { get; set; } // задачи, созданные после этой даты
    public DateTime? ToDate { get; set; } // задачи, созданные до этой даты
    
    // Сортировка
    private static readonly string[] AllowedSortFields = { "title", "createdat", "iscompleted" };
    private string? _sortBy;

    public string? SortBy
    {
        get => _sortBy;
        set => _sortBy = AllowedSortFields.Contains(value?.ToLower()) ? value.ToLower() : null;
    }

    [RegularExpression("^(asc|desc)$", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
    public string? SortOrder { get; set; } = "asc";

    // Пагинация
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}