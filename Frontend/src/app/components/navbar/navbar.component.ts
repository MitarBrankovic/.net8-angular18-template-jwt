import {
  ChangeDetectionStrategy,
  Component,
  WritableSignal,
  computed,
  effect,
  signal,
} from '@angular/core';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from 'src/app/services/user.service';
import { CommonModule } from '@angular/common';
import { ToastersService } from 'src/app/services/toasters.service';
import { async } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatButtonModule, CommonModule, MatMenuModule, MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isLoggedIn$ = this.userService.isAuthenticated$;
  username$ = this.userService.username$;
  username: any;

  constructor(
    private router: Router,
    private userService: UserService,
    private toastersService: ToastersService
  ) {}

  ngOnInit() {
    this.username$.subscribe((response) => {
      this.username = signal<string>(response || '');
    });
    this.username = signal<string>(
      localStorage.getItem('Template_email') || ''
    );
    // if (!this.username) {
    //   this.username = signal<string>(
    //     localStorage.getItem('Template_email') || ''
    //   );
    // }
  }

  loginRedirect() {
    this.router.navigate(['/login']);
  }

  registerRedirect() {
    this.router.navigate(['/register']);
  }

  editUser() {
    this.router.navigate(['/edit-user']);
  }

  logout() {
    this.userService.logout().subscribe(
      (response: any) => {
        console.log(response);
        this.userService.setIsAuthenticated(false);
        this.userService.clearJwtToken();
        this.router.navigate(['/']);
      },
      (error: any) => {
        this.toastersService.handleError(error);
      }
    );
  }

  homeRedirect() {
    this.router.navigate(['/']);
  }
}
