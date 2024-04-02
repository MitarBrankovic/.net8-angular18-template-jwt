import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToastersService {
  constructor(public toastr: ToastrService, private router: Router) {}

  showSuccess(message: string) {
    this.toastr.success(message, 'Success', {
      positionClass: 'toast-bottom-right',
    });
  }

  showError(message: string) {
    this.toastr.error(message, 'Error', {
      timeOut: 2000,
      positionClass: 'toast-bottom-right',
    });
  }

  showInfo(message: string) {
    this.toastr.info(message, 'Info', { positionClass: 'toast-bottom-right' });
  }

  handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      if (error && typeof error === 'object') {
        if (
          error.hasOwnProperty('error') &&
          typeof error.error[0] === 'string' &&
          error.error.length > 0
        ) {
          const errorMessage = this.extractErrorMessage(error.error[0]);
          this.showError(errorMessage);
        }

        if (
          error.hasOwnProperty('error') &&
          Array.isArray(error.error.errors) &&
          typeof error.error.errors[0] === 'string' &&
          error.error.errors.length > 0
        ) {
          const errorMessage = this.extractErrorMessage(error.error.errors[0]);
          this.showError(errorMessage);
        }

        if (
          error.hasOwnProperty('status') &&
          typeof error.status === 'number' &&
          isNaN(error.status) === false &&
          error.status === 401 &&
          error.hasOwnProperty('statusText') &&
          typeof error.statusText === 'string' &&
          error.statusText.length > 0 &&
          error.statusText.toLowerCase() === 'unauthorized'
        ) {
          if (typeof Storage !== 'undefined') localStorage.clear();
          this.router.navigate(['/login']);
        }
      }
    }
    // return an observable with a user-facing error message
    return throwError('Something bad happened; please try again later.');
  }

  private extractErrorMessage(fullErrorMessage: string): string {
    const regex = /(?:System\.Exception:)(.*?)(?=\s+at)/;
    const match = fullErrorMessage.match(regex);
    return match ? match[1].trim() : fullErrorMessage ?? 'Error occured.';
  }
}
