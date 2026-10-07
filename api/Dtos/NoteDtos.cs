using System.ComponentModel.DataAnnotations;

namespace NotesApp.Api.Dtos;

public record NoteDto(int Id, string Title, string Content, bool IsPinned, DateTime CreatedAt, DateTime UpdatedAt);

public record CreateNoteDto(
    [Required, StringLength(100, MinimumLength = 1)] string Title,
    [StringLength(2000)] string Content,
    bool IsPinned);

public record UpdateNoteDto(
    [Required, StringLength(100, MinimumLength = 1)] string Title,
    [StringLength(2000)] string Content,
    bool IsPinned);
