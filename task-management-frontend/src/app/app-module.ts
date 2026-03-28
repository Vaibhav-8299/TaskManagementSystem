import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; // VERY IMPORTANT

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskFormComponent } from './components/task-form/task-form.component';

@NgModule({
  declarations: [
    App,
    TaskListComponent,
    TaskFormComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    FormsModule,       // Required for ngModel to work in our forms
    HttpClientModule   // Required to make API calls to your .NET backend
  ],
  providers: [],
  bootstrap: [App]
})
export class AppModule { }
