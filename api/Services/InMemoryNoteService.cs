using NotesApp.Api.Dtos;
using NotesApp.Api.Models;

namespace NotesApp.Api.Services;

// Registered as a singleton, so the lock guards against concurrent requests.
public class InMemoryNoteService : INoteService
{
    private readonly List<Note> _notes = [];
    private readonly Lock _lock = new();
    private int _nextId = 1;

    public InMemoryNoteService()
    {
        Create(new CreateNoteDto("Welcome", "This note lives in memory. Restart the API and it's gone.", true));
        Create(new CreateNoteDto("Next step", "Level 2 replaces this list with SQL Server.", false));
    }

    public IEnumerable<NoteDto> GetAll(string? search)
    {
        lock (_lock)
        {
            var query = _notes.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(n =>
                    n.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    n.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.UpdatedAt)
                .Select(ToDto)
                .ToList();
        }
    }

    public NoteDto? GetById(int id)
    {
        lock (_lock)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            return note is null ? null : ToDto(note);
        }
    }

    public NoteDto Create(CreateNoteDto dto)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            var note = new Note
            {
                Id = _nextId++,
                Title = dto.Title.Trim(),
                Content = dto.Content?.Trim() ?? string.Empty,
                IsPinned = dto.IsPinned,
                CreatedAt = now,
                UpdatedAt = now
            };
            _notes.Add(note);
            return ToDto(note);
        }
    }

    public NoteDto? Update(int id, UpdateNoteDto dto)
    {
        lock (_lock)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note is null) return null;

            note.Title = dto.Title.Trim();
            note.Content = dto.Content?.Trim() ?? string.Empty;
            note.IsPinned = dto.IsPinned;
            note.UpdatedAt = DateTime.UtcNow;
            return ToDto(note);
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _notes.RemoveAll(n => n.Id == id) > 0;
        }
    }

    private static NoteDto ToDto(Note n) =>
        new(n.Id, n.Title, n.Content, n.IsPinned, n.CreatedAt, n.UpdatedAt);
}
