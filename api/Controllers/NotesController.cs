using Microsoft.AspNetCore.Mvc;
using NotesApp.Api.Dtos;
using NotesApp.Api.Services;

namespace NotesApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(INoteService notes) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<NoteDto>> GetAll([FromQuery] string? search) =>
        Ok(notes.GetAll(search));

    [HttpGet("{id:int}")]
    public ActionResult<NoteDto> GetById(int id)
    {
        var note = notes.GetById(id);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public ActionResult<NoteDto> Create(CreateNoteDto dto)
    {
        var note = notes.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPut("{id:int}")]
    public ActionResult<NoteDto> Update(int id, UpdateNoteDto dto)
    {
        var note = notes.Update(id, dto);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) =>
        notes.Delete(id) ? NoContent() : NotFound();
}
