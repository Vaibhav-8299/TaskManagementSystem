import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css'],
  standalone: false
})
export class TaskListComponent implements OnInit {

  tasks: Task[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';

  constructor(
    private taskService: TaskService, 
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  // Load all tasks from API
  loadTasks(): void {
    this.isLoading = true;
    this.taskService.getAllTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.isLoading = false;
        this.cdr.detectChanges(); // Trigger update for zoneless Angular
      },
      error: (err) => {
        this.errorMessage = 'Failed to load tasks. Is the API running?';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // Navigate to Add Task page
  addTask(): void {
    this.router.navigate(['/tasks/new']);
  }

  // Navigate to Edit Task page
  editTask(id: number): void {
    this.router.navigate(['/tasks/edit', id]);
  }

  // Delete a task
  deleteTask(id: number): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.successMessage = 'Task deleted successfully!';
          this.loadTasks(); // Refresh list
          setTimeout(() => {
            this.successMessage = '';
            this.cdr.detectChanges();
          }, 3000);
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Failed to delete task.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  // Toggle task status between Pending and Completed
  toggleStatus(task: Task): void {
    const newStatus = task.status === 'Completed' ? 'Pending' : 'Completed';
    this.taskService.updateStatus(task.id, newStatus).subscribe({
      next: () => {
        task.status = newStatus; // Update locally without reloading
        this.successMessage = `Task marked as ${newStatus}!`;
        setTimeout(() => {
            this.successMessage = '';
            this.cdr.detectChanges();
        }, 3000);
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to update status.';
        this.cdr.detectChanges();
      }
    });
  }
}
