import type { Note, NoteInput } from '../types'

const BASE_URL = `${import.meta.env.VITE_API_URL}/api/notes`

async function request<T>(url: string, init?: RequestInit): Promise<T> {
  const res = await fetch(url, {
    ...init,
    headers: { 'Content-Type': 'application/json', ...init?.headers },
  })
  if (!res.ok) {
    throw new Error(`${init?.method ?? 'GET'} ${url} failed: ${res.status} ${res.statusText}`)
  }
  return (res.status === 204 ? undefined : await res.json()) as T
}

export const notesApi = {
  getAll: (search?: string) =>
    request<Note[]>(search ? `${BASE_URL}?search=${encodeURIComponent(search)}` : BASE_URL),

  getById: (id: number) => request<Note>(`${BASE_URL}/${id}`),

  create: (input: NoteInput) =>
    request<Note>(BASE_URL, { method: 'POST', body: JSON.stringify(input) }),

  update: (id: number, input: NoteInput) =>
    request<Note>(`${BASE_URL}/${id}`, { method: 'PUT', body: JSON.stringify(input) }),

  remove: (id: number) => request<void>(`${BASE_URL}/${id}`, { method: 'DELETE' }),
}
