import { CommonModule, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { ToastersService } from 'src/app/services/toasters.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-user',
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
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.scss',
})
export class EditUserComponent {
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

  ngOnInit() {
    this.userService
      .getByEmail(localStorage.getItem('Template_email')!)
      .subscribe(
        (response: any) => {
          this.fullNameFormControl.setValue(response.fullName);
          this.emailFormControl.setValue(response.email);
          this.dateOfBirthFormControl.setValue(response.dateOfBirth);
        },
        (error: any) => {
          this.toastersService.handleError(error);
        }
      );
  }

  editUser() {
    if (!this.isFormValid())
      return this.toastersService.showError('Please fill in all fields');
    const dto = {
      fullName: this.fullNameFormControl.value,
      email: this.emailFormControl.value,
      password: this.passwordFormControl.value,
      dateOfBirth: this.datePipe.transform(
        this.dateOfBirthFormControl.value,
        'yyyy-MM-dd'
      ),
    };

    this.userService.updateUser(dto).subscribe(
      (response: any) => {
        this.toastersService.showSuccess('User edited successfully');
        this.router.navigate(['/']);
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
