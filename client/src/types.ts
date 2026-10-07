// Mirrors NotesApp.Api.Dtos (ASP.NET serializes to camelCase; DateTime arrives as an ISO string).
export interface Note {
  id: number
  title: string
  content: string
  isPinned: boolean
  createdAt: string
  updatedAt: string
}

export interface NoteInput {
  title: string
  content: string
  isPinned: boolean
}
