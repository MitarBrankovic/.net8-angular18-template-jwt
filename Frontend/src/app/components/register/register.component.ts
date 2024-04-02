import { Component } from '@angular/core';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { ToastersService } from 'src/app/services/toasters.service';
import { UserService } from 'src/app/services/user.service';
import { CommonModule, DatePipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
  ],
  providers: [DatePipe],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  fullNameFormControl = new FormControl('', Validators.required);
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  passwordFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(4),
  ]);

  dateOfBirthFormControl = new FormControl('', Validators.required);

  constructor(
    private userService: UserService,
    private toastersService: ToastersService,
    private datePipe: DatePipe,
    private router: Router
  ) {}

  ngOnInit(): void {}

  register() {
    if (!this.isFormValid()) {
      this.toastersService.showError('Please fill all fields');
      return;
    }
    this.userService
      .register({
        fullName: this.fullNameFormControl.value,
        email: this.emailFormControl.value,
        password: this.passwordFormControl.value,
        dateOfBirth: this.datePipe.transform(
          this.dateOfBirthFormControl.value,
          'yyyy-MM-dd'
        ),
        subjects: [],
      })
      .subscribe(
        (response: any) => {
          this.router.navigate(['/']);
          this.toastersService.showSuccess('Successfully registered');
        },
        (error: any) => {
          this.toastersService.handleError(error);
        }
      );
  }

  isFormValid() {
    return (
      this.fullNameFormControl.valid &&
      this.emailFormControl.valid &&
      this.passwordFormControl.valid &&
      this.dateOfBirthFormControl.valid
    );
  }
}
