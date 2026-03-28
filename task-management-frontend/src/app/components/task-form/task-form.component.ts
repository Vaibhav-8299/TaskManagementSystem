import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { CreateTask, UpdateTask } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css'],
  standalone: false
})
export class TaskFormComponent implements OnInit {

  task: CreateTask | UpdateTask = {
    title: '',
    description: '',
    status: 'Pending'
  };

  taskId: number | null = null;
  isEditMode = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    // Check if we are in 'edit' mode by looking for an ID in the URL
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.taskId = +id;
        this.isEditMode = true;
        this.loadTask(this.taskId);
      }
      this.cdr.detectChanges();
    });
  }

  // Load existing task data for editing
  loadTask(id: number): void {
    this.taskService.getTaskById(id).subscribe({
      next: (data) => {
        this.task = {
          title: data.title,
          description: data.description || '',
          status: data.status || 'Pending'
        };
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load task details.';
        this.cdr.detectChanges();
      }
    });
  }

  // Handle form submission
  onSubmit(): void {
    // Basic validation
    if (!this.task.title || this.task.title.trim() === '') {
      this.errorMessage = 'Title is required!';
      this.cdr.detectChanges();
      return;
    }

    this.isSaving = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    if (this.isEditMode && this.taskId) {
      // Update existing task
      const updateData: UpdateTask = {
        title: this.task.title,
        description: this.task.description,
        status: this.task.status
      };

      this.taskService.updateTask(this.taskId, updateData).subscribe({
        next: () => this.navigateBack(),
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to update task.';
          this.isSaving = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      // Create new task
      const createData: CreateTask = {
        title: this.task.title,
        description: this.task.description,
        status: this.task.status
      };

      this.taskService.createTask(createData).subscribe({
        next: () => this.navigateBack(),
        error: (err) => {
          this.errorMessage = err.error?.message || 'Failed to create task.';
          this.isSaving = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  // Go back to the list page
  navigateBack(): void {
    this.router.navigate(['/tasks']);
  }
}
