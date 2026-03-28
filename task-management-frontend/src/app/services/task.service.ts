import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTask, UpdateTask } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  // Backend API URL — matches your running .NET API
  private apiUrl = 'http://localhost:5222/api/tasks';

  constructor(private http: HttpClient) {}

  // GET /api/tasks — fetch all tasks
  getAllTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }

  // GET /api/tasks/{id} — fetch one task
  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  // POST /api/tasks — create a new task
  createTask(task: CreateTask): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  // PUT /api/tasks/{id} — update full task
  updateTask(id: number, task: UpdateTask): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, task);
  }

  // DELETE /api/tasks/{id} — delete task
  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  // PATCH /api/tasks/{id}/status — toggle status only
  updateStatus(id: number, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/status`, { status });
  }
}
