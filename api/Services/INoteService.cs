using NotesApp.Api.Dtos;

namespace NotesApp.Api.Services;

public interface INoteService
{
    IEnumerable<NoteDto> GetAll(string? search);
    NoteDto? GetById(int id);
    NoteDto Create(CreateNoteDto dto);
    NoteDto? Update(int id, UpdateNoteDto dto);
    bool Delete(int id);
}
