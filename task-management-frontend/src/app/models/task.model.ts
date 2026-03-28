// Task model — mirrors the backend TaskDto exactly
export interface Task {
  id: number;
  title: string;
  description?: string;
  status?: string;
  createdAt?: string;
  updatedAt?: string;
}

// Used when creating a task — no ID needed
export interface CreateTask {
  title: string;
  description?: string;
  status?: string;
}

// Used when updating a task — same as Create
export interface UpdateTask {
  title: string;
  description?: string;
  status?: string;
}
