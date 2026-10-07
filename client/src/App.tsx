import { useEffect, useState, type SubmitEvent } from 'react'
import { notesApi } from './api/notes'
import type { Note } from './types'
import './App.css'

function App() {
  const [notes, setNotes] = useState<Note[]>([])
  const [search, setSearch] = useState('')
  const [title, setTitle] = useState('')
  const [content, setContent] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)

  async function load(term = search) {
    try {
      setError(null)
      setNotes(await notesApi.getAll(term || undefined))
    } catch (e) {
      setError((e as Error).message)
    } finally {
      setLoading(false)
    }
  }

  // Debounce search so we don't hit the API on every keystroke.
  useEffect(() => {
    const t = setTimeout(() => load(search), 250)
    return () => clearTimeout(t)
  }, [search])

  async function handleCreate(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault()
    if (!title.trim()) return
    try {
      await notesApi.create({ title: title.trim(), content, isPinned: false })
      setTitle('')
      setContent('')
      await load()
    } catch (e) {
      setError((e as Error).message)
    }
  }

  async function togglePin(note: Note) {
    try {
      await notesApi.update(note.id, { title: note.title, content: note.content, isPinned: !note.isPinned })
      await load()
    } catch (e) {
      setError((e as Error).message)
    }
  }

  async function handleDelete(id: number) {
    try {
      await notesApi.remove(id)
      setNotes((prev) => prev.filter((n) => n.id !== id))
    } catch (e) {
      setError((e as Error).message)
    }
  }

  const sorted = [...notes].sort(
    (a, b) => Number(b.isPinned) - Number(a.isPinned) || b.updatedAt.localeCompare(a.updatedAt),
  )

  return (
    <main className="app">
      <h1>Notes</h1>

      <form className="note-form" onSubmit={handleCreate}>
        <input
          placeholder="Title"
          value={title}
          maxLength={100}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <textarea
          placeholder="Content"
          value={content}
          maxLength={2000}
          rows={3}
          onChange={(e) => setContent(e.target.value)}
        />
        <button type="submit">Add note</button>
      </form>

      <input
        className="search"
        type="search"
        placeholder="Search notes…"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      {error && <p className="error">{error}</p>}
      {loading ? (
        <p>Loading…</p>
      ) : sorted.length === 0 ? (
        <p className="empty">No notes yet.</p>
      ) : (
        <ul className="notes">
          {sorted.map((note) => (
            <li key={note.id} className={note.isPinned ? 'note pinned' : 'note'}>
              <header>
                <h2>{note.title}</h2>
                <div className="actions">
                  <button onClick={() => togglePin(note)}>{note.isPinned ? 'Unpin' : 'Pin'}</button>
                  <button onClick={() => handleDelete(note.id)}>Delete</button>
                </div>
              </header>
              {note.content && <p>{note.content}</p>}
              <small>Updated {new Date(note.updatedAt).toLocaleString()}</small>
            </li>
          ))}
        </ul>
      )}
    </main>
  )
}

export default App
