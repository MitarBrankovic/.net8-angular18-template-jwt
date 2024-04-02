import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FormatDatePipe } from './shared/pipes/format-date.pipe';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [CommonModule, RouterOutlet, NavbarComponent, FormatDatePipe],
})
export class AppComponent {
  title: any = 'Frontend';
  date: any = new Date();
}
